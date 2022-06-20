using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BlazorFocused.Client;

internal class StandaloneClient : RestClient
{
    public StandaloneClient(HttpClient httpClient, ILogger<RestClient> logger)
        : base(httpClient, Options.Create<RestClientOptions>(null), new RestClientRequestHeaders(), logger)
    { }
}
