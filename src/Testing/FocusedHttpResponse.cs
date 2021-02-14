using System.Net;

namespace BlazorFocused.Testing
{
    internal class FocusedHttpResponse : FocusedHttpRequest
    {
        public HttpStatusCode StatusCode { get; set; }

        public object Response { get; set; }
    }
}
