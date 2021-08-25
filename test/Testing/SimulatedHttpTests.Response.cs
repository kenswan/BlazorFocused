using BlazorFocused.Model;
using FluentAssertions;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace BlazorFocused.Testing
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
            SimpleClass responseObject)
        {
            simulatedHttp
                .Setup(httpMethod, relativeRequestUrl)
                .ReturnsAsync(httpStatusCode, responseObject);

            using var client = simulatedHttp.HttpClient;
            HttpResponseMessage actualResponse = await MakeRequest(client, httpMethod, relativeRequestUrl);
            var actualResponseString = await actualResponse.Content.ReadAsStringAsync();
            var actualResponseObject = JsonSerializer.Deserialize<SimpleClass>(actualResponseString);

            Assert.Equal(httpStatusCode, actualResponse.StatusCode);
            actualResponseObject.Should().BeEquivalentTo(responseObject);
        }
    }
}
