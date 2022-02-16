namespace BlazorFocused.Client.Extensions
{
    public partial class RestClientExtensions
    {
        public static Task<T> GetAsync<T>(
            this IRestClient restClient, Action<IRestClientUrlBuilder> urlBuilder) =>
                restClient.GetAsync<T>(GetUrlString(urlBuilder));

        public static Task<RestClientResponse<T>> TryGetAsync<T>(
            this IRestClient restClient, Action<IRestClientUrlBuilder> urlBuilder) =>
                GetRestClientResponse<T>(restClient, HttpMethod.Get, GetUrlString(urlBuilder));
    }
}
