using System.Net;
using System.Net.Http;

namespace BlazorFocused.Testing
{
    public class FocusedSetup
    {
        // TODO: Add Object compare to setup (POST & PUT)
        // public object Content { get; set; }

        public HttpMethod HttpMethod { get; set; }

        public string Url { get; set; }
    }
}
