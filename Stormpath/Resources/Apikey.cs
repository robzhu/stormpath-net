using System;
using System.Text;

namespace Stormpath
{
    public class ApiKey : Resource
    {
        public enum ApiKeyStatus
        {
            ENABLED,
            DISABLED,
        }

        public string Id { get; set; }
        public string Secret { get; set; }
        public ApiKeyStatus Status { get; set; }
        public Hyperlink Account { get; set; }
        public Hyperlink Tenant { get; set; }

        private Lazy<string> _base64EncodedValue = null;
        public string Base64EncodedValue
        {
            get{ return _base64EncodedValue.Value; }
        }

        public ApiKey()
        {
            _base64EncodedValue = new Lazy<string>( () => 
            { 
                return Encoder.FormatAndBase64Encode( Id, Secret ); 
            } );
        }
    }
}
