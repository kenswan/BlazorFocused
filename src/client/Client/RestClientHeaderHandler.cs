using Microsoft.Extensions.Logging;

namespace BlazorFocused.Client;
internal class RestClientHeaderHandler : DelegatingHandler
{
    private readonly RestClientRequestHeaders clientRequestHeaders;
    private readonly ILogger<RestClientHeaderHandler> logger;

    public RestClientHeaderHandler(
        RestClientRequestHeaders clientRequestHeaders,
        ILogger<RestClientHeaderHandler> logger)
    {
        this.clientRequestHeaders = clientRequestHeaders;
        this.logger = logger;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if (clientRequestHeaders.HeaderCache.Any())
        {
            logger.LogDebug("Found header attributes to add to request");
        }
        else
        {
            logger.LogDebug("No header elements found");
        }

        return await base.SendAsync(request, cancellationToken);
    }
}
