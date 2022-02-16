namespace BlazorFocused.Client.Extensions
{
    public partial class RestClientExtensions
    {
        public static Task<T> PutAsync<T>(
            this IRestClient restClient, Action<IRestClientUrlBuilder> urlBuilder, object data) =>
                restClient.PutAsync<T>(GetUrlString(urlBuilder), data);

        public static Task PutTaskAsync(
            this IRestClient restClient, Action<IRestClientUrlBuilder> urlBuilder, object data) =>
                restClient.PutTaskAsync(GetUrlString(urlBuilder), data);

        public static Task<RestClientResponse<T>> TryPutAsync<T>(
            this IRestClient restClient, Action<IRestClientUrlBuilder> urlBuilder, object data) =>
                GetRestClientResponse<T>(restClient, HttpMethod.Put, GetUrlString(urlBuilder), data);

        public static Task<RestClientTask> TryPutTaskAsync(
            this IRestClient restClient, Action<IRestClientUrlBuilder> urlBuilder, object data) =>
                GetRestClientTask(restClient, HttpMethod.Put, GetUrlString(urlBuilder), data);
    }
}
