using System.Net;

namespace BlazorFocused.Tools.Client
{
    internal class SimulatedHttpResponse : SimulatedHttpRequest
    {
        public HttpStatusCode StatusCode { get; set; }

        public string ResponseContent { get; set; }
    }
}
