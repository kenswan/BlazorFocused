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

        private readonly List<FocusedRequest> requests;
        private readonly List<FocusedResponse> responses;

        public FocusedHttp(string baseAddress = "http://test-url.io")
        {
            BaseAddress = baseAddress;
            requests = new List<FocusedRequest>();
            responses = new List<FocusedResponse>();
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var requestMethod = request.Method;
            var requestUrl = request.RequestUri.OriginalString;
            var requestContent = (request.Content is not null) ?
                await request.Content.ReadAsStringAsync(cancellationToken) : string.Empty;

            requests.Add(new FocusedRequest { Method = requestMethod, Content = requestContent, Url = requestUrl });

            var response = responses.Where(request =>
                request.RequestedMethod == requestMethod &&
                string.Equals(request.RequestedUrl, requestUrl, StringComparison.OrdinalIgnoreCase))
                    .FirstOrDefault();

            return new HttpResponseMessage
            {
                StatusCode = response is not null ? response.ResponseStatusCode : HttpStatusCode.NotImplemented,
                Content = new StringContent(JsonSerializer.Serialize(response?.ResponseObject), Encoding.UTF8, "application/json")
            };
        }

        public NativeHttpClient Client()
        {
            return new NativeHttpClient(this)
            {
                BaseAddress = new Uri(BaseAddress)
            };
        }

        public void Setup(Func<FocusedSetup, FocusedSetup> httpSetup, HttpStatusCode httpStatusCode, object response)
        {
            var setup = httpSetup(new FocusedSetup());

            var setupResponse = new FocusedResponse
            {
                RequestedMethod = setup.HttpMethod,
                RequestedUrl = GetFullUrl(setup.Url),
                ResponseStatusCode = httpStatusCode,
                ResponseObject = response
            };

            responses.Add(setupResponse);
        }

        public void VerifyWasCalled(HttpMethod httpMethod = default, string url = default)
        {
            if (httpMethod is not null && url is not null)
            {
                var match = requests
                    .Where(request => request.Method == httpMethod && request.Url == GetFullUrl(url))
                    .FirstOrDefault();

                if (match is null)
                    throw new FocusedTestException($"{httpMethod} - {url} was not requested");
            }
            else if (httpMethod is not null)
            {
                var match = requests
                    .Where(request => request.Method == httpMethod).FirstOrDefault();

                if (match is null)
                    throw new FocusedTestException($"{httpMethod} was not requested");
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
