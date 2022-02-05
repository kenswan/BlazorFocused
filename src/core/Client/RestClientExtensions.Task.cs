namespace BlazorFocused.Client
{
    public static partial class RestClientExtensions
    {
        public static async Task<RestClientTask> TryDeleteTaskAsync<T>(
            this IRestClient restClient, string relativeUrl) =>
                await GetRestClientTask(restClient, HttpMethod.Delete, relativeUrl);

        public static async Task<RestClientTask> TryPatchTaskAsync<T>(this IRestClient restClient, string relativeUrl, object data) =>
            await GetRestClientTask(restClient, HttpMethod.Patch, relativeUrl, data);

        public static async Task<RestClientTask> TryPostTaskAsync<T>(this IRestClient restClient, string relativeUrl, object data) =>
            await GetRestClientTask(restClient, HttpMethod.Post, relativeUrl, data);

        public static async Task<RestClientTask> TryPutTaskAsync<T>(this IRestClient restClient, string relativeUrl, object data) =>
           await GetRestClientTask(restClient, HttpMethod.Put, relativeUrl, data);
    }
}
