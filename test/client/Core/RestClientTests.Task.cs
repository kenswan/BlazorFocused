using FluentAssertions;
using Xunit;

namespace BlazorFocused.Client
{
    public partial class RestClientTests
    {
        [Theory]
        [MemberData(nameof(HttpMethodSelectionForTask))]
        public async Task ShouldPerformHttpRequestTask(HttpMethod httpMethod)
        {
            var url = GetRandomRelativeUrl();
            var request = GetRandomResponseObjects();
            var successStatusCode = GenerateSuccessStatusCode();
            var expectedResponse = GetRandomResponseObjects();

            GetHttpSetup(httpMethod, url, request)
                .ReturnsAsync(successStatusCode, expectedResponse);

            await MakeTaskRequest(httpMethod, url, request);

            if (httpMethod == HttpMethod.Delete)
                simulatedHttp.VerifyWasCalled(httpMethod, url);
            else
                simulatedHttp.VerifyWasCalled(httpMethod, url, request);
        }

        [Theory]
        [MemberData(nameof(HttpMethodSelectionForTask))]
        public async Task ShouldThrowForTaskNonSuccessStatusCodes(HttpMethod httpMethod)
        {
            var url = GetRandomRelativeUrl();
            var request = GetRandomResponseObjects();
            var errorStatusCode = GenerateErrorStatusCode();
            var invalidResponse = GetRandomResponseObject();

            GetHttpSetup(httpMethod, url, request)
                .ReturnsAsync(errorStatusCode, invalidResponse);

            var actualException = await Record.ExceptionAsync(() =>
                MakeTaskRequest(httpMethod, url, request));

            actualException.Should().BeOfType(typeof(RestClientHttpException))
                .And.Match<RestClientHttpException>(exception =>
                    exception.Method == httpMethod &&
                    exception.StatusCode == errorStatusCode &&
                    exception.Message.Contains(url));
        }

        private Task MakeTaskRequest(HttpMethod httpMethod, string url, object request)
        {
            return httpMethod switch
            {
                HttpMethod method when method == HttpMethod.Delete => restClient.DeleteTaskAsync(url),
                HttpMethod method when method == HttpMethod.Patch => restClient.PatchTaskAsync(url, request),
                HttpMethod method when method == HttpMethod.Post => restClient.PostTaskAsync(url, request),
                HttpMethod method when method == HttpMethod.Put => restClient.PutTaskAsync(url, request),
                _ => throw new Exception($"{httpMethod} not supported"),
            };
        }

        public static TheoryData<HttpMethod> HttpMethodSelectionForTask =>
            new()
            {
                { HttpMethod.Delete },
                { HttpMethod.Patch },
                { HttpMethod.Post },
                { HttpMethod.Put },
            };
    }
}
