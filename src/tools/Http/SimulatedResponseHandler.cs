using System.Net;
using System.Text;

namespace BlazorFocused.Tools.Http;

internal class SimulatedResponseHandler : DelegatingHandler
{
    private readonly List<SimulatedHttpResponse> responses;

    public SimulatedResponseHandler(List<SimulatedHttpResponse> responses)
    {
        this.responses = responses;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        (HttpMethod method, string url, string content) =
            await SimulatedHandler.GetRequestMessageContents(request, cancellationToken);

        var response = responses.Where(request =>
            request.Method == method &&
            string.Equals(request.Url, url, StringComparison.OrdinalIgnoreCase))
                .FirstOrDefault();

        return new HttpResponseMessage
        {
            StatusCode = response is not null ? response.StatusCode : HttpStatusCode.NotImplemented,
            Content = new StringContent(response?.ResponseContent ?? string.Empty, Encoding.UTF8, "application/json"),
            RequestMessage = request
        };
    }
}
