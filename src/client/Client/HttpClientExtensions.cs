// -------------------------------------------------------
// Copyright (c) Ken Swan All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

namespace BlazorFocused.Client;

internal static class HttpClientExtensions
{
    public static void ConfigureRestClientOptions(this HttpClient httpClient, RestClientOptions restClientOptions)
    {
        if (!string.IsNullOrWhiteSpace(restClientOptions.BaseAddress))
        {
            httpClient.BaseAddress = new Uri(restClientOptions.BaseAddress);
        }

        foreach (KeyValuePair<string, string> header in restClientOptions.DefaultRequestHeaders)
        {
            httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
        }

        if (restClientOptions.MaxResponseContentBufferSize.HasValue)
        {
            httpClient.MaxResponseContentBufferSize = restClientOptions.MaxResponseContentBufferSize.Value;
        }

        if (restClientOptions.Timeout.HasValue)
        {
            httpClient.Timeout = TimeSpan.FromMilliseconds(restClientOptions.Timeout.Value);
        }
    }
}
