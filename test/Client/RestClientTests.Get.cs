using BlazorFocused.Model;
using FluentAssertions;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace BlazorFocused.Client
{
    public partial class RestClientTests
    {
        [Fact]
        public async Task ShouldDoGetRequest()
        {
            var url = GetRandomRelativeUrl();
            var expectedResponse = GetRandomResponseObjects();

            simulatedHttp.SetupGET(url)
                .ReturnsAsync(HttpStatusCode.OK, expectedResponse);

            var actualResponse = await restClient.GetAsync<IEnumerable<SimpleClass>>(url);

            actualResponse.Should().BeEquivalentTo(expectedResponse);
        }

        [Fact]
        public async Task ShouldThrowForBadGetRequest()
        {
            var url = GetRandomRelativeUrl();
            var invalidResponse = GetRandomResponseObject();

            simulatedHttp.SetupGET(url)
                .ReturnsAsync(HttpStatusCode.InternalServerError, invalidResponse);

            var actualException = await Record.ExceptionAsync(() =>
                restClient.GetAsync<IEnumerable<SimpleClass>>(url));

            actualException.Should().BeOfType(typeof(RestClientException))
                .And.Match<RestClientException>(exception =>
                    exception.Message.Contains($"{HttpMethod.Get.Method}") &&
                    exception.Message.Contains($"{HttpStatusCode.InternalServerError}") &&
                    exception.Message.Contains(url));
        }

        [Fact]
        public async Task ShouldReturnNullForBadGetRequestOnTry()
        {
            var url = GetRandomRelativeUrl();
            var invalidResponse = GetRandomResponseObject();

            simulatedHttp
                .SetupGET(url)
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
