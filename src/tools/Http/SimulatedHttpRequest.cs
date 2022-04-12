namespace BlazorFocused.Tools.Http;

internal class SimulatedHttpRequest
{
    public HttpMethod Method { get; set; }

    public string Url { get; set; }

    public string RequestContent { get; set; }
}
