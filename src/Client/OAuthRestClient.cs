using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net.Http;

namespace BlazorFocused.Client
{
    internal class OAuthRestClient : RestClient, IOAuthRestClient
    {
        private OAuthToken oAuthToken;

        public OAuthRestClient(
            OAuthToken oAuthToken,
            HttpClient httpClient,
            IOptionsSnapshot<RestClientOptions> restClientOptions,
            ILogger<OAuthRestClient> logger) :
                base(httpClient, DetectOptions(restClientOptions), logger)
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

        private static IOptions<RestClientOptions> DetectOptions(IOptionsSnapshot<RestClientOptions> restClientOptions)
        {
            var oAuthRestClientOptions = restClientOptions?.Get(nameof(OAuthRestClient));

            return (oAuthRestClientOptions is not null && oAuthRestClientOptions.IsConfigured) ?
                Options.Create(oAuthRestClientOptions) : restClientOptions;
        }

    }
}
