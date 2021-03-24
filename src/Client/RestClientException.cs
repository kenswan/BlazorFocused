using System;
using System.Net;
using System.Net.Http;

namespace BlazorFocused.Client
{
    public class RestClientException : Exception
    {
        public RestClientException(string message) : base(message) { }

        public RestClientException(string message, Exception exception) : base(message, exception) { }
        
        public RestClientException(HttpMethod method, HttpStatusCode httpStatusCode, string url) :
            base($"{method} request failed with {httpStatusCode} at {url}") { }
    }
}
