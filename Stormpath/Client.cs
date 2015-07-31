using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Stormpath
{
    /// <summary>
    /// Class used to store the ApiKey for use in the authorization header.
    /// </summary>
    public class Client
    {
        protected HttpClient WebClient { get; private set; }
        public LocalApiKey ApiKey { get; private set; }

        public Client( string id, string secret ): this( new LocalApiKey( id, secret ) ) { }
        public Client( LocalApiKey apiKey )
        {
            ApiKey = apiKey;
            WebClient = new HttpClient();
            WebClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue( "Basic", ApiKey.Base64EncodedValue );
        }

        public Client( LocalApiKey apiKey, DelegatingHandler httpHandler )
        {
            ApiKey = apiKey;
            WebClient = new HttpClient( httpHandler );
            WebClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue( "Basic", ApiKey.Base64EncodedValue );
        }

        protected async Task<T> GetResourceAsync<T>( string url ) where T : Resource
        {
            var request = new HttpRequestMessage( HttpMethod.Get, url );
            HttpResponseMessage response = await WebClient.SendAsync( request );
            var content = await response.Content.ReadAsStringAsync();

            if( response.IsSuccessStatusCode )
            {
                T resource = JsonConvert.DeserializeObject<T>( content );
                resource.SetClient( this );
                return resource;
            }
            else
            {
                throw new OperationErrorException( content );
            }
        }

        public async Task<Application> GetApplicationAsync( string appUrl )
        {
            return await GetResourceAsync<Application>( appUrl );
        }

        public async Task<Account> GetAccountAsync( string accountUrl )
        {
            return await GetResourceAsync<Account>( accountUrl );
        }

        public async Task<HttpResponseMessage> SendAsync( HttpRequestMessage request )
        { 
            return await WebClient.SendAsync( request );
        }

        public async Task<string> GetStringAsync( string url )
        {
            return await WebClient.GetStringAsync( url );
        }
    }    
}
