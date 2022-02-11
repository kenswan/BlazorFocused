using BlazorFocused.Client.Extensions;
using BlazorFocused.Tools.Model;
using FluentAssertions;
using Xunit;

namespace BlazorFocused.Client
{
    public partial class RestClientTests
    {
        [Theory]
        [MemberData(nameof(HttpMethodSelectionForResponse))]
        public async Task ShouldPerformTryHttpRequest(HttpMethod httpMethod)
        {
            var url = GetRandomRelativeUrl();
            var request = GetRandomResponseObjects();
            var successStatusCode = GenerateSuccessStatusCode();
            var expectedResponse = GetRandomResponseObject();

            GetHttpSetup(httpMethod, url, request)
                .ReturnsAsync(successStatusCode, expectedResponse);

            var actualClientResponse =
                await MakeTryRequest<SimpleClass>(httpMethod, url, request);

            Assert.True(actualClientResponse.IsSuccess);
            Assert.Equal(successStatusCode, actualClientResponse.StatusCode);
            Assert.Null(actualClientResponse.Exception);

            actualClientResponse.Value.Should().BeEquivalentTo(expectedResponse);

            if (httpMethod == HttpMethod.Delete || httpMethod == HttpMethod.Get)
                simulatedHttp.VerifyWasCalled(httpMethod, url);
            else
                simulatedHttp.VerifyWasCalled(httpMethod, url, request);
        }

        [Theory]
        [MemberData(nameof(HttpMethodSelectionForResponse))]
        public async Task ShouldReturnInvalidResponseForNonSuccessStatusCodes(HttpMethod httpMethod)
        {
            var url = GetRandomRelativeUrl();
            var request = GetRandomResponseObjects();
            var errorStatusCode = GenerateErrorStatusCode();
            var invalidResponse = GetRandomResponseObject();

            GetHttpSetup(httpMethod, url, request)
                .ReturnsAsync(errorStatusCode, invalidResponse);

            var actualClientResponse =
                await MakeTryRequest<SimpleClass>(httpMethod, url, request);

            Assert.False(actualClientResponse.IsSuccess);
            Assert.Equal(errorStatusCode, actualClientResponse.StatusCode);
            Assert.Null(actualClientResponse.Value);

            actualClientResponse.Exception.Should().BeOfType(typeof(RestClientHttpException))
                .And.Match<RestClientHttpException>(exception =>
                    exception.Method == httpMethod &&
                    exception.StatusCode == errorStatusCode &&
                    exception.Message.Contains(url));
        }

        private Task<RestClientResponse<T>> MakeTryRequest<T>(HttpMethod httpMethod, string url, object request)
        {
            return httpMethod switch
            {
                HttpMethod method when method == HttpMethod.Delete => restClient.TryDeleteAsync<T>(url),
                HttpMethod method when method == HttpMethod.Get => restClient.TryGetAsync<T>(url),
                HttpMethod method when method == HttpMethod.Patch => restClient.TryPatchAsync<T>(url, request),
                HttpMethod method when method == HttpMethod.Post => restClient.TryPostAsync<T>(url, request),
                HttpMethod method when method == HttpMethod.Put => restClient.TryPutAsync<T>(url, request),
                _ => throw new ArgumentException($"{httpMethod} not supported"),
            };
        }
    }
}
