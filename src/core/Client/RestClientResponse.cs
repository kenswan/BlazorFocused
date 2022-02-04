using System.Net;

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
        public Exception Exception { get; set; } = null;

        /// <summary>
        /// Status of http request
        /// </summary>
        /// <remarks>This may be null if url passed in is not valid relative or absolute path</remarks>
        public HttpStatusCode? StatusCode { get; set; } = null;

        /// <summary>
        /// Identifies whether request was successful or failed
        /// </summary>
        public bool IsSuccess 
        {
            get => Value is not null && Exception is null;
        }

        /// <summary>
        /// Value expected from <see cref="HttpResponseMessage.Content"/> deserialization
        /// </summary>
        public T Value { get; set; } = default;
    }
}
