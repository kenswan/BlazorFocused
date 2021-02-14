using System.Net;
using System.Net.Http;

namespace BlazorFocused.Testing
{
    public class FocusedResponse
    {
        public HttpMethod RequestedMethod { get; set; }

        public string RequestedUrl { get; set; }

        public HttpStatusCode ResponseStatusCode { get; set; }

        public object ResponseObject { get; set; }
    }
}
