using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

[assembly: InternalsVisibleTo("BlazorFocused.Test")]

namespace BlazorFocused.Client
{
    /// <inheritdoc cref="IRestClient"/>
    internal class RestClient : IRestClient
    {
        private readonly HttpClient client;
        private readonly IParameterBuilder parameterBuilder;
        private readonly ILogger<RestClient> logger;

        public RestClient(
            HttpClient client,
            IParameterBuilder parameterBuilder,
            ILogger<RestClient> logger)
        {
            this.client = client ?? throw new ArgumentNullException(nameof(client));
            this.parameterBuilder = parameterBuilder ?? throw new ArgumentNullException(nameof(parameterBuilder));
            this.logger = logger ?? NullLogger<RestClient>.Instance;
        }

        public async Task<T> DeleteAsync<T>(string relativeUrl, object[] parameters = null) =>
            ExtractOrThrowResponse<T>(await TryDeleteAsync<T>(relativeUrl, parameters));

        public async Task<T> GetAsync<T>(string relativeUrl, object[] parameters = null) =>
            ExtractOrThrowResponse<T>(await TryGetAsync<T>(relativeUrl, parameters));

        public async Task<T> PostAsync<T>(string relativeUrl, object data, object[] parameters = null) =>
            ExtractOrThrowResponse<T>(await TryPostAsync<T>(relativeUrl, data, parameters));

        public async Task<T> PutAsync<T>(string relativeUrl, object data, object[] parameters = null) =>
            ExtractOrThrowResponse<T>(await TryPutAsync<T>(relativeUrl, data, parameters));

        public async Task<RestClientResponse<T>> TryDeleteAsync<T>(string relativeUrl, object[] parameters = null)
        {
            return await SendAsync<T>(HttpMethod.Delete, GetUrl(relativeUrl, parameters));
        }

        public async Task<RestClientResponse<T>> TryGetAsync<T>(string relativeUrl, object[] parameters = null)
        {
            return await SendAsync<T>(HttpMethod.Get, GetUrl(relativeUrl, parameters));
        }

        public async Task<RestClientResponse<T>> TryPostAsync<T>(string relativeUrl, object data, object[] parameters = null)
        {
            return await SendAsync<T>(HttpMethod.Post, GetUrl(relativeUrl, parameters), data);
        }

        public async Task<RestClientResponse<T>> TryPutAsync<T>(string relativeUrl, object data, object[] parameters = null)
        {
            return await SendAsync<T>(HttpMethod.Put, GetUrl(relativeUrl, parameters), data);
        }

        private HttpContent ConvertToHttpContent(object data)
        {
            return new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");
        }

        private async Task<T> Deserialize<T>(HttpContent content)
        {
            return await content.ReadFromJsonAsync<T>();
        }

        private bool GetStatusCodeValidation(HttpStatusCode? httpStatusCode) =>
            httpStatusCode.HasValue ? new HttpResponseMessage(httpStatusCode.Value).IsSuccessStatusCode : false;


        private T ExtractOrThrowResponse<T>(RestClientResponse<T> response)
        {
            if (response.IsValid)
                return response.Value;
            else
                throw response.Exception;
        }

        private RestClientResponse<T> GetRestClientResponse<T>(T value, HttpStatusCode? httpStatusCode, Exception exception)
        {
            return new RestClientResponse<T>
            {
                Exception = exception,
                StatusCode = httpStatusCode.GetValueOrDefault(),
                IsValid = GetStatusCodeValidation(httpStatusCode),
                Value = value
            };
        }
        private Uri GetUri(string relativeUrl) =>
            new Uri(client.BaseAddress, relativeUrl);

        private string GetUrl(string relativeUrl, object[] parameters)
        {
            return relativeUrl;
        }

        private async Task<RestClientResponse<T>> SendAsync<T>(HttpMethod method, string url, object data = null)
        {
            var value = default(T);
            Exception exception = default;
            HttpStatusCode? httpStatusCode = default;

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

                HttpResponseMessage httpResponseMessage = await client.SendAsync(httpRequestMessage);
                httpStatusCode = httpResponseMessage.StatusCode;

                if (httpResponseMessage.IsSuccessStatusCode)
                    value = await Deserialize<T>(httpResponseMessage.Content);
                else
                    exception = new RestClientException(HttpMethod.Delete, httpResponseMessage.StatusCode, url);
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            return GetRestClientResponse<T>(value, httpStatusCode, exception);
        }
    }
}