// -------------------------------------------------------
// Copyright (c) Ken Swan All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using Microsoft.Extensions.Logging;

namespace BlazorFocused.Client;
internal class RestClientHeaderHandler : DelegatingHandler
{
    private readonly IRestClientRequestHeaders clientRequestHeaders;
    private readonly ILogger<RestClientHeaderHandler> logger;

    public RestClientHeaderHandler(
        IRestClientRequestHeaders clientRequestHeaders,
        ILogger<RestClientHeaderHandler> logger)
    {
        this.clientRequestHeaders = clientRequestHeaders;
        this.logger = logger;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if (clientRequestHeaders.HasValues())
        {
            logger.LogDebug("Found header attributes to add to request");

            foreach (var headerKey in clientRequestHeaders.GetHeaderKeys())
            {
                foreach (var value in clientRequestHeaders.GetHeaderValues(headerKey))
                {
                    request.Headers.Add(headerKey, value);
                }
            }
        }
        else
        {
            logger.LogDebug("No header elements found");
        }

        return await base.SendAsync(request, cancellationToken);
    }
}
