// -------------------------------------------------------
// Copyright (c) Ken Swan All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

namespace BlazorFocused.Tools.Http;

internal class SimulatedHandler
{
    public static async Task<(HttpMethod method, string url, string content)> GetRequestMessageContents(
        HttpRequestMessage request, CancellationToken cancellationToken)
    {
        HttpMethod method = request.Method;
        string url = request.RequestUri.OriginalString;

        string content = (request.Content is not null) ?
            await request.Content.ReadAsStringAsync(cancellationToken) : default;

        return (method, url, content);
    }
}
