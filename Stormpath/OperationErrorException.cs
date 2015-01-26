using System;
using Newtonsoft.Json;

namespace Stormpath
{
    /// <summary>
    /// The request was received and processed by the Stormpath server, but an error occurred. See the Error property for more details.
    /// </summary>
    public class OperationErrorException : Exception
    {
        public OperationError Error { get; set; }

        public OperationErrorException() : base() { }
        public OperationErrorException( string contentBody ) : this( JsonConvert.DeserializeObject<OperationError>( contentBody ) ) { }
        public OperationErrorException( OperationError error ) : base( error.DeveloperMessage )
        {
            Error = error;
        }
    }
}
