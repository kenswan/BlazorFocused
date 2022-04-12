using System.Net;

namespace BlazorFocused.Tools.Http;

internal class SimulatedHttpResponse : SimulatedHttpRequest
{
    public HttpStatusCode StatusCode { get; set; }

    public string ResponseContent { get; set; }
}
