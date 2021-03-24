using System.Threading.Tasks;

namespace BlazorFocused.Client
{
    /// <summary>
    /// Standardizes the handling of HTTP requests/responses within a given application.
    /// </summary>
    public interface IRestClient
    {
        /// <summary>
        /// Performs DELETE http request
        /// </summary>
        /// <typeparam name="T">Type of response from http request</typeparam>
        /// <param name="relativeUrl">Relative url for http request</param>
        /// <returns>Http response body</returns>
        /// <remarks>
        /// This method will throw an exception for non-success status codes 
        /// </remarks>
        Task<T> DeleteAsync<T>(string relativeUrl);

        /// <summary>
        /// Performs GET http request
        /// </summary>
        /// <typeparam name="T">Type of response from http request</typeparam>
        /// <param name="relativeUrl">Relative url for http request</param>
        /// <returns>Http response body</returns>
        /// <remarks>
        /// This method will throw an exception for non-success status codes 
        /// </remarks>
        Task<T> GetAsync<T>(string relativeUrl);

        /// <summary>
        /// Performs PATCH http request
        /// </summary>
        /// <typeparam name="T">Type of response from http request</typeparam>
        /// <param name="relativeUrl">Relative url for http request</param>
        /// <param name="data">Http request object body</param>
        /// <returns>Http Response Body</returns>
        /// <remarks>
        /// This method will throw an exception for non-success status codes
        /// </remarks>
        Task<T> PatchAsync<T>(string relativeUrl, object data);

        /// <summary>
        /// Performs POST http request
        /// </summary>
        /// <typeparam name="T">Type of response from http request</typeparam>
        /// <param name="relativeUrl">Relative url for http request</param>
        /// <param name="data">Http request object body</param>
        /// <returns>Http response body</returns>
        /// <remarks>
        /// This method will throw an exception for non-success status codes
        /// </remarks>
        Task<T> PostAsync<T>(string relativeUrl, object data);

        /// <summary>
        /// Performs PUT http request
        /// </summary>
        /// <typeparam name="T">Type of response from http request</typeparam>
        /// <param name="relativeUrl">Relative url for http request</param>
        /// <param name="data">Http request object body</param>
        /// <returns>Http response body</returns>
        /// <remarks>
        /// This method will throw an exception for non-success status codes
        /// </remarks>
        Task<T> PutAsync<T>(string relativeUrl, object data);

        Task<RestClientResponse<T>> TryDeleteAsync<T>(string relativeUrl);

        Task<RestClientResponse<T>> TryGetAsync<T>(string relativeUrl);

        Task<RestClientResponse<T>> TryPatchAsync<T>(string relativeUrl, object data);

        Task<RestClientResponse<T>> TryPostAsync<T>(string relativeUrl, object data);

        Task<RestClientResponse<T>> TryPutAsync<T>(string relativeUrl, object data);
    }
}
