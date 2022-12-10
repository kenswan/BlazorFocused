// -------------------------------------------------------
// Copyright (c) Ken Swan All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using FluentAssertions;
using Xunit;

namespace BlazorFocused.Client;

public partial class StandaloneClientTests
{
    [Theory]
    [MemberData(nameof(HttpMethodsForTask))]
    public async Task ShouldPerformHttpRequestTask(HttpMethod httpMethod)
    {
        var url = RestClientTestExtensions.GenerateRelativeUrl();
        IEnumerable<Tools.Model.SimpleClass> request = RestClientTestExtensions.GenerateResponseObjects();
        System.Net.HttpStatusCode successStatusCode = RestClientTestExtensions.GenerateSuccessStatusCode();
        IEnumerable<Tools.Model.SimpleClass> expectedResponse = RestClientTestExtensions.GenerateResponseObjects();

        simulatedHttp.GetHttpSetup(httpMethod, url, request)
            .ReturnsAsync(successStatusCode, expectedResponse);

        await MakeTaskRequest(httpMethod, url, request);

        if (httpMethod == HttpMethod.Delete)
        {
            simulatedHttp.VerifyWasCalled(httpMethod, url);
        }
        else
        {
            simulatedHttp.VerifyWasCalled(httpMethod, url, request);
        }
    }

    [Theory]
    [MemberData(nameof(HttpMethodsForTask))]
    public async Task ShouldThrowForTaskNonSuccessStatusCodes(HttpMethod httpMethod)
    {
        var url = RestClientTestExtensions.GenerateRelativeUrl();
        IEnumerable<Tools.Model.SimpleClass> request = RestClientTestExtensions.GenerateResponseObjects();
        System.Net.HttpStatusCode errorStatusCode = RestClientTestExtensions.GenerateErrorStatusCode();
        Tools.Model.SimpleClass invalidResponse = RestClientTestExtensions.GenerateResponseObject();

        simulatedHttp.GetHttpSetup(httpMethod, url, request)
            .ReturnsAsync(errorStatusCode, invalidResponse);

        Exception actualException = await Record.ExceptionAsync(() =>
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

    public static TheoryData<HttpMethod> HttpMethodsForTask =>
        new()
        {
            { HttpMethod.Delete },
            { HttpMethod.Patch },
            { HttpMethod.Post },
            { HttpMethod.Put },
        };
}
