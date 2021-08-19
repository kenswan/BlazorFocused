using BlazorFocused.Testing;
using BlazorFocused.Utility;
using FluentAssertions;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace BlazorFocused.Client
{
    public class OAuthRestClientTests
    {
        private readonly IOAuthRestClient oAuthRestClient;
        private readonly SimulatedHttp simulatedHttp;
        private readonly MockLogger<OAuthRestClient> mockLogger;
        private readonly HttpClient httpClient;
        private readonly OAuthToken oAuthToken;

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

        [Fact]
        public void ShouldClearAuthorization()
        {
            var scheme = "Bearer";
            var token = "TestToken";
            var addedAuthorization = $"{scheme} {token}";
            var clearedAuthorization = string.Empty;

            oAuthRestClient.AddAuthorization(scheme, token);

            Assert.Equal(addedAuthorization, oAuthRestClient.RetrieveAuthorization());

            oAuthRestClient.ClearAuthorization();

            Assert.Equal(clearedAuthorization, oAuthRestClient.RetrieveAuthorization());
        }

        [Fact]
        public void ShouldDetectAuthorization()
        {
            var scheme = "Bearer";
            var token = "TestToken";

            oAuthRestClient.AddAuthorization(scheme, token);

            Assert.True(oAuthRestClient.HasAuthorization());

            oAuthRestClient.ClearAuthorization();

            Assert.False(oAuthRestClient.HasAuthorization());
        }

        [Fact]
        public void ShouldReturnAuthorization()
        {
            var scheme = "Bearer";
            var token = "TestToken";
            var expectedResponse = $"{scheme} {token}";

            oAuthRestClient.AddAuthorization(scheme, token);

            var actualResponse = oAuthRestClient.RetrieveAuthorization();

            Assert.Equal(expectedResponse, actualResponse);
        }

        public class TestObject : TestClass { }
    }
}
