using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using BlazorFocused.Core.Test.Model;
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

            focusedHttp
                .Setup(HttpMethod.Get, url)
                .ReturnsAsync(HttpStatusCode.OK, expectedResponse);

            var actualResponse = await restClient.GetAsync<IEnumerable<SimpleClass>>(url);

            actualResponse.Should().BeEquivalentTo(expectedResponse);
        }

        [Fact]
        public async Task ShouldReturnNullOnBadGetRequest()
        {
            var url = GetRandomRelativeUrl();
            var invalidResponse = GetRandomResponseObject();

            focusedHttp
                .Setup(HttpMethod.Get, url)
                .ReturnsAsync(HttpStatusCode.BadRequest, invalidResponse);

            var actualResponse = await restClient.GetAsync<IEnumerable<SimpleClass>>(url);

            actualResponse.Should().BeNull();
        }
    }
}
