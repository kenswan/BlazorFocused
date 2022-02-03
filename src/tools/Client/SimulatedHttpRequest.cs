namespace BlazorFocused.Tools.Client
{
    internal class SimulatedHttpRequest
    {
        public HttpMethod Method { get; set; }

        public string Url { get; set; }

        public string RequestContent { get; set; }
    }
}
