using System;
using System.Net;
using System.Net.Http;

namespace BlazorFocused.Client
{
    /// <summary>
    /// Gives result of an http request using <see cref="IRestClient"/>
    /// </summary>
    /// <typeparam name="T">
    /// Value expected from <see cref="HttpResponseMessage.Content"/> deserialization
    /// </typeparam>
    public class RestClientResponse<T>
    {
        /// <summary>
        /// Exception that occurred during request
        /// </summary>
        /// <remarks>This value will be "null" if exception did not occur</remarks>
        public Exception Exception { get; set; }

        /// <summary>
        /// Status of http request
        /// </summary>
        public HttpStatusCode? StatusCode { get; set; }

        /// <summary>
        /// Identifies whether request was successful or failed
        /// </summary>
        public bool IsValid { get; set; }

        /// <summary>
        /// Value expected from <see cref="HttpResponseMessage.Content"/> deserialization
        /// </summary>
        public T Value { get; set; }
    }
}
