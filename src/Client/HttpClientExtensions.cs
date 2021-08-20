using System;
using System.Net.Http;

namespace BlazorFocused.Client
{
    internal static class HttpClientExtensions
    {
        public static void ConfigureRestClientOptions(this HttpClient httpClient, RestClientOptions restClientOptions)
        {
            if (!string.IsNullOrWhiteSpace(restClientOptions.BaseAddress))
            {
                httpClient.BaseAddress = new Uri(restClientOptions.BaseAddress);
            }

            foreach (var header in restClientOptions.DefaultRequestHeaders)
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
}
