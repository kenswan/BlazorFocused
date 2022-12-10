﻿// -------------------------------------------------------
// Copyright (c) Ken Swan All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using BlazorFocused.Tools.Model;
using FluentAssertions;
using System.Text;
using System.Text.Json;
using Xunit;

namespace BlazorFocused.Extensions;

public partial class RestClientExtensionsTests
{
    [Theory]
    [MemberData(nameof(HttpMethodsForResponse))]
    public async Task ShouldSendNativeHttpRequestAndDeserialize(HttpMethod httpMethod)
    {
        var url = RestClientTestExtensions.GenerateRelativeUrl();
        SimpleClass request = RestClientTestExtensions.GenerateResponseObject();

        var requestMessage = new HttpRequestMessage(httpMethod, url)
        {
            Content = GetStringContent(request)
        };

        System.Net.HttpStatusCode successStatusCode = RestClientTestExtensions.GenerateSuccessStatusCode();
        IEnumerable<SimpleClass> expectedResponse = RestClientTestExtensions.GenerateResponseObjects();

        simulatedHttp.GetHttpSetup(httpMethod, url, request)
            .ReturnsAsync(successStatusCode, expectedResponse);

        RestClientResponse<IEnumerable<SimpleClass>> actualResponse = await restClient.SendAsync<IEnumerable<SimpleClass>>(requestMessage);

        actualResponse.Value.Should().BeEquivalentTo(expectedResponse);

        Assert.True(actualResponse.IsSuccess);
        Assert.Equal(successStatusCode, actualResponse.StatusCode);
        Assert.Null(actualResponse.Exception);

        if (httpMethod != HttpMethod.Delete)
        {
            simulatedHttp.VerifyWasCalled(httpMethod, url, request);
        }
        else
        {
            simulatedHttp.VerifyWasCalled(httpMethod, url);
        }
    }

    [Theory]
    [MemberData(nameof(HttpMethodsForResponse))]
    public async Task ShouldReturnInvalidResponseForNonSuccessStatusCodesForNativeRequest(HttpMethod httpMethod)
    {
        var url = RestClientTestExtensions.GenerateRelativeUrl();
        SimpleClass request = RestClientTestExtensions.GenerateResponseObject();

        var requestMessage = new HttpRequestMessage(httpMethod, url)
        {
            Content = GetStringContent(request)
        };

        System.Net.HttpStatusCode errorStatusCode = RestClientTestExtensions.GenerateErrorStatusCode();
        SimpleClass invalidResponse = RestClientTestExtensions.GenerateResponseObject();

        simulatedHttp.GetHttpSetup(httpMethod, url, request)
            .ReturnsAsync(errorStatusCode, invalidResponse);

        RestClientResponse<IEnumerable<SimpleClass>> actualResponse = await restClient.SendAsync<IEnumerable<SimpleClass>>(requestMessage);

        Assert.False(actualResponse.IsSuccess);
        Assert.Equal(errorStatusCode, actualResponse.StatusCode);

        actualResponse.Exception.Should().BeOfType(typeof(RestClientHttpException))
            .And.Match<RestClientHttpException>(exception =>
                exception.Method == httpMethod &&
                exception.StatusCode == errorStatusCode &&
                exception.Message.Contains(url));
    }

    [Theory]
    [MemberData(nameof(HttpMethodsForTask))]
    public async Task ShouldNativeHttpRequestAndTask(HttpMethod httpMethod)
    {
        var url = RestClientTestExtensions.GenerateRelativeUrl();
        IEnumerable<SimpleClass> request = RestClientTestExtensions.GenerateResponseObjects();

        var requestMessage = new HttpRequestMessage(httpMethod, url)
        {
            Content = GetStringContent(request)
        };

        System.Net.HttpStatusCode successStatusCode = RestClientTestExtensions.GenerateSuccessStatusCode();
        IEnumerable<SimpleClass> expectedResponse = RestClientTestExtensions.GenerateResponseObjects();

        simulatedHttp.GetHttpSetup(httpMethod, url, request)
            .ReturnsAsync(successStatusCode, expectedResponse);

        RestClientTask actualTask = await restClient.SendAsync(requestMessage);

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
    public async Task ShouldReturnInvalidTaskForNonSuccessStatusCodesNativeRequest(HttpMethod httpMethod)
    {
        var url = RestClientTestExtensions.GenerateRelativeUrl();
        IEnumerable<SimpleClass> request = RestClientTestExtensions.GenerateResponseObjects();

        var requestMessage = new HttpRequestMessage(httpMethod, url)
        {
            Content = GetStringContent(request)
        };

        System.Net.HttpStatusCode errorStatusCode = RestClientTestExtensions.GenerateErrorStatusCode();
        SimpleClass invalidResponse = RestClientTestExtensions.GenerateResponseObject();

        simulatedHttp.GetHttpSetup(httpMethod, url, request)
            .ReturnsAsync(errorStatusCode, invalidResponse);

        RestClientTask actualTask = await restClient.SendAsync(requestMessage);

        Assert.False(actualTask.IsSuccess);
        Assert.Equal(errorStatusCode, actualTask.StatusCode);

        actualTask.Exception.Should().BeOfType(typeof(RestClientHttpException))
            .And.Match<RestClientHttpException>(exception =>
                exception.Method == httpMethod &&
                exception.StatusCode == errorStatusCode &&
                exception.Message.Contains(url));
    }

    [Theory]
    [MemberData(nameof(HttpMethodsForResponse))]
    public async Task ShouldSendNativeHttpRequest(HttpMethod httpMethod)
    {
        var url = RestClientTestExtensions.GenerateRelativeUrl();
        SimpleClass request = RestClientTestExtensions.GenerateResponseObject();

        var requestMessage = new HttpRequestMessage(httpMethod, url)
        {
            Content = GetStringContent(request)
        };

        System.Net.HttpStatusCode successStatusCode = RestClientTestExtensions.GenerateSuccessStatusCode();
        IEnumerable<SimpleClass> expectedResponse = RestClientTestExtensions.GenerateResponseObjects();

        simulatedHttp.GetHttpSetup(httpMethod, url, request)
            .ReturnsAsync(successStatusCode, expectedResponse);

        HttpResponseMessage httpResponseMessage = await restClient.BaseSendAsync(requestMessage);

        Assert.True(httpResponseMessage.IsSuccessStatusCode);
        Assert.Equal(successStatusCode, httpResponseMessage.StatusCode);

        if (httpMethod != HttpMethod.Delete)
        {
            simulatedHttp.VerifyWasCalled(httpMethod, url, request);
        }
        else
        {
            simulatedHttp.VerifyWasCalled(httpMethod, url);
        }
    }
    private static StringContent GetStringContent(object requestObject)
    {
        return new(JsonSerializer.Serialize(requestObject), Encoding.UTF8, "application/json");
    }
}
