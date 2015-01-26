using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;

namespace Stormpath
{
    public static class HttpRequestMessageExtensions
    {
        public static void SetJsonContent( this HttpRequestMessage request, object data )
        {
            var json = JsonConvert.SerializeObject( data );
            request.Content = new StringContent( json, Encoding.UTF8, "application/json" );
        }

        public static void AcceptJson( this HttpRequestMessage request )
        {
            request.Headers.Accept.Add( new MediaTypeWithQualityHeaderValue( "application/json" ) );
        }

        public static string GetRawString( this HttpRequestMessage request )
        {
            StringBuilder sb = new StringBuilder();

            //request line
            sb.AppendFormat( "{0} {1} HTTP/{2}\n", request.Method.ToString(), request.RequestUri.ToString(), request.Version );

            //foreach( var header in request.Headers.
            sb.AppendLine( request.Headers.ToString() );
            sb.AppendLine( request.Content.ReadAsStringAsync().Result );

            return sb.ToString();
        }
    }
}
