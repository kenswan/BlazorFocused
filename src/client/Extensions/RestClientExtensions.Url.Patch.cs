namespace BlazorFocused.Extensions
{
    public partial class RestClientExtensions
    {
        /// <summary>
        /// Performs PATCH http request
        /// </summary>
        /// <typeparam name="T">Object type of response from http request</typeparam>
        /// <param name="restClient"><see cref="IRestClient"/> implementation being extended</param>
        /// <param name="urlBuilder">
        /// <see cref="IRestClientUrlBuilder"/> used to construct relative or absolute url for request
        /// </param>
        /// <param name="data">Http request object body</param>
        /// <returns>Http response body of type <see cref="{T}"/></returns>
        /// <remarks>
        /// Rules/Details on <see cref="IRestClient.PatchAsync{T}(string, object)"/> apply
        /// </remarks>
        public static Task<T> PatchAsync<T>(
            this IRestClient restClient, Action<IRestClientUrlBuilder> urlBuilder, object data) =>
                restClient.PatchAsync<T>(GetUrlString(urlBuilder), data);

        /// <summary>
        /// Performs PATCH http request
        /// </summary>
        /// <param name="restClient"><see cref="IRestClient"/> implementation being extended</param>
        /// <param name="urlBuilder">
        /// <see cref="IRestClientUrlBuilder"/> used to construct relative or absolute url for request
        /// </param>
        /// <param name="data">Http request object body</param>
        /// <returns>Task for completion detection</returns>
        /// <remarks>
        /// Rules/Details on <see cref="IRestClient.PatchTaskAsync(string, object)"/> apply
        /// </remarks>
        public static Task PatchTaskAsync(
            this IRestClient restClient, Action<IRestClientUrlBuilder> urlBuilder, object data) =>
                restClient.PatchTaskAsync(GetUrlString(urlBuilder), data);

        /// <summary>
        /// Performs PATCH http request
        /// </summary>
        /// <typeparam name="T">Object type of response from http request</typeparam>
        /// <param name="restClient"><see cref="IRestClient"/> implementation being extended</param>
        /// <param name="urlBuilder">
        /// <see cref="IRestClientUrlBuilder"/> used to construct relative or absolute url for request
        /// </param>
        /// <param name="data">Http request object body</param>
        /// <returns>
        /// Http response attributes including value, status code, or any exceptions.
        /// See <see cref="RestClientResponse{T}"/>
        /// </returns>
        /// <remarks>
        /// Rules/Details on <see cref="TryPatchAsync{T}(IRestClient, string, object)"/> apply
        /// </remarks>
        public static Task<RestClientResponse<T>> TryPatchAsync<T>(
            this IRestClient restClient, Action<IRestClientUrlBuilder> urlBuilder, object data) =>
                GetRestClientResponse<T>(restClient, HttpMethod.Patch, GetUrlString(urlBuilder), data);

        /// <summary>
        /// Performs PATCH http request
        /// </summary>
        /// <param name="restClient"><see cref="IRestClient"/> implementation being extended</param>
        /// <param name="urlBuilder">
        /// <see cref="IRestClientUrlBuilder"/> used to construct relative or absolute url for request
        /// </param>
        /// <param name="data">Http request object body</param>
        /// <returns>
        /// <see cref="RestClientTask"/> with http response attributes including status code or any exceptions.
        /// </returns>
        /// <remarks>
        /// Rules/Details on <see cref="TryPatchTaskAsync(IRestClient, string, object)"/> apply
        /// </remarks>
        public static Task<RestClientTask> TryPatchTaskAsync(
            this IRestClient restClient, Action<IRestClientUrlBuilder> urlBuilder, object data) =>
                GetRestClientTask(restClient, HttpMethod.Patch, GetUrlString(urlBuilder), data);
    }
}
