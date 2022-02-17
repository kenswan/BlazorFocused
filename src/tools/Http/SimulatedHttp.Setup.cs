using System.Net;
using System.Text.Json;

namespace BlazorFocused.Tools.Http
{
    internal partial class SimulatedHttp
    {
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

        private ISimulatedHttpSetup Setup(HttpMethod method, string url, object content = null)
        {
            var requestString = content is not null ? JsonSerializer.Serialize(content) : null;

            var request = new SimulatedHttpRequest { Method = method, Url = url, RequestContent = requestString };

            return new SimulatedHttpSetup(request, Resolve);
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
    }
}
