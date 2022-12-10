// -------------------------------------------------------
// Copyright (c) Ken Swan All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using FluentAssertions;
using Xunit;

namespace BlazorFocused.Extensions;

public partial class RestClientExtensionsTests
{
    [Theory]
    [MemberData(nameof(HttpMethodsForTask))]
    public async Task ShouldTryHttpRequestTask(HttpMethod httpMethod)
    {
        var url = RestClientTestExtensions.GenerateRelativeUrl();
        IEnumerable<Tools.Model.SimpleClass> request = RestClientTestExtensions.GenerateResponseObjects();
        System.Net.HttpStatusCode successStatusCode = RestClientTestExtensions.GenerateSuccessStatusCode();
        IEnumerable<Tools.Model.SimpleClass> expectedResponse = RestClientTestExtensions.GenerateResponseObjects();

        simulatedHttp.GetHttpSetup(httpMethod, url, request)
            .ReturnsAsync(successStatusCode, expectedResponse);

        RestClientTask actualTask = await MakeTryTaskRequest(httpMethod, url, request);

        Assert.True(actualTask.IsSuccess);
        Assert.Equal(successStatusCode, actualTask.StatusCode);
        Assert.Null(actualTask.Exception);

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
    public async Task ShouldReturnInvalidTaskForNonSuccessStatusCodes(HttpMethod httpMethod)
    {
        var url = RestClientTestExtensions.GenerateRelativeUrl();
        IEnumerable<Tools.Model.SimpleClass> request = RestClientTestExtensions.GenerateResponseObjects();
        System.Net.HttpStatusCode errorStatusCode = RestClientTestExtensions.GenerateErrorStatusCode();
        Tools.Model.SimpleClass invalidResponse = RestClientTestExtensions.GenerateResponseObject();

        simulatedHttp.GetHttpSetup(httpMethod, url, request)
            .ReturnsAsync(errorStatusCode, invalidResponse);

        RestClientTask actualTask = await MakeTryTaskRequest(httpMethod, url, request);

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
