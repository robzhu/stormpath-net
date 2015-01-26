using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Stormpath
{
    public class Application : Resource
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public ResourceStatus Status { get; set; }

        public Tenant Tenant { get; set; }
        public Hyperlink DefaultAccountStoreMapping { get; set; }
        public Hyperlink DefaultGroupStoreMapping { get; set; }
        public Hyperlink CustomData { get; set; }
        public Hyperlink Accounts { get; set; }
        public Hyperlink Groups { get; set; }
        public Hyperlink AccountStoreMappings { get; set; }
        public Hyperlink LoginAttempts { get; set; }
        public Hyperlink PasswordResetTokens { get; set; }
        public Hyperlink ApiKeys { get; set; }
        public Hyperlink VerificationEmails { get; set; }

        public async Task<Account> CreateAccountAsync( string givenName, string surname, string username, string email, string password )
        {
            var request = HttpRequestFactory.CreatePostJsonRequest( Accounts.Href, new
            {
                givenName = givenName,
                surname = surname,
                username = username,
                email = email,
                password = password,
            } );

            HttpResponseMessage response = await Client.SendAsync( request );
            var content = await response.Content.ReadAsStringAsync();

            if( response.IsSuccessStatusCode )
            {
                return DeserializeJson<Account>( content );
            }
            else
            {
                throw new OperationErrorException( content );
            }
        }

        public async Task<Account> AttemptLoginAsync( string username, string password )
        {
            var loginAttempUrl = string.Format( "{0}?expand=account", LoginAttempts.Href );
            var request = HttpRequestFactory.CreatePostJsonRequest( loginAttempUrl, new
            {
                type = "basic",
                value = Encoder.FormatAndBase64Encode( username, password ),
            } );

            HttpResponseMessage response = await Client.SendAsync( request );
            var jsonDocument = await response.Content.ReadAsStringAsync();

            if( response.IsSuccessStatusCode )
            {
                var accountJson = jsonDocument.GetJsonElementValue( "account" );
                var account = JsonConvert.DeserializeObject<Account>( accountJson );
                account.SetClient( this.Client );
                return account;
            }
            else
            {
                throw new OperationErrorException( jsonDocument );
            }
        }

        public async Task<Account> AuthenticateApiKeyAsync( string id, string secret )
        {
            //Get the account associated with the key id.
            var authUrl = string.Format( "{0}?id={1}&expand=account", ApiKeys.Href, id );
            var request = new HttpRequestMessage( HttpMethod.Get, authUrl );

            HttpResponseMessage response = await Client.SendAsync( request );
            var contentBody = await response.Content.ReadAsStringAsync();

            if( response.IsSuccessStatusCode )
            {
                JObject obj = JObject.Parse( contentBody );
                int count = (int)obj[ "size" ];
                if( count == 0 )
                {
                    throw new OperationErrorException( new OperationError()
                    {
                        Code = 5000,
                        DeveloperMessage = "The API Key is not valid.",
                    } );
                }

                JToken token = obj[ "items" ][ 0 ];
                ApiKey apiKey = JsonConvert.DeserializeObject<ApiKey>( token.ToString() );

                //check to see if the secret matches that of the API Key
                if( apiKey.Secret == secret )
                {
                    var account = JsonConvert.DeserializeObject<Account>( token[ "account" ].ToString() );
                    account.SetClient( Client );
                    return account;
                }
                else
                {
                    throw new OperationErrorException( new OperationError()
                    {
                        Code = 5000,
                        DeveloperMessage = "The API Key is not valid.",
                    } );
                }
            }
            else
            {
                throw new OperationErrorException( contentBody );
            }
        }

        public async Task<bool> AddAccountToGroupAsync( Account account, string groupUrl )
        {
            var groupMembershipUrl = "https://api.stormpath.com/v1/groupMemberships";

            var request = HttpRequestFactory.CreatePostJsonRequest( groupMembershipUrl, new
            {
                account = new 
                {
                    href = account.Href,
                },
                group = new
                {
                    href = groupUrl
                }
            } );

            HttpResponseMessage response = await Client.SendAsync( request );

            if( response.IsSuccessStatusCode )
            {
                await account.Groups.ExpandAsync();
                return true;
            }
            else
            {
                throw new OperationErrorException( new OperationError()
                {
                    Code = 6000,
                    DeveloperMessage = string.Format( "Could not add user {0} to group {1}.", account.Username, groupUrl ),
                } );
            }
        }
    }
}
