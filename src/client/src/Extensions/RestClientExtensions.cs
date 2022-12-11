// -------------------------------------------------------
// Copyright (c) Ken Swan All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using BlazorFocused.Client;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace BlazorFocused.Extensions;

/// <summary>
/// <see cref="IRestClient"/> extensions for http requests/responses and url construction
/// </summary>
public static partial class RestClientExtensions
{
    public static IRestClient CreateRestClient(HttpClient httpClient, ILogger logger = null)
    {
        return new StandaloneClient(httpClient, logger ?? NullLogger.Instance);
    }

    private static string GetUrlString(Action<IRestClientUrlBuilder> builderUrlAction)
    {
        var restClientUrlBuilder = new RestClientUrlBuilder();

        builderUrlAction(restClientUrlBuilder);

        return restClientUrlBuilder.Build();
    }
}
