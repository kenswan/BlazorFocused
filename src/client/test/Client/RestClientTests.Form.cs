// -------------------------------------------------------
// Copyright (c) Ken Swan All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using BlazorFocused.Tools.Model;
using FluentAssertions;
using System.Net;
using Xunit;

namespace BlazorFocused.Client;
public partial class RestClientTests
{
    [Theory]
    [MemberData(nameof(HttpMethodsForResponse))]
    public async Task ShouldSendFormUrlEncodedContent(HttpMethod httpMethod)
    {
        if (httpMethod == HttpMethod.Delete || httpMethod == HttpMethod.Get)
        {
            return;
        }

        string url = RestClientTestExtensions.GenerateRelativeUrl();
        HttpStatusCode successStatusCode = RestClientTestExtensions.GenerateSuccessStatusCode();
        IEnumerable<SimpleClass> expectedResponse = RestClientTestExtensions.GenerateResponseObjects();

        KeyValuePair<string, string>[] formInputData = new[]
        {
            new KeyValuePair<string, string>("testField1", "testValue1"),
            new KeyValuePair<string, string>("testField2", "testValue2"),
            new KeyValuePair<string, string>("testField3", "testValue3")
        };

        using var formContent = new FormUrlEncodedContent(formInputData);

        simulatedHttp.GetHttpSetup(httpMethod, url, formContent)
            .ReturnsAsync(successStatusCode, expectedResponse);

        IEnumerable<SimpleClass> actualResponse =
            await MakeRequest<IEnumerable<SimpleClass>>(httpMethod, url, formContent);

        actualResponse.Should().BeEquivalentTo(expectedResponse);

        simulatedHttp.VerifyWasCalled(httpMethod, url, formContent);
    }

    [Theory]
    [MemberData(nameof(HttpMethodsForResponse))]
    public async Task ShouldSendMultipartFormData(HttpMethod httpMethod)
    {
        if (httpMethod == HttpMethod.Delete || httpMethod == HttpMethod.Get)
        {
            return;
        }

        string url = RestClientTestExtensions.GenerateRelativeUrl();
        HttpStatusCode successStatusCode = RestClientTestExtensions.GenerateSuccessStatusCode();
        IEnumerable<SimpleClass> expectedResponse = RestClientTestExtensions.GenerateResponseObjects();

        using var multipartFormDataContent = new MultipartFormDataContent
        {
            { new StringContent("testField1"), "testValue1" },
            { new StringContent("testField2"), "testValue2" },
            { new StringContent("testField3"), "testValue3" }
        };

        simulatedHttp.GetHttpSetup(httpMethod, url, multipartFormDataContent)
            .ReturnsAsync(successStatusCode, expectedResponse);

        IEnumerable<SimpleClass> actualResponse =
            await MakeRequest<IEnumerable<SimpleClass>>(httpMethod, url, multipartFormDataContent);

        actualResponse.Should().BeEquivalentTo(expectedResponse);

        simulatedHttp.VerifyWasCalled(httpMethod, url, multipartFormDataContent);
    }
}
