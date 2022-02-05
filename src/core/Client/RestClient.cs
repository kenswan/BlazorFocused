using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net;

namespace BlazorFocused.Client
{
    /// <inheritdoc cref="IRestClient"/>
    internal class RestClient : BaseRestClient, IRestClient
    {
        public RestClient(
            HttpClient httpClient,
            IOptions<RestClientOptions> restClientOptions,
            ILogger<RestClient> logger)
                : base(httpClient, logger)
        {
            if (restClientOptions?.Value is not null)
            {
                this.httpClient.ConfigureRestClientOptions(restClientOptions.Value);
            }
        }

        /// <summary>
        /// Gets current instance of <see cref="HttpClient"/> being used for requests
        /// </summary>
        /// <returns>Current <see cref="HttpClient"/> from dependency injection</returns>
        public HttpClient GetClient() => httpClient;

        public async Task<T> DeleteAsync<T>(string relativeUrl) =>
            await GetResponseValue<T>(HttpMethod.Delete, relativeUrl);

        public async Task DeleteTaskAsync(string relativeUrl) =>
            await SendAndTaskAsync(HttpMethod.Delete, relativeUrl);

        public async Task<T> GetAsync<T>(string relativeUrl) =>
            await GetResponseValue<T>(HttpMethod.Get, relativeUrl);

        public async Task<T> PatchAsync<T>(string relativeUrl, object data) =>
            await GetResponseValue<T>(HttpMethod.Patch, relativeUrl, data);

        public async Task PatchTaskAsync(string relativeUrl, object data) =>
            await SendAndTaskAsync(HttpMethod.Patch, relativeUrl, data);

        public async Task<T> PostAsync<T>(string relativeUrl, object data) =>
            await GetResponseValue<T>(HttpMethod.Post, relativeUrl, data);

        public async Task PostTaskAsync(string relativeUrl, object data) =>
            await SendAndTaskAsync(HttpMethod.Post, relativeUrl, data);

        public async Task<T> PutAsync<T>(string relativeUrl, object data) =>
            await GetResponseValue<T>(HttpMethod.Put, relativeUrl, data);

        public async Task PutTaskAsync(string relativeUrl, object data) =>
            await SendAndTaskAsync(HttpMethod.Put, relativeUrl, data);

        public void UpdateHttpClient(Action<HttpClient> updateHttpClient)
        {
            updateHttpClient(httpClient);
        }

        private async Task<T> GetResponseValue<T>(HttpMethod method, string url, object data = null)
        {
            (HttpStatusCode _, T value) = await SendAndDeserializeAsync<T>(method, url, data);

            return value;
        }
    }
}