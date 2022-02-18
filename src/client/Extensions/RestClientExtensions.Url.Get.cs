namespace BlazorFocused.Extensions
{
    public partial class RestClientExtensions
    {
        /// <summary>
        /// Performs GET http request
        /// </summary>
        /// <typeparam name="T">Object type of response from http request</typeparam>
        /// <param name="restClient"><see cref="IRestClient"/> implementation being extended</param>
        /// <param name="urlBuilder">
        /// <see cref="IRestClientUrlBuilder"/> used to construct relative or absolute url for request
        /// </param>
        /// <returns>Http response body of type <see cref="{T}"/></returns>
        /// <remarks>
        /// Rules/Details on <see cref="IRestClient.GetAsync{T}(string)"/> apply
        /// </remarks>
        public static Task<T> GetAsync<T>(
            this IRestClient restClient, Action<IRestClientUrlBuilder> urlBuilder) =>
                restClient.GetAsync<T>(GetUrlString(urlBuilder));

        /// <summary>
        /// Performs GET http request
        /// </summary>
        /// <typeparam name="T">Object type of response from http request</typeparam>
        /// <param name="restClient"><see cref="IRestClient"/> implementation being extended</param>
        /// <param name="urlBuilder">
        /// <see cref="IRestClientUrlBuilder"/> used to construct relative or absolute url for request
        /// </param>
        /// <returns>
        /// Http response attributes including value, status code, or any exceptions.
        /// See <see cref="RestClientResponse{T}"/>
        /// </returns>
        /// <remarks>
        /// Rules/Details on <see cref="TryGetAsync{T}(IRestClient, string)"/> apply
        /// </remarks>
        public static Task<RestClientResponse<T>> TryGetAsync<T>(
            this IRestClient restClient, Action<IRestClientUrlBuilder> urlBuilder) =>
                GetRestClientResponse<T>(restClient, HttpMethod.Get, GetUrlString(urlBuilder));
    }
}
