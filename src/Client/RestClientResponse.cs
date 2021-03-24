using System;
using System.Net;

namespace BlazorFocused.Client
{
    public class RestClientResponse<T>
    {
        public Exception Exception { get; set; }

        public HttpStatusCode? StatusCode { get; set; }

        public bool IsValid { get; set; }

        public T Value { get; set; }
    }
}
