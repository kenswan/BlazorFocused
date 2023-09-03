// -------------------------------------------------------
// Copyright (c) Ken Swan All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

namespace BlazorFocused.Tools.Http;

internal class SimulatedRequestHandler : DelegatingHandler
{
    private readonly Action<SimulatedHttpRequest> addRequest;

    public SimulatedRequestHandler(Action<SimulatedHttpRequest> addRequest)
    {
        this.addRequest = addRequest;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        (HttpMethod method, string url, string content) =
            await SimulatedHandler.GetRequestMessageContents(request, cancellationToken);

        addRequest(new SimulatedHttpRequest { Method = method, Url = url, RequestContent = content });

        return await base.SendAsync(request, cancellationToken);
    }
}
