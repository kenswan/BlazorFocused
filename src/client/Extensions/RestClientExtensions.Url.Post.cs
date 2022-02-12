namespace BlazorFocused.Client.Extensions
{
    public partial class RestClientExtensions
    {
        public static Task<T> PostAsync<T>(
            this IRestClient restClient, Action<IRestClientUrlBuilder> urlBuilder, object data) =>
                restClient.PostAsync<T>(GetUrlString(urlBuilder), data);

        public static Task PostTaskAsync(
            this IRestClient restClient, Action<IRestClientUrlBuilder> urlBuilder, object data) =>
                restClient.PostTaskAsync(GetUrlString(urlBuilder), data);

        public static Task<RestClientResponse<T>> TryPostAsync<T>(
            this IRestClient restClient, Action<IRestClientUrlBuilder> urlBuilder, object data) =>
                GetRestClientResponse<T>(restClient, HttpMethod.Post, GetUrlString(urlBuilder), data);

        public static Task<RestClientTask> TryPostTaskAsync(
            this IRestClient restClient, Action<IRestClientUrlBuilder> urlBuilder, object data) =>
                GetRestClientTask(restClient, HttpMethod.Post, GetUrlString(urlBuilder), data);
    }
}
