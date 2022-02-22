using System.Text;
using System.Text.Json;

namespace BlazorFocused.Tools.Utility
{
    internal class TestHttpHeaderService
    {
        private readonly HttpClient httpClient;
        private readonly HttpRequestMessage httpRequestMessage;

        public TestHttpHeaderService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
            httpRequestMessage = new();
        }

        public void AddDefaultHeader(string key, string value)
        {
            httpClient.DefaultRequestHeaders.Add(key, value);
        }

        public void AddRequestHeader(string key, string value)
        {
            httpRequestMessage.Headers.Add(key, value);
        }

        public async Task MakeRequestAsync(HttpMethod httpMethod, string url, object content = null)
        {
            httpRequestMessage.Method = httpMethod;
            httpRequestMessage.RequestUri = new Uri(httpClient.BaseAddress, url);

            httpRequestMessage.Content =
                new StringContent(JsonSerializer.Serialize(content), Encoding.UTF8, "application/json");

            await httpClient.SendAsync(httpRequestMessage);
        }
    }
}
