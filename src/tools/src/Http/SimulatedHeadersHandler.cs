// -------------------------------------------------------
// Copyright (c) Ken Swan All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

namespace BlazorFocused.Tools.Http;

internal class SimulatedHeadersHandler : DelegatingHandler
{
    private readonly Action<SimulatedHttpHeaders> storeHeaders;

    public SimulatedHeadersHandler(Action<SimulatedHttpHeaders> storeHeaders)
    {
        this.storeHeaders = storeHeaders;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var headers = request.Headers.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

        (HttpMethod method, string url, string content) =
            await SimulatedHandler.GetRequestMessageContents(request, cancellationToken);

        var requestHeaders = new SimulatedHttpHeaders
        {
            Method = method,
            Url = url,
            Headers = headers
        };

        storeHeaders(requestHeaders);

        return await base.SendAsync(request, cancellationToken);
    }
}
