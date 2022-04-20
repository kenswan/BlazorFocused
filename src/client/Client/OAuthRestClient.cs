using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BlazorFocused.Client;

/// <inheritdoc cref="IOAuthRestClient"/>
internal class OAuthRestClient : RestClient, IOAuthRestClient
{
    private readonly IOAuthToken oAuthToken;

    public OAuthRestClient(
        IOAuthToken oAuthToken,
        HttpClient httpClient,
        IOptionsSnapshot<RestClientOptions> restClientOptions,
        IRestClientRequestHeaders requestHeaders,
        ILogger<OAuthRestClient> logger) :
            base(httpClient, DetectOptions(restClientOptions), requestHeaders, logger)
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
        oAuthToken.Update(string.Empty, string.Empty);

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
