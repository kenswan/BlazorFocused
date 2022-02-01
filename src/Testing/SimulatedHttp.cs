using System.Net;

namespace BlazorFocused.Testing
{
    /// <inheritdoc cref="ISimulatedHttp"/>
    public class SimulatedHttp : ISimulatedHttp
    {
        public DelegatingHandler DelegatingHandler => GetDelegatingHandler();

        public HttpClient HttpClient =>
            new(GetDelegatingHandler()) { BaseAddress = baseAddressUri };

        internal List<SimulatedHttpResponse> Responses => responses;

        private readonly List<SimulatedHttpRequest> requests;
        private readonly List<SimulatedHttpResponse> responses;
        private readonly Uri baseAddressUri;

        public SimulatedHttp(string baseAddress = "https://dev.blazorfocused.net")
        {
            requests = new List<SimulatedHttpRequest>();
            responses = new List<SimulatedHttpResponse>();

            if (Uri.TryCreate(baseAddress, UriKind.Absolute, out Uri uri))
            {
                baseAddressUri = uri;
            }
            else
            {
                throw new SimulatedHttpTestException("Invalid base address was given");
            }
        }

        public ISimulatedHttpSetup SetupGET(string url)
        {
            return Setup(HttpMethod.Get, url);
        }

        public ISimulatedHttpSetup SetupDELETE(string url)
        {
            return Setup(HttpMethod.Delete, url);
        }

        public ISimulatedHttpSetup SetupPOST(string url, object content = null)
        {
            return Setup(HttpMethod.Post, url, content);
        }

        public ISimulatedHttpSetup SetupPATCH(string url, object content = null)
        {
            return Setup(HttpMethod.Patch, url, content);
        }

        public ISimulatedHttpSetup SetupPUT(string url, object content = null)
        {
            return Setup(HttpMethod.Put, url, content);
        }

        public void VerifyWasCalled(HttpMethod method = default, string url = default, object content = default)
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

        private ISimulatedHttpSetup Setup(HttpMethod method, string url, object content = null)
        {
            var request = new SimulatedHttpRequest { Method = method, Url = url, RequestContent = content };

            return new SimulatedHttpSetup(request, Resolve);
        }

        internal void AddRequest(SimulatedHttpRequest request)
        {
            requests.Add(request);
        }

        private void Resolve(SimulatedHttpRequest request, HttpStatusCode statusCode, object response)
        {
            var setupResponse = new SimulatedHttpResponse
            {
                Method = request.Method,
                Url = GetFullUrl(request.Url),
                StatusCode = statusCode,
                RequestContent = request?.RequestContent,
                ResponseContent = response
            };

            responses.Add(setupResponse);
        }

        private DelegatingHandler GetDelegatingHandler() =>
            new SimulatedVerificationHandler()
            {
                InnerHandler = new SimulatedRequestHandler(AddRequest)
                {
                    InnerHandler = new SimulatedResponseHandler(responses)
                }
            };

        private string GetFullUrl(string relativeUrl) =>
            new Uri(baseAddressUri, relativeUrl).ToString();
    }
}
