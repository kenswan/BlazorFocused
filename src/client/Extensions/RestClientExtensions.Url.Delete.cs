namespace BlazorFocused.Client.Extensions
{
    public partial class RestClientExtensions
    {
        public static Task<T> DeleteAsync<T>(
            this IRestClient restClient, Action<IRestClientUrlBuilder> urlBuilder) =>
                restClient.DeleteAsync<T>(GetUrlString(urlBuilder));

        public static Task DeleteTaskAsync(
            this IRestClient restClient, Action<IRestClientUrlBuilder> urlBuilder) =>
                restClient.DeleteTaskAsync(GetUrlString(urlBuilder));

        public static Task<RestClientResponse<T>> TryDeleteAsync<T>(
            this IRestClient restClient, Action<IRestClientUrlBuilder> urlBuilder) =>
                GetRestClientResponse<T>(restClient, HttpMethod.Delete, GetUrlString(urlBuilder));

        public static Task<RestClientTask> TryDeleteTaskAsync(
            this IRestClient restClient, Action<IRestClientUrlBuilder> urlBuilder) =>
                GetRestClientTask(restClient, HttpMethod.Delete, GetUrlString(urlBuilder));
    }
}
