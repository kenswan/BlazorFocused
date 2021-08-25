using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;

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
        private readonly string baseAddress;
        private readonly Uri baseAddressUri;

        public SimulatedHttp(string baseAddress = "https://dev.blazorfocused.net")
        {
            requests = new List<SimulatedHttpRequest>();
            responses = new List<SimulatedHttpResponse>();

            if (Uri.TryCreate(baseAddress, UriKind.Absolute, out Uri uri))
            {
                this.baseAddress = baseAddress;
                baseAddressUri = uri;
            }
            else
            {
                throw new SimulatedHttpTestException("Invalid base address was given");
            }
        }

        public ISimulatedHttpSetup Setup(HttpMethod method, string url)
        {
            var request = new SimulatedHttpRequest { Method = method, Url = url };

            return new SimulatedHttpSetup(request, Resolve);
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
                Response = response
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
            new Uri(new Uri(baseAddress), relativeUrl).ToString();
    }
}
