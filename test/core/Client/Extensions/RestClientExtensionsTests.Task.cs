using BlazorFocused.Client.Extensions;
using FluentAssertions;
using Xunit;

namespace BlazorFocused.Client
{
    public partial class RestClientTests
    {
        [Theory]
        [MemberData(nameof(HttpMethodSelectionForTask))]
        public async Task ShouldTryHttpRequestTask(HttpMethod httpMethod)
        {
            var url = GetRandomRelativeUrl();
            var request = GetRandomResponseObjects();
            var successStatusCode = GenerateSuccessStatusCode();
            var expectedResponse = GetRandomResponseObjects();

            GetHttpSetup(httpMethod, url, request)
                .ReturnsAsync(successStatusCode, expectedResponse);

            var actualTask = await MakeTryTaskRequest(httpMethod, url, request);

            Assert.True(actualTask.IsSuccess);
            Assert.Equal(successStatusCode, actualTask.StatusCode);
            Assert.Null(actualTask.Exception);

            if (httpMethod == HttpMethod.Delete)
                simulatedHttp.VerifyWasCalled(httpMethod, url);
            else
                simulatedHttp.VerifyWasCalled(httpMethod, url, request);
        }

        [Theory]
        [MemberData(nameof(HttpMethodSelectionForTask))]
        public async Task ShouldReturnInvalidTaskForNonSuccessStatusCodes(HttpMethod httpMethod)
        {
            var url = GetRandomRelativeUrl();
            var request = GetRandomResponseObjects();
            var errorStatusCode = GenerateErrorStatusCode();
            var invalidResponse = GetRandomResponseObject();

            GetHttpSetup(httpMethod, url, request)
                .ReturnsAsync(errorStatusCode, invalidResponse);

            var actualTask = await MakeTryTaskRequest(httpMethod, url, request);

            Assert.False(actualTask.IsSuccess);
            Assert.Equal(errorStatusCode, actualTask.StatusCode);

            actualTask.Exception.Should().BeOfType(typeof(RestClientHttpException))
                .And.Match<RestClientHttpException>(exception =>
                    exception.Method == httpMethod &&
                    exception.StatusCode == errorStatusCode &&
                    exception.Message.Contains(url));
        }

        private Task<RestClientTask> MakeTryTaskRequest(HttpMethod httpMethod, string url, object request)
        {
            return httpMethod switch
            {
                HttpMethod method when method == HttpMethod.Delete => restClient.TryDeleteTaskAsync(url),
                HttpMethod method when method == HttpMethod.Patch => restClient.TryPatchTaskAsync(url, request),
                HttpMethod method when method == HttpMethod.Post => restClient.TryPostTaskAsync(url, request),
                HttpMethod method when method == HttpMethod.Put => restClient.TryPutTaskAsync(url, request),
                _ => throw new Exception($"{httpMethod} not supported"),
            };
        }
    }
}
