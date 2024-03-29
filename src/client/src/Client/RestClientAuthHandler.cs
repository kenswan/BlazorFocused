﻿// -------------------------------------------------------
// Copyright (c) Ken Swan All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using Microsoft.Extensions.Logging;
using System.Net.Http.Headers;

namespace BlazorFocused.Client;

internal class RestClientAuthHandler : DelegatingHandler
{
    private readonly IOAuthToken oAuthToken;
    private readonly ILogger<RestClientAuthHandler> logger;

    public RestClientAuthHandler(IOAuthToken oAuthToken, ILogger<RestClientAuthHandler> logger)
    {
        this.oAuthToken = oAuthToken;
        this.logger = logger;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if (!oAuthToken.IsEmpty())
        {
            logger.LogDebug("Authorization token found");

            request.Headers.Authorization =
                new AuthenticationHeaderValue(oAuthToken.Scheme, oAuthToken.Token);

            logger.LogDebug("Authorization token applied");
        }
        else
        {
            logger.LogInformation($"Authorization token not found in {nameof(RestClientAuthHandler)}");
        }

        return await base.SendAsync(request, cancellationToken);
    }
}
