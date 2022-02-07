using System.Net;

namespace BlazorFocused.Client
{
    /// <summary>
    /// Gives result of an http request using <see cref="IRestClient"/>
    /// </summary>
    public class RestClientTask
    {
        /// <summary>
        /// Exception that occurred during request
        /// </summary>
        /// <remarks>This value will be "null" if exception did not occur</remarks>
        public Exception Exception { get; set; } = null;

        /// <summary>
        /// Identifies whether request was successful or failed
        /// </summary>
        public virtual bool IsSuccess
        {
            get => HasSuccessStatusCode();
        }

        /// <summary>
        /// Status of http request
        /// </summary>
        /// <remarks>This may be null if url passed in is not valid relative or absolute path</remarks>
        public HttpStatusCode? StatusCode { get; set; } = null;

        protected bool HasSuccessStatusCode() =>
            StatusCode.HasValue && new HttpResponseMessage(StatusCode.Value).IsSuccessStatusCode;
    }
}
