// Add Header Here

using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Http.Json;

namespace BlazorFocused.Client;

internal partial class BaseRestClient
{
    public async Task<RestClientHttpResponse<T>> SendAndDeserializeAsync<T>(HttpMethod method, string url, object data = null)
    {
        var httpResponseMessage = await SendAndLogAsync(method, url, data);

        logger.LogDebug("Deserializing response content");

        return await DeserializeAsync<T>(httpResponseMessage);
    }

    public async Task<RestClientHttpResponse<T>> SendAndDeserializeAsync<T>(HttpRequestMessage httpRequestMessage)
    {
        var httpResponseMessage = await SendAndLogAsync(httpRequestMessage);

        logger.LogDebug("Deserializing response content");

        return await DeserializeAsync<T>(httpResponseMessage);
    }

    public async Task<RestClientHttpResponse> SendAndTaskAsync(HttpMethod method, string url, object data = null)
    {
        var httpResponseMessage = await SendAndLogAsync(method, url, data);

        return new RestClientHttpResponse
        {
            Headers = httpResponseMessage.Headers,
            StatusCode = httpResponseMessage.StatusCode
        };
    }

    public async Task<RestClientHttpResponse> SendAndTaskAsync(HttpRequestMessage httpRequestMessage)
    {
        var httpResponseMessage = await SendAndLogAsync(httpRequestMessage);

        return new RestClientHttpResponse
        {
            Headers = httpResponseMessage.Headers,
            StatusCode = httpResponseMessage.StatusCode
        };
    }

    public async Task<HttpResponseMessage> StandardSendAsync(HttpRequestMessage httpRequestMessage) =>
        await SendAndLogAsync(httpRequestMessage);

    private async Task<HttpResponseMessage> SendAndLogAsync(HttpMethod method, string url, object data = null)
    {
        var httpResponseMessage = await SendAsync(method, url, data);

        return await LogAsync(httpResponseMessage, method, url);
    }

    private async Task<HttpResponseMessage> SendAndLogAsync(HttpRequestMessage httpRequestMessage)
    {
        var method = httpRequestMessage.Method;
        var url = httpRequestMessage.RequestUri.OriginalString;
        var httpResponseMessage = await SendAsync(httpRequestMessage);

        return await LogAsync(httpResponseMessage, method, url);
    }

    private static async Task<RestClientHttpResponse<T>> DeserializeAsync<T>(HttpResponseMessage httpResponseMessage)
    {
        var content = await httpResponseMessage.Content.ReadFromJsonAsync<T>();

        return new RestClientHttpResponse<T>
        {
            Content = content,
            StatusCode = httpResponseMessage.StatusCode,
            Headers = httpResponseMessage.Headers
        };
    }
    private async Task<HttpResponseMessage> LogAsync(HttpResponseMessage httpResponseMessage, HttpMethod method, string url)
    {
        var errorContent = await httpResponseMessage.Content?.ReadAsStringAsync() ?? string.Empty;

        if (!httpResponseMessage.IsSuccessStatusCode)
            LogAndThrowFailure(httpResponseMessage.StatusCode, method, url, errorContent);
        else
            LogSuccess(httpResponseMessage.StatusCode, method, url);

        return httpResponseMessage;
    }

    private async Task<T> GetResponseValue<T>(HttpMethod method, string url, object data = null)
    {
        var restClientHttpResponse = await SendAndDeserializeAsync<T>(method, url, data);

        return restClientHttpResponse.Content;
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
