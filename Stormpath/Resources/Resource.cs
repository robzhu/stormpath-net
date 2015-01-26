using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Stormpath
{
    public enum ResourceStatus
    {
        ENABLED,
        DISABLED,
    }

    public abstract class Resource
    {
        public Client Client { get; private set; }
        public string Href { get; set; }

        protected T DeserializeJson<T>( string jsonBlob ) where T : Resource
        {
            T value = JsonConvert.DeserializeObject<T>( jsonBlob );
            value.SetClient( Client );
            return value;
        }

        public void SetClient( Client client )
        {
            Client = client;
            SetClientOnAllChildResourceProperties( client );
        }

        private void SetClientOnAllChildResourceProperties( Client client )
        {
            foreach( var property in GetType().GetProperties( BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly ) )
            {
                //if the property is also a resource, set its client.
                if( property.PropertyType.IsSubclassOf( typeof( Resource ) ) )
                {
                    Resource childResource = (Resource)property.GetValue( this );
                    if( childResource != null )
                    {
                        childResource.SetClient( client );
                    }
                }
            }
        }

        public void Expand( string json )
        {
            foreach( var property in GetType().GetProperties( BindingFlags.Public | BindingFlags.Instance ) )
            {
                //for each property on this class, try to deserialize the json blob value into it
                var jsonElementValue = json.GetJsonElementValue( property.Name );
                if( jsonElementValue == null ) continue;

                if( property.PropertyType == typeof( string ) )
                {
                    property.SetValue( this, Convert.ChangeType( jsonElementValue, property.PropertyType ) );
                }
                else if( property.PropertyType.IsEnum )
                {
                    property.SetValue( this, Enum.Parse( property.PropertyType, jsonElementValue ) );
                }
                else
                {
                    var value = JsonConvert.DeserializeObject( jsonElementValue, property.PropertyType );
                    property.SetValue( this, value );
                }
            }
        }

        public async Task ExpandAsync()
        {
            var jsonBlob = await Client.GetStringAsync( Href );
            Expand( jsonBlob );
        }

        public override string ToString()
        {
            return Href;
        }
    }

    public class ResourceList<T> : Resource where T : Resource
    {
        public int Offset { get; set; }
        public int Limit { get; set; }
        public int Size { get; set; }
        public List<T> Items { get; set; }
    }
}
