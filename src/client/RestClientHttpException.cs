using System.Net;

namespace BlazorFocused
{
    /// <summary>
    /// Exception returned with failed http requests and operations within <see cref="IRestClient"/>
    /// </summary>
    public class RestClientHttpException : Exception
    {
        /// <summary>
        /// Method of origin http request (DELETE, GET, PATCH, POST, PUT)
        /// </summary>
        public HttpMethod Method { get; private set; }

        /// <summary>
        /// Http Response status code of failed http request
        /// </summary>
        public HttpStatusCode StatusCode { get; private set; }

        /// <summary>
        /// Url of origin http request
        /// </summary>
        public string Url { get; private set; }

        /// <summary>
        /// Initializes a new instance of <see cref="RestClientHttpException"/> with the Http Request
        /// information that caused the exception.
        /// </summary>
        /// <param name="httpMethod">Http method of request that caused exception</param>
        /// <param name="httpStatusCode">Status code of the response from failed request</param>
        /// <param name="url">Url of request that caused exception</param>
        public RestClientHttpException(HttpMethod httpMethod, HttpStatusCode httpStatusCode, string url) :
            base($"{httpMethod} request failed with {httpStatusCode} at {url}")
        {
            Method = httpMethod;
            StatusCode = httpStatusCode;
            Url = url;
        }
    }
}
