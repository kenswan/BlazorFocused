using BlazorFocused.Test.Utility;
using BlazorFocused.Testing;
using FluentAssertions;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace BlazorFocused.Client.Test
{
    public class OAuthRestClientTests
    {
        private readonly IOAuthRestClient oAuthRestClient;
        private readonly SimulatedHttp simulatedHttp;
        private readonly MockLogger<OAuthRestClient> mockLogger;
        private HttpClient httpClient;
        private OAuthToken oAuthToken;

        public OAuthRestClientTests()
        {
            oAuthToken = new();
            simulatedHttp = new();
            mockLogger = new();
            httpClient = simulatedHttp.Client();
            oAuthRestClient = new OAuthRestClient(oAuthToken, httpClient, mockLogger);
        }

        [Fact]
        public async Task ShouldAddAuthToken()
        {
            var expectedScheme = "Bearer";
            var expectedToken = "TestToken";
            var apiUrl = "api/test";
            var expectedResponse = new TestObject { CheckedPropertyId = "Testing" };

            simulatedHttp.Setup(HttpMethod.Get, apiUrl)
                .ReturnsAsync(HttpStatusCode.OK, expectedResponse);

            oAuthRestClient.AddAuthorization(expectedScheme, expectedToken);
            var actualResponse = await oAuthRestClient.GetAsync<TestObject>(apiUrl);

            actualResponse.Should().BeEquivalentTo(expectedResponse);
            Assert.Equal(expectedScheme, httpClient.DefaultRequestHeaders.Authorization.Scheme);
            Assert.Equal(expectedToken, httpClient.DefaultRequestHeaders.Authorization.Parameter);
        }

        public class TestObject : TestClass { }
    }
}
