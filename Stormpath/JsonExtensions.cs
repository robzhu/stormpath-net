using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Stormpath
{
    public static class JsonExtensions
    {
        public static string ToCamelCase( this string value )
        {
            return value.Substring( 0, 1 ).ToLower() + value.Substring( 1 );
        }

        public static string GetJsonElementValue( this string document, string elementName )
        {
            JObject obj = JObject.Parse( document );
            JToken token = null;
            if( obj.TryGetValue( elementName.ToCamelCase(), out token ) )
            {
                return token.ToString();
            }
            else if( obj.TryGetValue( elementName, out token ) )
            {
                return token.ToString();
            }
            else return null;
        }

        public static T GetJsonElementValue<T>( this string document, string elementName )
        {
            var elementJson = GetJsonElementValue( document, elementName );
            return JsonConvert.DeserializeObject<T>( elementJson );
        }
    }
}
