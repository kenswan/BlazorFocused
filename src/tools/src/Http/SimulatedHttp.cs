// -------------------------------------------------------
// Copyright (c) Ken Swan All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

namespace BlazorFocused.Tools.Http;

/// <inheritdoc cref="ISimulatedHttp"/>
internal partial class SimulatedHttp : ISimulatedHttp
{
    public DelegatingHandler DelegatingHandler => GetDelegatingHandler();

    public HttpClient HttpClient =>
        new(GetDelegatingHandler()) { BaseAddress = baseAddressUri };

    internal List<SimulatedHttpResponse> Responses { get; }
    internal Dictionary<string, List<string>> ResponseHeaders { get; }

    private readonly List<SimulatedHttpHeaders> requestHeaders;
    private readonly List<SimulatedHttpRequest> requests;
    private readonly Uri baseAddressUri;

    public SimulatedHttp(string baseAddress = "https://dev.blazorfocused.net")
    {
        requestHeaders = new();
        ResponseHeaders = new();
        requests = new();
        Responses = new();

        baseAddressUri = Uri.TryCreate(baseAddress, UriKind.Absolute, out Uri uri)
            ? uri
            : throw new SimulatedHttpTestException("Invalid base address was given");
    }

    internal void AddRequest(SimulatedHttpRequest request) => requests.Add(request);

    internal void StoreHeaders(SimulatedHttpHeaders requestHeaders) => this.requestHeaders.Add(requestHeaders);

    private DelegatingHandler GetDelegatingHandler() => new SimulatedVerificationHandler()
    {
        InnerHandler = new SimulatedRequestHandler(AddRequest)
        {
            InnerHandler = new SimulatedHeadersHandler(StoreHeaders)
            {
                InnerHandler = new SimulatedResponseHandler(Responses, ResponseHeaders)
            }
        }
    };

    private string GetFullUrl(string relativeUrl) => new Uri(baseAddressUri, relativeUrl).ToString();
}
