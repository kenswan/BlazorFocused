using BlazorFocused.Tools.Model;
using FluentAssertions;
using System.Net;
using System.Text.Json;
using Xunit;

namespace BlazorFocused.Tools.Testing
{
    public partial class SimulatedHttpTests
    {
        [Fact]
        public void ShouldSetBaseUri()
        {
            var actualBaseAddress = simulatedHttp.HttpClient.BaseAddress.OriginalString;

            Assert.Equal(baseAddress, actualBaseAddress);
        }

        [Theory]
        [MemberData(nameof(HttpData))]
        public async Task ShouldReturnRequestedResponse(
            HttpMethod httpMethod,
            HttpStatusCode httpStatusCode,
            string relativeRequestUrl,
            SimpleClass requestObject,
            SimpleClass responseObject)
        {
            ISimulatedHttpSetup setup = GetHttpSetup(httpMethod, relativeRequestUrl, requestObject);
            setup.ReturnsAsync(httpStatusCode, responseObject);

            using var client = simulatedHttp.HttpClient;

            HttpResponseMessage actualResponse =
                await MakeRequest(client, httpMethod, relativeRequestUrl, requestObject);

            var actualResponseString = await actualResponse.Content.ReadAsStringAsync();
            var actualResponseObject = JsonSerializer.Deserialize<SimpleClass>(actualResponseString);

            Assert.Equal(httpStatusCode, actualResponse.StatusCode);
            actualResponseObject.Should().BeEquivalentTo(responseObject);
        }
    }
}
