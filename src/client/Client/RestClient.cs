﻿using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BlazorFocused.Client;

/// <inheritdoc cref="IRestClient"/>
internal class RestClient : BaseRestClient
{
    protected readonly IRestClientRequestHeaders requestHeaders;

    public RestClient(
        HttpClient httpClient,
        IOptions<RestClientOptions> restClientOptions,
        IRestClientRequestHeaders requestHeaders,
        ILogger<RestClient> logger)
            : base(httpClient, logger)
    {
        if (restClientOptions?.Value is not null)
        {
            this.httpClient.ConfigureRestClientOptions(restClientOptions.Value);
        }

        this.requestHeaders = requestHeaders;
    }

    public override void AddHeader(string key, string value, bool global = true)
    {
        if (global)
            requestHeaders.AddHeader(key, value);
        else
            httpClient.DefaultRequestHeaders.Add(key, value);
    }
}
