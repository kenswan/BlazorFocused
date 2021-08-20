using BlazorFocused.Testing;
using Bogus;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace BlazorFocused.Client
{
    public class RestClientAuthHandlerTests : IDisposable
    {
        private readonly MockLogger<RestClientAuthHandler> mockLogger;
        private readonly SimulatedHttp simulatedHttp;
        private readonly string baseAddress = new Faker().Internet.Url();

        public RestClientAuthHandlerTests()
        {
            mockLogger = new();
            simulatedHttp = new(baseAddress);
        }

        [Fact]
        public async Task ShouldAddAuthTokenToRequest()
        {
            var scheme = "Bearer";
            var token = GetRandomToken();
            var oAuthToken = new OAuthToken { Scheme = scheme, Token = token };
            var relativePath = new Faker().Internet.UrlRootedPath();

            simulatedHttp.Setup(HttpMethod.Get, relativePath)
                .ReturnsAsync(System.Net.HttpStatusCode.OK, string.Empty);

            var restClientAuthHandler = new RestClientAuthHandler(oAuthToken, mockLogger)
            {
                InnerHandler = simulatedHttp
            };

            var httpClient = new HttpClient(restClientAuthHandler)
            {
                BaseAddress = new Uri(baseAddress)
            };

            var httpResponseMessage = await httpClient.GetAsync(relativePath);

            httpResponseMessage.EnsureSuccessStatusCode();
            Assert.Equal(scheme, httpResponseMessage.RequestMessage.Headers.Authorization.Scheme);
            Assert.Equal(token, httpResponseMessage.RequestMessage.Headers.Authorization.Parameter);
        }

        [Fact]
        public async Task ShouldNotAddAuthTokenToRequestIfTokenEmpty()
        {
            var oAuthToken = new OAuthToken();
            var relativePath = new Faker().Internet.UrlRootedPath();

            simulatedHttp.Setup(HttpMethod.Get, relativePath)
                .ReturnsAsync(System.Net.HttpStatusCode.OK, string.Empty);

            var restClientAuthHandler = new RestClientAuthHandler(oAuthToken, mockLogger)
            {
                InnerHandler = simulatedHttp
            };

            var httpClient = new HttpClient(restClientAuthHandler)
            {
                BaseAddress = new Uri(baseAddress)
            };

            var httpResponseMessage = await httpClient.GetAsync(relativePath);

            httpResponseMessage.EnsureSuccessStatusCode();
            Assert.Equal(default, httpResponseMessage.RequestMessage.Headers.Authorization);
        }

        private static string GetRandomToken() =>
            new Faker().Random.AlphaNumeric(new Faker().Random.Int(10, 20));

        public void Dispose() => simulatedHttp.Dispose();
    }
}
