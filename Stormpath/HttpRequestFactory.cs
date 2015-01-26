using System.Net.Http;

namespace Stormpath
{
    public static class HttpRequestFactory 
    {
        public static HttpRequestMessage CreatePostJsonRequest( string uri, object data )
        {
            var request = new HttpRequestMessage( HttpMethod.Post, uri );
            request.AcceptJson();
            request.SetJsonContent( data );
            return request;
        }
    }
}
