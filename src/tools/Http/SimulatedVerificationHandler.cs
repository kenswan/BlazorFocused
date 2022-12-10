// -------------------------------------------------------
// Copyright (c) Ken Swan All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

namespace BlazorFocused.Tools.Http;

internal class SimulatedVerificationHandler : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        (HttpMethod _, var url, var _) =
            await SimulatedHandler.GetRequestMessageContents(request, cancellationToken);

        return !Uri.TryCreate(url, UriKind.RelativeOrAbsolute, out Uri _)
            ? throw new SimulatedHttpTestException($"Url is not propert Uri: {url}")
            : await base.SendAsync(request, cancellationToken);
    }
}
