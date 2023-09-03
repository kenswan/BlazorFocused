// -------------------------------------------------------
// Copyright (c) Ken Swan All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using System.Net;
using System.Text.Json;

namespace BlazorFocused.Tools.Http;

internal partial class SimulatedHttp
{
    public ISimulatedHttpSetup SetupGET(string url) => Setup(HttpMethod.Get, url);

    public ISimulatedHttpSetup SetupDELETE(string url) => Setup(HttpMethod.Delete, url);

    public ISimulatedHttpSetup SetupPOST(string url, object content = null) => Setup(HttpMethod.Post, url, content);

    public ISimulatedHttpSetup SetupPATCH(string url, object content = null) => Setup(HttpMethod.Patch, url, content);

    public ISimulatedHttpSetup SetupPUT(string url, object content = null) => Setup(HttpMethod.Put, url, content);

    private ISimulatedHttpSetup Setup(HttpMethod method, string url, object content = null)
    {
        string requestString = content switch
        {
            null => null,
            { } when content is HttpContent httpContent => httpContent.ReadAsStringAsync().GetAwaiter().GetResult(),
            _ => JsonSerializer.Serialize(content)
        };

        var request = new SimulatedHttpRequest { Method = method, Url = url, RequestContent = requestString };

        return new SimulatedHttpSetup(request, Resolve);
    }

    private void Resolve(SimulatedHttpRequest request, HttpStatusCode statusCode, object response)
    {
        string responseString = response is not null ? JsonSerializer.Serialize(response) : null;

        var setupResponse = new SimulatedHttpResponse
        {
            Method = request.Method,
            Url = GetFullUrl(request.Url),
            StatusCode = statusCode,
            RequestContent = request?.RequestContent,
            ResponseContent = responseString
        };

        Responses.Add(setupResponse);
    }
}
