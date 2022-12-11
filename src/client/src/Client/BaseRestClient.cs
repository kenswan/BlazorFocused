// -------------------------------------------------------
// Copyright (c) Ken Swan All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System.Net;
using System.Text;
using System.Text.Json;

namespace BlazorFocused.Client;

/// <inheritdoc cref="IBaseRestClient"/>
internal abstract class BaseRestClient : IBaseRestClient
{
    protected readonly HttpClient httpClient;
    protected readonly ILogger logger;

    public BaseRestClient(HttpClient httpClient, ILogger logger)
    {
        this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        this.logger = logger ?? NullLogger.Instance;
    }

    public async Task<RestClientResponse<T>> SendAsync<T>(HttpMethod httpMethod, string url, object data = null)
    {
        RestClientTask task = await SendAsync(httpMethod, url, data);

        var restClientResponse = new RestClientResponse<T>
        {
            Content = task.Content,
            StatusCode = task.StatusCode,
            Headers = task.Headers,
            Exception = task.Exception
        };

        if (task.IsSuccess)
        {
            restClientResponse.Value = JsonSerializer.Deserialize<T>(task.Content);
        }

        return restClientResponse;
    }

    public async Task<RestClientTask> SendAsync(HttpMethod httpMethod, string url, object data = null)
    {
        logger.LogDebug("Starting Request: {Method} - {Url}", httpMethod, url);

        HttpContent httpContent = data switch
        {
            null => default,
            { } when data is FormUrlEncodedContent formUrlEncodedContent => formUrlEncodedContent,
            { } when data is MultipartFormDataContent multipartFormDataContent => multipartFormDataContent,
            _ => new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json")
        };

        var httpRequestMessage = new HttpRequestMessage
        {
            Content = httpContent,
            Method = httpMethod,
            RequestUri = new(httpClient.BaseAddress, url)
        };

        HttpResponseMessage httpResponseMessage = await SendAsync(httpRequestMessage);
        HttpStatusCode httpStatusCode = httpResponseMessage.StatusCode;
        var contentString = await httpResponseMessage.Content?.ReadAsStringAsync();
        RestClientHttpException exception = default;

        if (!httpResponseMessage.IsSuccessStatusCode)
        {
            exception = new RestClientHttpException(httpMethod, httpStatusCode, url)
            {
                Content = contentString,
                Headers = httpResponseMessage.Headers
            };

            if (contentString is not null)
            {
                logger.LogError("FAILED Request: {StatusCode} - {Method} - {Url} {Response}", httpStatusCode, httpMethod, url, contentString);
            }
            else
            {
                logger.LogError("FAILED Request: {StatusCode} - {Method} - {Url}", httpStatusCode, httpMethod, url);
            }
        }
        else
        {
            logger.LogDebug("SUCCESSFUL Request: {StatusCode} - {Method} - {Url} {Response}", httpResponseMessage.StatusCode, httpMethod, url, contentString);
        }

        return new RestClientTask
        {
            Headers = httpResponseMessage.Headers,
            StatusCode = httpResponseMessage.StatusCode,
            Content = contentString,
            Exception = exception
        };
    }

    public async Task<HttpResponseMessage> SendAsync(HttpRequestMessage httpRequestMessage)
    {
        HttpMethod httpMethod = httpRequestMessage.Method;

        var url = httpRequestMessage.RequestUri.IsAbsoluteUri ?
            httpRequestMessage.RequestUri.PathAndQuery :
            throw new RestClientException($"Unable to send Http Request because {nameof(httpRequestMessage)} is not an absolute Uri");

        logger.LogDebug("Starting Request: {Method} - {Url}", httpMethod, url);

        HttpResponseMessage httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

        logger.LogDebug("Request Completed: {Method} - {Url}", httpMethod, url);

        return httpResponseMessage;
    }
}
