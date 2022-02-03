using BlazorFocused.Tools.Client;
using BlazorFocused.Tools.Logger;
using Bogus;
using Xunit;

namespace BlazorFocused.Client
{
    public class RestClientAuthHandlerTests
    {
        private readonly MockLogger<RestClientAuthHandler> mockLogger;
        private readonly ISimulatedHttp simulatedHttp;
        private readonly string baseAddress = new Faker().Internet.Url();

        public RestClientAuthHandlerTests()
        {
            mockLogger = new();
            simulatedHttp = new SimulatedHttp(baseAddress);
        }

        [Fact]
        public async Task ShouldAddAuthTokenToRequest()
        {
            var scheme = "Bearer";
            var token = GetRandomToken();
            var oAuthToken = new OAuthToken { Scheme = scheme, Token = token };
            var relativePath = new Faker().Internet.UrlRootedPath();

            simulatedHttp.SetupGET(relativePath)
                .ReturnsAsync(System.Net.HttpStatusCode.OK, string.Empty);

            var restClientAuthHandler = new RestClientAuthHandler(oAuthToken, mockLogger)
            {
                InnerHandler = simulatedHttp.DelegatingHandler
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

            simulatedHttp.SetupGET(relativePath)
                .ReturnsAsync(System.Net.HttpStatusCode.OK, string.Empty);

            var restClientAuthHandler = new RestClientAuthHandler(oAuthToken, mockLogger)
            {
                InnerHandler = simulatedHttp.DelegatingHandler
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
    }
}
