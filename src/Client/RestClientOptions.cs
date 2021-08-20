using System.Collections.Generic;

namespace BlazorFocused.Client
{
    internal class RestClientOptions
    {
        public string BaseAddress { get; set; }

        public Dictionary<string, string> DefaultRequestHeaders { get; set; } = new();

        public long? MaxResponseContentBufferSize { get; set; }

        public double? Timeout { get; set; }
    }
}
