using System;

namespace Stormpath
{
    public class LocalApiKey
    {
        public string Id { get; set; }
        public string Secret { get; set; }

        private Lazy<string> _base64EncodedValue = null;
        public string Base64EncodedValue
        {
            get{ return _base64EncodedValue.Value; }
        }

        public LocalApiKey( string id, string secret )
        {
            Id = id;
            Secret = secret;
            _base64EncodedValue = new Lazy<string>( () => 
            { 
                return Encoder.FormatAndBase64Encode( Id, Secret ); 
            } );
        }
    }
}
