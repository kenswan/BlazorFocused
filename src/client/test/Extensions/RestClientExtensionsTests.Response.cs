// -------------------------------------------------------
// Copyright (c) Ken Swan All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using BlazorFocused.Tools.Model;
using FluentAssertions;
using Moq;
using Xunit;

namespace BlazorFocused.Extensions;

public partial class RestClientExtensionsTests
{
    [Theory]
    [MemberData(nameof(HttpMethodsForResponse))]
    public async Task ShouldPerformTryHttpRequest(HttpMethod httpMethod)
    {
        string url = RestClientTestExtensions.GenerateRelativeUrl();
        IEnumerable<SimpleClass> request = RestClientTestExtensions.GenerateResponseObjects();
        System.Net.HttpStatusCode successStatusCode = RestClientTestExtensions.GenerateSuccessStatusCode();
        SimpleClass expectedResponse = RestClientTestExtensions.GenerateResponseObject();

        simulatedHttp.GetHttpSetup(httpMethod, url, request)
            .ReturnsAsync(successStatusCode, expectedResponse);

        RestClientResponse<SimpleClass> actualClientResponse =
            await MakeTryRequest<SimpleClass>(httpMethod, url, request);

        Assert.True(actualClientResponse.IsSuccess);
        Assert.Equal(successStatusCode, actualClientResponse.StatusCode);
        Assert.Null(actualClientResponse.Exception);

        actualClientResponse.Value.Should().BeEquivalentTo(expectedResponse);

        if (httpMethod == HttpMethod.Delete || httpMethod == HttpMethod.Get)
        {
            simulatedHttp.VerifyWasCalled(httpMethod, url);
        }
        else
        {
            simulatedHttp.VerifyWasCalled(httpMethod, url, request);
        }
    }

    [Theory]
    [MemberData(nameof(HttpMethodsForResponse))]
    public async Task ShouldReturnInvalidResponseForNonSuccessStatusCodes(HttpMethod httpMethod)
    {
        string url = RestClientTestExtensions.GenerateRelativeUrl();
        IEnumerable<SimpleClass> request = RestClientTestExtensions.GenerateResponseObjects();
        System.Net.HttpStatusCode errorStatusCode = RestClientTestExtensions.GenerateErrorStatusCode();
        SimpleClass invalidResponse = RestClientTestExtensions.GenerateResponseObject();

        simulatedHttp.GetHttpSetup(httpMethod, url, request)
            .ReturnsAsync(errorStatusCode, invalidResponse);

        RestClientResponse<SimpleClass> actualClientResponse =
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

    [Theory]
    [MemberData(nameof(HttpMethodsForResponse))]
    public async Task ShouldPerformTryHttpRequestWithMock(HttpMethod httpMethod)
    {
        string url = RestClientTestExtensions.GenerateRelativeUrl();

        IEnumerable<SimpleClass> request = (httpMethod != HttpMethod.Delete && httpMethod != HttpMethod.Get) ?
            RestClientTestExtensions.GenerateResponseObjects() : null;

        System.Net.HttpStatusCode successStatusCode = RestClientTestExtensions.GenerateSuccessStatusCode();
        SimpleClass expectedResponse = RestClientTestExtensions.GenerateResponseObject();

        restClientMock.Setup(client =>
            client.SendAsync<SimpleClass>(httpMethod, url, request))
                .ReturnsAsync(new RestClientResponse<SimpleClass>
                {
                    StatusCode = successStatusCode,
                    Value = expectedResponse
                });

        RestClientResponse<SimpleClass> actualClientResponse =
            await MakeTryRequestWithMock<SimpleClass>(restClientMock.Object, httpMethod, url, request);

        Assert.True(actualClientResponse.IsSuccess);
        Assert.Equal(successStatusCode, actualClientResponse.StatusCode);
        Assert.Null(actualClientResponse.Exception);

        actualClientResponse.Value.Should().BeEquivalentTo(expectedResponse);

        if (httpMethod == HttpMethod.Delete || httpMethod == HttpMethod.Get)
        {
            restClientMock.Verify(client =>
                client.SendAsync<SimpleClass>(httpMethod, url, null),
                    Times.Once());
        }
        else
        {
            restClientMock.Verify(client =>
                client.SendAsync<SimpleClass>(httpMethod, url, request),
                    Times.Once());
        }
    }

    private Task<RestClientResponse<T>> MakeTryRequest<T>(HttpMethod httpMethod, string url, object request) => httpMethod switch
    {
        HttpMethod method when method == HttpMethod.Delete => restClient.TryDeleteAsync<T>(url),
        HttpMethod method when method == HttpMethod.Get => restClient.TryGetAsync<T>(url),
        HttpMethod method when method == HttpMethod.Patch => restClient.TryPatchAsync<T>(url, request),
        HttpMethod method when method == HttpMethod.Post => restClient.TryPostAsync<T>(url, request),
        HttpMethod method when method == HttpMethod.Put => restClient.TryPutAsync<T>(url, request),
        _ => throw new ArgumentException($"{httpMethod} not supported"),
    };

    private static Task<RestClientResponse<T>> MakeTryRequestWithMock<T>(IRestClient restClient, HttpMethod httpMethod, string url, object request) => httpMethod switch
    {
        HttpMethod method when method == HttpMethod.Delete => restClient.TryDeleteAsync<T>(url),
        HttpMethod method when method == HttpMethod.Get => restClient.TryGetAsync<T>(url),
        HttpMethod method when method == HttpMethod.Patch => restClient.TryPatchAsync<T>(url, request),
        HttpMethod method when method == HttpMethod.Post => restClient.TryPostAsync<T>(url, request),
        HttpMethod method when method == HttpMethod.Put => restClient.TryPutAsync<T>(url, request),
        _ => throw new ArgumentException($"{httpMethod} not supported"),
    };
}
