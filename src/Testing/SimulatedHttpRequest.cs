using System.Net.Http;

namespace BlazorFocused.Testing
{
    public class SimulatedHttpRequest
    {
        public HttpMethod Method { get; set; }

        public string Url { get; set; }

        public string Content { get; set; }
    }
}
