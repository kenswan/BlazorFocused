using System.Net;

namespace BlazorFocused.Client
{
    /// <summary>
    /// Exception returned with failed requests and operations within <see cref="IRestClient"/>
    /// and <see cref="IOAuthRestClient"/>
    /// </summary>
    public class RestClientException : Exception
    {
        /// <summary>
        /// Initializes a new instance of <see cref="RestClientException"/>
        /// with an exception message
        /// </summary>
        /// <param name="message">Exception message</param>
        public RestClientException(string message) : base(message) { }

        /// <summary>
        /// Initializes a new instance of <see cref="RestClientException"/>
        /// with an exception message and inner exception
        /// </summary>
        /// <param name="message">Exception message</param>
        /// <param name="exception">Inner Exception</param>
        public RestClientException(string message, Exception exception) : base(message, exception) { }

        /// <summary>
        /// Initializes a new instance of <see cref="RestClientException"/> with the Http Request
        /// information that caused the exception.
        /// </summary>
        /// <param name="method">Http method of request that caused exception</param>
        /// <param name="httpStatusCode">Status code of the response from failed request</param>
        /// <param name="url">Url of request that caused exception</param>
        public RestClientException(HttpMethod method, HttpStatusCode httpStatusCode, string url) :
            base($"{method} request failed with {httpStatusCode} at {url}")
        { }
    }
}
