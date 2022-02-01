using System.Net;
using System.Text;
using System.Text.Json;

namespace BlazorFocused.Testing
{
    internal class SimulatedResponseHandler : DelegatingHandler
    {
        private readonly List<SimulatedHttpResponse> responses;

        public SimulatedResponseHandler(List<SimulatedHttpResponse> responses)
        {
            this.responses = responses;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            (HttpMethod method, string url, object content) =
                await SimulatedHandler.GetRequestMessageContents(request, cancellationToken);

            var response = responses.Where(request =>
                request.Method == method &&
                string.Equals(request.Url, url, StringComparison.OrdinalIgnoreCase))
                    .FirstOrDefault();

            return new HttpResponseMessage
            {
                StatusCode = response is not null ? response.StatusCode : HttpStatusCode.NotImplemented,
                Content = new StringContent(JsonSerializer.Serialize(response?.ResponseContent), Encoding.UTF8, "application/json"),
                RequestMessage = request
            };
        }
    }
}
