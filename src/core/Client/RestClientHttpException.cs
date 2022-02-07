using System.Net;

namespace BlazorFocused.Client
{
    public class RestClientHttpException : Exception
    {
        public HttpMethod Method { get; private set; }
        
        public HttpStatusCode StatusCode { get; private set; }

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
