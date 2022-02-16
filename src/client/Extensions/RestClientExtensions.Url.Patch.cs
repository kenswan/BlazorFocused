namespace BlazorFocused.Client.Extensions
{
    public partial class RestClientExtensions
    {
        public static Task<T> PatchAsync<T>(
            this IRestClient restClient, Action<IRestClientUrlBuilder> urlBuilder, object data) =>
                restClient.PatchAsync<T>(GetUrlString(urlBuilder), data);

        public static Task PatchTaskAsync(
            this IRestClient restClient, Action<IRestClientUrlBuilder> urlBuilder, object data) =>
                restClient.PatchTaskAsync(GetUrlString(urlBuilder), data);

        public static Task<RestClientResponse<T>> TryPatchAsync<T>(
            this IRestClient restClient, Action<IRestClientUrlBuilder> urlBuilder, object data) =>
                GetRestClientResponse<T>(restClient, HttpMethod.Patch, GetUrlString(urlBuilder), data);

        public static Task<RestClientTask> TryPatchTaskAsync(
            this IRestClient restClient, Action<IRestClientUrlBuilder> urlBuilder, object data) =>
                GetRestClientTask(restClient, HttpMethod.Patch, GetUrlString(urlBuilder), data);
    }
}
