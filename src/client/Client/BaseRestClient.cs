using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Http.Json;

namespace BlazorFocused.Client
{
    internal abstract class BaseRestClient : AbstractRestClient
    {
        public BaseRestClient(HttpClient httpClient, ILogger logger)
            : base(httpClient, logger)
        { }

        public async Task<(HttpStatusCode, T)> SendAndDeserializeAsync<T>(HttpMethod method, string url, object data = null)
        {
            var httpResponseMessage = await SendAndLogAsync(method, url, data);

            logger.LogDebug("Deserializing response content");

            var content = await httpResponseMessage.Content.ReadFromJsonAsync<T>();

            return (httpResponseMessage.StatusCode, content);
        }

        public async Task<HttpStatusCode> SendAndTaskAsync(HttpMethod method, string url, object data = null)
        {
            var httpResponseMessage = await SendAndLogAsync(method, url, data);

            return httpResponseMessage.StatusCode;
        }

        private async Task<HttpResponseMessage> SendAndLogAsync(HttpMethod method, string url, object data = null)
        {
            var httpResponseMessage = await SendAsync(method, url, data);
            var errorContent = await httpResponseMessage.Content?.ReadAsStringAsync() ?? string.Empty;

            if (!httpResponseMessage.IsSuccessStatusCode)
                LogAndThrowFailure(httpResponseMessage.StatusCode, method, url, errorContent);
            else
                LogSuccess(httpResponseMessage.StatusCode, method, url);

            return httpResponseMessage;
        }

        private void LogSuccess(HttpStatusCode code, HttpMethod method, string url) =>
            logger.LogDebug("SUCCESSFUL Request: {Code} - {Method} - {Url} Request", code, method, url);

        private void LogAndThrowFailure(HttpStatusCode code, HttpMethod method, string url, string content)
        {
            var exception = new RestClientHttpException(method, code, url) { Content = content };

            logger.LogError(exception, "FAILED Request: {Code} - {Method} - {Url} Request", code, method, url);

            if (content is not null)
                logger.LogDebug("FAILED Request Content: ({Method} - {Url}) {Content}", code, method, content);

            throw exception;
        }
    }
}
