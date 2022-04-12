using BlazorFocused.Tools.Model;
using FluentAssertions;
using System.Text.Json;
using Xunit;

namespace BlazorFocused.Client;

public partial class RestClientTests
{
    [Theory]
    [MemberData(nameof(HttpMethodsForResponse))]
    public async Task ShouldPerformHttpRequest(HttpMethod httpMethod)
    {
        var url = RestClientTestExtensions.GenerateRelativeUrl();
        var request = RestClientTestExtensions.GenerateResponseObjects();
        var successStatusCode = RestClientTestExtensions.GenerateSuccessStatusCode();
        var expectedResponse = RestClientTestExtensions.GenerateResponseObjects();

        simulatedHttp.GetHttpSetup(httpMethod, url, request)
            .ReturnsAsync(successStatusCode, expectedResponse);

        var actualResponse =
            await MakeRequest<IEnumerable<SimpleClass>>(httpMethod, url, request);

        actualResponse.Should().BeEquivalentTo(expectedResponse);

        if (httpMethod == HttpMethod.Delete || httpMethod == HttpMethod.Get)
            simulatedHttp.VerifyWasCalled(httpMethod, url);
        else
            simulatedHttp.VerifyWasCalled(httpMethod, url, request);
    }

    [Theory]
    [MemberData(nameof(HttpMethodsForResponse))]
    public async Task ShouldThrowForNonSuccessStatusCodes(HttpMethod httpMethod)
    {
        var url = RestClientTestExtensions.GenerateRelativeUrl();
        var request = RestClientTestExtensions.GenerateResponseObjects();
        var errorStatusCode = RestClientTestExtensions.GenerateErrorStatusCode();
        var invalidResponse = RestClientTestExtensions.GenerateResponseObject();
        var invalidResponseString = JsonSerializer.Serialize(invalidResponse);

        simulatedHttp.GetHttpSetup(httpMethod, url, request)
            .ReturnsAsync(errorStatusCode, invalidResponse);

        var actualException = await Record.ExceptionAsync(() =>
            MakeRequest<IEnumerable<SimpleClass>>(httpMethod, url, request));

        actualException.Should().BeOfType(typeof(RestClientHttpException))
            .And.Match<RestClientHttpException>(exception =>
                exception.Method == httpMethod &&
                exception.StatusCode == errorStatusCode &&
                exception.Message.Contains(url) &&
                exception.Content == invalidResponseString);
    }

    [Theory]
    [MemberData(nameof(HttpMethodsForResponse))]
    public async Task ShouldReturnEmptyForErrorResponseContentIfNotAvailable(HttpMethod httpMethod)
    {
        var url = RestClientTestExtensions.GenerateRelativeUrl();
        var request = RestClientTestExtensions.GenerateResponseObjects();
        var errorStatusCode = RestClientTestExtensions.GenerateErrorStatusCode();
        var invalidResponse = RestClientTestExtensions.GenerateResponseObject();

        simulatedHttp.GetHttpSetup(httpMethod, url, request)
            .ReturnsAsync(errorStatusCode, null);

        var actualException = await Record.ExceptionAsync(() =>
            MakeRequest<IEnumerable<SimpleClass>>(httpMethod, url, request));

        actualException.Should().BeOfType(typeof(RestClientHttpException))
            .And.Match<RestClientHttpException>(exception =>
                exception.Method == httpMethod &&
                exception.StatusCode == errorStatusCode &&
                exception.Message.Contains(url) &&
                exception.Content == string.Empty);
    }

    private Task<T> MakeRequest<T>(HttpMethod httpMethod, string url, object request)
    {
        return httpMethod switch
        {
            HttpMethod method when method == HttpMethod.Delete => restClient.DeleteAsync<T>(url),
            HttpMethod method when method == HttpMethod.Get => restClient.GetAsync<T>(url),
            HttpMethod method when method == HttpMethod.Patch => restClient.PatchAsync<T>(url, request),
            HttpMethod method when method == HttpMethod.Post => restClient.PostAsync<T>(url, request),
            HttpMethod method when method == HttpMethod.Put => restClient.PutAsync<T>(url, request),
            _ => throw new ArgumentException($"{httpMethod} not supported"),
        };
    }

    public static TheoryData<HttpMethod> HttpMethodsForResponse =>
        new()
        {
            { HttpMethod.Delete },
            { HttpMethod.Get },
            { HttpMethod.Patch },
            { HttpMethod.Post },
            { HttpMethod.Put },
        };
}
