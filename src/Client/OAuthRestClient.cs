using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace BlazorFocused.Client
{
    internal class OAuthRestClient : RestClient, IOAuthRestClient
    {
        private OAuthToken oAuthToken;

        public OAuthRestClient(OAuthToken oAuthToken, HttpClient httpClient, ILogger<OAuthRestClient> logger) :
            base(httpClient, logger)
        {
            this.oAuthToken = oAuthToken;
        }

        public void AddAuthorization(string scheme, string token)
        {
            lock (oAuthToken)
            {
                oAuthToken.Update(scheme, token);
            }
        }

        public void ClearAuthorization() =>
            oAuthToken.Update("", "");

        public bool HasAuthorization() =>
            !oAuthToken.IsEmpty();

        public string RetrieveAuthorization() =>
            oAuthToken.ToString();

        protected override Task<RestClientResponse<T>> SendAsync<T>(HttpMethod method, string url, object data = null)
        {
            if (!oAuthToken.IsEmpty())
            {
                httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue(oAuthToken.Scheme, oAuthToken.Token);
            }
            else
            {
                logger.LogInformation("OAuth token has not been configured for rest client");
            }

            return base.SendAsync<T>(method, url, data);
        }
    }
}
