using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using NativeHttpClient = System.Net.Http.HttpClient;

namespace BlazorFocused.Testing
{
    public class SimulatedHttp : HttpMessageHandler
    {
        public string BaseAddress { get; private set; }

        private readonly List<SimulatedHttpRequest> requests;
        private readonly List<SimulatedHttpResponse> responses;

        public SimulatedHttp(string baseAddress = "http://test-url.io")
        {
            BaseAddress = baseAddress;
            requests = new List<SimulatedHttpRequest>();
            responses = new List<SimulatedHttpResponse>();
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var method = request.Method;
            var url = request.RequestUri.OriginalString;
            var content = (request.Content is not null) ?
                await request.Content.ReadAsStringAsync(cancellationToken) : string.Empty;

            requests.Add(new SimulatedHttpRequest { Method = method, Content = content, Url = url });

            var response = responses.Where(request =>
                request.Method == method &&
                string.Equals(request.Url, url, StringComparison.OrdinalIgnoreCase))
                    .FirstOrDefault();

            return new HttpResponseMessage
            {
                StatusCode = response is not null ? response.StatusCode : HttpStatusCode.NotImplemented,
                Content = new StringContent(JsonSerializer.Serialize(response?.Response), Encoding.UTF8, "application/json")
            };
        }

        public NativeHttpClient Client()
        {
            return new NativeHttpClient(this)
            {
                BaseAddress = new Uri(BaseAddress)
            };
        }

        public SimulatedHttpSetup Setup(HttpMethod method, string url)
        {
            var request = new SimulatedHttpRequest { Method = method, Url = url };

            return new SimulatedHttpSetup(request, Resolve);
        }

        private void Resolve(SimulatedHttpRequest request, HttpStatusCode statusCode, object response)
        {
            var setupResponse = new SimulatedHttpResponse
            {
                Method = request.Method,
                Url = GetFullUrl(request.Url),
                StatusCode = statusCode,
                Response = response
            };

            responses.Add(setupResponse);
        }

        public void VerifyWasCalled(HttpMethod method = default, string url = default)
        {
            if (method is not null && url is not null)
            {
                var match = requests
                    .Where(request => request.Method == method && request.Url == GetFullUrl(url))
                    .FirstOrDefault();

                if (match is null)
                    throw new SimulatedHttpTestException($"{method} - {url} was not requested");
            }
            else if (method is not null)
            {
                var match = requests
                    .Where(request => request.Method == method).FirstOrDefault();

                if (match is null)
                    throw new SimulatedHttpTestException($"{method} was not requested");
            }
            else
            {
                if (!requests.Any())
                    throw new SimulatedHttpTestException($"No request was made");
            }
        }

        private string GetFullUrl(string relativeUrl) =>
            new Uri(new Uri(BaseAddress), relativeUrl).ToString();
    }
}
