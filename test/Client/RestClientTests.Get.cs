using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using BlazorFocused.Test.Model;
using FluentAssertions;
using Xunit;

namespace BlazorFocused.Client.Test
{
    public partial class RestClientTests
    {
        [Fact]
        public async Task ShouldDoGetRequest()
        {
            var url = GetRandomRelativeUrl();
            var expectedResponse = GetRandomResponseObjects();

            simulatedHttp
                .Setup(HttpMethod.Get, url)
                .ReturnsAsync(HttpStatusCode.OK, expectedResponse);

            var actualResponse = await restClient.GetAsync<IEnumerable<SimpleClass>>(url);

            actualResponse.Should().BeEquivalentTo(expectedResponse);
        }

        [Fact]
        public async Task ShouldReturnNullForBadGetRequestOnTry()
        {
            var url = GetRandomRelativeUrl();
            var invalidResponse = GetRandomResponseObject();

            simulatedHttp
                .Setup(HttpMethod.Get, url)
                .ReturnsAsync(HttpStatusCode.BadRequest, invalidResponse);

            var actualResponse = await restClient.TryGetAsync<IEnumerable<SimpleClass>>(url);

            actualResponse.Should().NotBeNull()
                .And.Match<RestClientResponse<IEnumerable<SimpleClass>>>(response => 
                    response.Value == default &&
                    response.IsValid == false &&
                    response.StatusCode == HttpStatusCode.BadRequest);
        }
    }
}
