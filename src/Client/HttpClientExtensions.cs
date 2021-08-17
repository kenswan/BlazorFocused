using System;
using System.Net.Http;

namespace BlazorFocused.Client
{
    internal static class HttpClientExtensions
    {
        public static void ConfigureRestClientSettings(this HttpClient httpClient, RestClientSettings restClientSettings)
        {
            if (!string.IsNullOrWhiteSpace(restClientSettings.BaseAddress))
            {
                httpClient.BaseAddress = new System.Uri(restClientSettings.BaseAddress);
            }

            foreach (var header in restClientSettings.DefaultRequestHeaders)
            {
                httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
            }

            if (restClientSettings.MaxResponseContentBufferSize.HasValue)
            {
                httpClient.MaxResponseContentBufferSize = restClientSettings.MaxResponseContentBufferSize.Value;
            }

            if (restClientSettings.Timeout.HasValue)
            {
                httpClient.Timeout = TimeSpan.FromMilliseconds(restClientSettings.Timeout.Value);
            }
        }
    }
}
