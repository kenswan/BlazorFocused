﻿using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace BlazorFocused.Client
{
    /// <inheritdoc cref="IRestClient"/>
    internal class RestClient : IRestClient
    {
        protected readonly HttpClient httpClient;
        protected readonly ILogger<RestClient> logger;

        public RestClient(
            HttpClient httpClient,
            IOptions<RestClientOptions> restClientOptions,
            ILogger<RestClient> logger)
        {
            this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            this.logger = logger ?? NullLogger<RestClient>.Instance;

            if (restClientOptions?.Value is not null)
            {
                this.httpClient.ConfigureRestClientOptions(restClientOptions.Value);
            }
        }

        public async Task<T> DeleteAsync<T>(string relativeUrl) =>
            ExtractOrThrowResponse<T>(await TryDeleteAsync<T>(relativeUrl));

        public async Task<T> GetAsync<T>(string relativeUrl) =>
            ExtractOrThrowResponse<T>(await TryGetAsync<T>(relativeUrl));

        public async Task<T> PatchAsync<T>(string relativeUrl, object data) =>
            ExtractOrThrowResponse<T>(await TryPatchAsync<T>(relativeUrl, data));

        public async Task<T> PostAsync<T>(string relativeUrl, object data) =>
            ExtractOrThrowResponse<T>(await TryPostAsync<T>(relativeUrl, data));

        public async Task<T> PutAsync<T>(string relativeUrl, object data) =>
            ExtractOrThrowResponse<T>(await TryPutAsync<T>(relativeUrl, data));

        public async Task<RestClientResponse<T>> TryDeleteAsync<T>(string relativeUrl) =>
            await SendAsync<T>(HttpMethod.Delete, relativeUrl);

        public async Task<RestClientResponse<T>> TryGetAsync<T>(string relativeUrl) =>
            await SendAsync<T>(HttpMethod.Get, relativeUrl);

        public async Task<RestClientResponse<T>> TryPatchAsync<T>(string relativeUrl, object data) =>
            await SendAsync<T>(HttpMethod.Patch, relativeUrl, data);

        public async Task<RestClientResponse<T>> TryPostAsync<T>(string relativeUrl, object data) =>
            await SendAsync<T>(HttpMethod.Post, relativeUrl, data);

        public async Task<RestClientResponse<T>> TryPutAsync<T>(string relativeUrl, object data) =>
            await SendAsync<T>(HttpMethod.Put, relativeUrl, data);

        /// <summary>
        /// Gets current instance of <see cref="HttpClient"/> being used for requests
        /// </summary>
        /// <returns>Current <see cref="HttpClient"/> from dependency injection</returns>
        public HttpClient GetClient() => httpClient;

        private HttpContent ConvertToHttpContent(object data)
        {
            logger.LogDebug("Creating HttpContent for request");

            return new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");
        }

        private async Task<T> Deserialize<T>(HttpContent content)
        {
            logger.LogDebug("Deserializing response content");

            return await content.ReadFromJsonAsync<T>();
        }

        private static T ExtractOrThrowResponse<T>(RestClientResponse<T> response)
        {
            if (response.IsSuccess)
                return response.Value;
            else
                throw response.Exception;
        }

        private RestClientResponse<T> GetRestClientResponse<T>(T value, HttpStatusCode? httpStatusCode, Exception exception)
        {
            logger.LogDebug("Creating rest client response for {HttpStatusCode} status code", (int)httpStatusCode);

            return new RestClientResponse<T>
            {
                Exception = exception,
                StatusCode = httpStatusCode.GetValueOrDefault(),
                Value = value
            };
        }

        private Uri GetUri(string relativeUrl) =>
            new(httpClient.BaseAddress, relativeUrl);

        protected virtual async Task<RestClientResponse<T>> SendAsync<T>(HttpMethod method, string url, object data = null)
        {
            var value = default(T);
            Exception exception = default;
            HttpStatusCode? httpStatusCode = default;

            logger.LogDebug("Starting {Method} - {Url} Request", method, url);

            try
            {
                HttpContent content = data != null ?
                    ConvertToHttpContent(data) : default;

                var httpRequestMessage = new HttpRequestMessage
                {
                    Content = content,
                    Method = method,
                    RequestUri = GetUri(url)
                };

                logger.LogDebug("Constructed HttpRequestMessage");

                HttpResponseMessage httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);
                httpStatusCode = httpResponseMessage.StatusCode;

                logger.LogDebug("HttpResponse returned from request with status code {HttpStatusCode}", (int)httpStatusCode);

                if (httpResponseMessage.IsSuccessStatusCode)
                    value = await Deserialize<T>(httpResponseMessage.Content);
                else
                    exception = new RestClientException(method, httpResponseMessage.StatusCode, url);
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            return GetRestClientResponse(value, httpStatusCode, exception);
        }

        public void UpdateHttpClient(Action<HttpClient> updateHttpClient)
        {
            updateHttpClient(httpClient);
        }
    }
}