// -------------------------------------------------------
// Copyright (c) Ken Swan All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System.Text;
using System.Text.Json;

namespace BlazorFocused.Client;

internal abstract class AbstractRestClient
{
    protected readonly HttpClient httpClient;
    protected readonly ILogger logger;

    public AbstractRestClient(HttpClient httpClient, ILogger logger)
    {
        this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        this.logger = logger ?? NullLogger.Instance;
    }

    protected async Task<HttpResponseMessage> SendAsync(HttpMethod method, string url, object data = null)
    {
        logger.LogDebug("Starting Request: {Method} - {Url}", method, url);

        HttpContent content = data is not null ?
            ConvertToHttpContent(data) : default;

        var httpRequestMessage = new HttpRequestMessage
        {
            Content = content,
            Method = method,
            RequestUri = GetUri(url)
        };

        logger.LogDebug("Constructed HttpRequestMessage");

        return await httpClient.SendAsync(httpRequestMessage);
    }

    protected async Task<HttpResponseMessage> SendAsync(HttpRequestMessage httpRequestMessage)
    {
        logger.LogDebug("Starting Request: {Method} - {Url}", httpRequestMessage.Method, httpRequestMessage.RequestUri.OriginalString);

        if (!httpRequestMessage.RequestUri.IsAbsoluteUri)
        {
            httpRequestMessage.RequestUri = new Uri(httpClient.BaseAddress, httpRequestMessage.RequestUri.OriginalString);
        }

        logger.LogDebug("Constructed HttpRequestMessage");

        return await httpClient.SendAsync(httpRequestMessage);
    }

    private HttpContent ConvertToHttpContent(object data)
    {
        logger.LogDebug("Creating HttpContent for request");

        return new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");
    }

    private Uri GetUri(string relativeUrl)
    {
        return new(httpClient.BaseAddress, relativeUrl);
    }
}
