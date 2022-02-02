using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

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
            var requestString = content is not null ? JsonSerializer.Serialize(content) : null;

            SimulatedHttpRequest simulatedRequest = new()
            {
                Method = method,
                Url = url,
                RequestContent = requestString
            };

            var invalidCheck = simulatedRequest switch
            {
                { Method: null, Url: null, RequestContent: null } => CheckAny(),
                { Url: null, RequestContent: null } => CheckMethod(simulatedRequest),
                { RequestContent: null } => CheckMethodAndUrl(simulatedRequest),
                { } when simulatedRequest is not null => CheckFullRequest(simulatedRequest),
                _ => throw new SimulatedHttpTestException("Improper type of validation request")
            };

            if (!string.IsNullOrEmpty(invalidCheck))
                throw new SimulatedHttpTestException($"{invalidCheck} was not requested");
        }

        private ISimulatedHttpSetup Setup(HttpMethod method, string url, object content = null)
        {
            var requestString = content is not null ? JsonSerializer.Serialize(content) : null;

            var request = new SimulatedHttpRequest { Method = method, Url = url, RequestContent = requestString };

            return new SimulatedHttpSetup(request, Resolve);
        }

        internal void AddRequest(SimulatedHttpRequest request)
        {
            requests.Add(request);
        }

        private void Resolve(SimulatedHttpRequest request, HttpStatusCode statusCode, object response)
        {
            var responseString = response is not null ? JsonSerializer.Serialize(response) : null;

            var setupResponse = new SimulatedHttpResponse
            {
                Method = request.Method,
                Url = GetFullUrl(request.Url),
                StatusCode = statusCode,
                RequestContent = request?.RequestContent,
                ResponseContent = responseString
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

        private string CheckFullRequest(SimulatedHttpRequest simulatedHttpRequest)
        {
            var method = simulatedHttpRequest.Method;
            var url = simulatedHttpRequest.Url;

            var matches = requests
                    .Where(request => request.Method == method && request.Url == GetFullUrl(url));

            if (!matches.Any())
                return $"Method {method} & Url {url}";

            foreach (var match in matches)
            {
                var compareEquals = string.Equals(
                    match.RequestContent,
                    simulatedHttpRequest.RequestContent,
                    StringComparison.InvariantCultureIgnoreCase);

                if (compareEquals)
                    return string.Empty;
            }

            return "Request Object";
        }

        private string CheckMethodAndUrl(SimulatedHttpRequest simulatedHttpRequest)
        {
            var method = simulatedHttpRequest.Method;
            var url = simulatedHttpRequest.Url;

            var match = requests
                    .Where(request => request.Method == method && request.Url == GetFullUrl(url))
                    .FirstOrDefault();

            return match is not null ? string.Empty : $"Method {method} & Url {url}";
        }

        private string CheckMethod(SimulatedHttpRequest simulatedHttpRequest)
        {
            var method = simulatedHttpRequest.Method;

            var match = requests.Where(request => request.Method == method).FirstOrDefault();

            return match is not null ? String.Empty : $"Method {method}";
        }

        private string CheckAny() =>
            !requests.Any() ? "Generic (Any) Request" : string.Empty;
    }
}
