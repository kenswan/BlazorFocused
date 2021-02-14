using System.Net;
using System.Net.Http;

namespace BlazorFocused.Testing
{
    public class FocusedSetup
    {
        public object Content { get; set; }

        public HttpMethod HttpMethod { get; set; }

        public string Url { get; set; }
    }
}
