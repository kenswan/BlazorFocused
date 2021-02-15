using System.Net.Http;

namespace BlazorFocused.Testing
{
    internal class SimulatedHttpRequest
    {
        public HttpMethod Method { get; set; }

        public string Url { get; set; }

        public string Content { get; set; }
    }
}
