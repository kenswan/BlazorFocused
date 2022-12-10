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

    private static async Task<RestClientResponse<T>> GetRestClientResponse<T>(
        IRestClient restClient, HttpMethod method, string url, object data = null)
    {
        RestClientHttpResponse<T> restClientHttpResponse = new();
        Exception exception = default;

        try
        {
            if (restClient is BaseRestClient baseRestClient)
            {
                restClientHttpResponse =
                    await baseRestClient.SendAndDeserializeAsync<T>(method, url, data);
            }
            else
            {
                ThrowUnqualifiedRestClient(restClient);
            }
        }
        catch (Exception ex)
        {
            exception = ex;

            if (ex is RestClientHttpException restClientHttpException)
            {
                restClientHttpResponse.StatusCode = restClientHttpException.StatusCode;
            }
        }

        return new RestClientResponse<T>
        {
            Exception = exception,
            Headers = restClientHttpResponse.Headers,
            StatusCode = restClientHttpResponse.StatusCode.GetValueOrDefault(),
            Value = restClientHttpResponse.Content
        };
    }

    private static async Task<RestClientTask> GetRestClientTask(
        IRestClient restClient, HttpMethod method, string url, object data = null)
    {
        RestClientHttpResponse restClientHttpResponse = new();
        Exception exception = default;

        try
        {
            if (restClient is BaseRestClient baseRestClient)
            {
                restClientHttpResponse = await baseRestClient.SendAndTaskAsync(method, url, data);
            }
            else
            {
                ThrowUnqualifiedRestClient(restClient);
            }
        }
        catch (Exception ex)
        {
            exception = ex;

            if (ex is RestClientHttpException restClientHttpException)
            {
                restClientHttpResponse.StatusCode = restClientHttpException.StatusCode;
            }
        }

        return new RestClientTask
        {
            Exception = exception,
            Headers = restClientHttpResponse.Headers,
            StatusCode = restClientHttpResponse.StatusCode.GetValueOrDefault(),
        };
    }

    private static void ThrowUnqualifiedRestClient(IRestClient restClient)
    {
        throw new RestClientException(
                    $"Operation not allowed by IRestClient of type {restClient.GetType().FullName}");
    }

    private static string GetUrlString(Action<IRestClientUrlBuilder> builderUrlAction)
    {
        var restClientUrlBuilder = new RestClientUrlBuilder();

        builderUrlAction(restClientUrlBuilder);

        return restClientUrlBuilder.Build();
    }
}
