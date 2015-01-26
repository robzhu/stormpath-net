using System.Net.Http;
using System.Threading.Tasks;

namespace Stormpath
{
    public class Account : Resource
    {
        public enum AccountStatus
        {
            ENABLED,
            UNVERIFIED,
            DISABLED,
        }

        public string Username { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string GivenName { get; set; }
        public string Surname { get; set; }
        public AccountStatus Status { get; set; }

        public Hyperlink CustomData { get; set; }
        public ResourceList<Group> Groups { get; set; }
        public Hyperlink GroupMemberships { get; set; }
        public Hyperlink Directory { get; set; }
        public Hyperlink Tenant { get; set; }
        //public Hyperlink EmailVerificationToken { get; set; }
        public ResourceList<ApiKey> ApiKeys { get; set; }

        public async Task DeleteAsync()
        {
            var request = new HttpRequestMessage( HttpMethod.Delete, Href );
            var response = await Client.SendAsync( request );
            var content = await response.Content.ReadAsStringAsync();

            if( !response.IsSuccessStatusCode )
            {
                throw new OperationErrorException( content );
            }
        }

        public async Task<ApiKey> CreateApiKeyAsync()
        {
            var request = new HttpRequestMessage( HttpMethod.Post, ApiKeys.Href );
            var response = await Client.SendAsync( request );
            var content = await response.Content.ReadAsStringAsync();

            if( response.IsSuccessStatusCode )
            {
                return DeserializeJson<ApiKey>( content );
            }
            else
            {
                throw new OperationErrorException( content );
            }
        }

        public string GetIdFromHref()
        {
            return Href.Substring( Href.LastIndexOf( "/" ) + 1 );
        }
    }
}
