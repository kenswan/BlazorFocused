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
    public class FocusedHttp : HttpMessageHandler
    {
        public string BaseAddress { get; private set; }

        private readonly List<FocusedHttpRequest> requests;
        private readonly List<FocusedHttpResponse> responses;

        public FocusedHttp(string baseAddress = "http://test-url.io")
        {
            BaseAddress = baseAddress;
            requests = new List<FocusedHttpRequest>();
            responses = new List<FocusedHttpResponse>();
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var method = request.Method;
            var url = request.RequestUri.OriginalString;
            var content = (request.Content is not null) ?
                await request.Content.ReadAsStringAsync(cancellationToken) : string.Empty;

            requests.Add(new FocusedHttpRequest { Method = method, Content = content, Url = url });

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

        public FocusedHttpSetup Setup(HttpMethod method, string url)
        {
            var request = new FocusedHttpRequest { Method = method, Url = url };

            return new FocusedHttpSetup(request, Resolve);
        }

        private void Resolve(FocusedHttpRequest request, HttpStatusCode statusCode, object response)
        {
            var setupResponse = new FocusedHttpResponse
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
                    throw new FocusedTestException($"{method} - {url} was not requested");
            }
            else if (method is not null)
            {
                var match = requests
                    .Where(request => request.Method == method).FirstOrDefault();

                if (match is null)
                    throw new FocusedTestException($"{method} was not requested");
            }
            else
            {
                if (!requests.Any())
                    throw new FocusedTestException($"No request was made");
            }
        }

        private string GetFullUrl(string relativeUrl) =>
            new Uri(new Uri(BaseAddress), relativeUrl).ToString();
    }
}
