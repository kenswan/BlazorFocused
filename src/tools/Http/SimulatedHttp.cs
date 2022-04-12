namespace BlazorFocused.Tools.Http;

/// <inheritdoc cref="ISimulatedHttp"/>
internal partial class SimulatedHttp : ISimulatedHttp
{
    public DelegatingHandler DelegatingHandler => GetDelegatingHandler();

    public HttpClient HttpClient =>
        new(GetDelegatingHandler()) { BaseAddress = baseAddressUri };

    internal List<SimulatedHttpResponse> Responses => responses;

    private readonly List<SimulatedHttpHeaders> headers;
    private readonly List<SimulatedHttpRequest> requests;
    private readonly List<SimulatedHttpResponse> responses;
    private readonly Uri baseAddressUri;

    public SimulatedHttp(string baseAddress = "https://dev.blazorfocused.net")
    {
        headers = new();
        requests = new();
        responses = new();

        if (Uri.TryCreate(baseAddress, UriKind.Absolute, out Uri uri))
        {
            baseAddressUri = uri;
        }
        else
        {
            throw new SimulatedHttpTestException("Invalid base address was given");
        }
    }

    internal void AddRequest(SimulatedHttpRequest request)
    {
        requests.Add(request);
    }

    internal void StoreHeaders(SimulatedHttpHeaders requestHeaders)
    {
        headers.Add(requestHeaders);
    }

    private DelegatingHandler GetDelegatingHandler() =>
        new SimulatedVerificationHandler()
        {
            InnerHandler = new SimulatedRequestHandler(AddRequest)
            {
                InnerHandler = new SimulatedHeadersHandler(StoreHeaders)
                {
                    InnerHandler = new SimulatedResponseHandler(responses)
                }
            }
        };

    private string GetFullUrl(string relativeUrl) =>
        new Uri(baseAddressUri, relativeUrl).ToString();
}
