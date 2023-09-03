// -------------------------------------------------------
// Copyright (c) Ken Swan All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using BlazorFocused.Tools.Model;
using FluentAssertions;
using Moq;
using System.Net;
using Xunit;

namespace BlazorFocused.Extensions;

public partial class RestClientExtensionsTests
{
    [Theory]
    [MemberData(nameof(HttpMethodsForTask))]
    public async Task ShouldTryHttpRequestTask(HttpMethod httpMethod)
    {
        string url = RestClientTestExtensions.GenerateRelativeUrl();
        IEnumerable<SimpleClass> request = RestClientTestExtensions.GenerateResponseObjects();
        HttpStatusCode successStatusCode = RestClientTestExtensions.GenerateSuccessStatusCode();
        IEnumerable<SimpleClass> expectedResponse = RestClientTestExtensions.GenerateResponseObjects();

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
        string url = RestClientTestExtensions.GenerateRelativeUrl();
        IEnumerable<SimpleClass> request = RestClientTestExtensions.GenerateResponseObjects();
        HttpStatusCode errorStatusCode = RestClientTestExtensions.GenerateErrorStatusCode();
        SimpleClass invalidResponse = RestClientTestExtensions.GenerateResponseObject();

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

    [Theory]
    [MemberData(nameof(HttpMethodsForTask))]
    public async Task ShouldPerformTryHttpRequestTaskWithMock(HttpMethod httpMethod)
    {
        string url = RestClientTestExtensions.GenerateRelativeUrl();

        IEnumerable<SimpleClass> request = (httpMethod != HttpMethod.Delete) ?
            RestClientTestExtensions.GenerateResponseObjects() : null;

        HttpStatusCode successStatusCode = RestClientTestExtensions.GenerateSuccessStatusCode();
        SimpleClass expectedResponse = RestClientTestExtensions.GenerateResponseObject();

        restClientMock.Setup(client =>
            client.SendAsync(httpMethod, url, request))
                .ReturnsAsync(new RestClientTask
                {
                    StatusCode = successStatusCode,
                });

        RestClientTask actualTask = await MakeTryRequestTaskWithMock(restClientMock.Object, httpMethod, url, request);

        Assert.True(actualTask.IsSuccess);
        Assert.Equal(successStatusCode, actualTask.StatusCode);
        Assert.Null(actualTask.Exception);

        if (httpMethod == HttpMethod.Delete || httpMethod == HttpMethod.Get)
        {
            restClientMock.Verify(client =>
                client.SendAsync(httpMethod, url, null),
                    Times.Once());
        }
        else
        {
            restClientMock.Verify(client =>
                client.SendAsync(httpMethod, url, request),
                    Times.Once());
        }
    }

    private Task<RestClientTask> MakeTryTaskRequest(HttpMethod httpMethod, string url, object request) => httpMethod switch
    {
        HttpMethod method when method == HttpMethod.Delete => restClient.TryDeleteTaskAsync(url),
        HttpMethod method when method == HttpMethod.Patch => restClient.TryPatchTaskAsync(url, request),
        HttpMethod method when method == HttpMethod.Post => restClient.TryPostTaskAsync(url, request),
        HttpMethod method when method == HttpMethod.Put => restClient.TryPutTaskAsync(url, request),
        _ => throw new Exception($"{httpMethod} not supported"),
    };

    private static Task<RestClientTask> MakeTryRequestTaskWithMock(IRestClient restClient, HttpMethod httpMethod, string url, object request) => httpMethod switch
    {
        HttpMethod method when method == HttpMethod.Delete => restClient.TryDeleteTaskAsync(url),
        HttpMethod method when method == HttpMethod.Patch => restClient.TryPatchTaskAsync(url, request),
        HttpMethod method when method == HttpMethod.Post => restClient.TryPostTaskAsync(url, request),
        HttpMethod method when method == HttpMethod.Put => restClient.TryPutTaskAsync(url, request),
        _ => throw new ArgumentException($"{httpMethod} not supported"),
    };
}
