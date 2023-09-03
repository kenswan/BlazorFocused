// -------------------------------------------------------
// Copyright (c) Ken Swan All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using BlazorFocused.Tools.Model;
using Bogus;
using Xunit;

namespace BlazorFocused.Extensions;
public partial class RestClientExtensionsTests
{
    [Theory]
    [MemberData(nameof(HttpMethodsForResponse))]
    public async Task ShouldReturnResponseHeaders(HttpMethod httpMethod)
    {
        string url = RestClientTestExtensions.GenerateRelativeUrl();
        IEnumerable<SimpleClass> request = RestClientTestExtensions.GenerateResponseObjects();
        System.Net.HttpStatusCode successStatusCode = RestClientTestExtensions.GenerateSuccessStatusCode();
        SimpleClass expectedResponse = RestClientTestExtensions.GenerateResponseObject();
        string keyOne = "X-IPv6-Address";
        string valueOneA = new Faker().Internet.Ipv6();
        string valueOneB = new Faker().Internet.Ipv6();
        string keyTwo = "X-IPv4-Address";
        string valueTwo = new Faker().Internet.Ip();

        simulatedHttp.AddResponseHeader(keyOne, valueOneA);
        simulatedHttp.AddResponseHeader(keyOne, valueOneB);
        simulatedHttp.AddResponseHeader(keyTwo, valueTwo);

        simulatedHttp.GetHttpSetup(httpMethod, url, request)
            .ReturnsAsync(successStatusCode, expectedResponse);

        RestClientResponse<SimpleClass> actualClientResponse =
            await MakeTryRequest<SimpleClass>(httpMethod, url, request);

        bool firstKeyExists = actualClientResponse.Headers.TryGetValues(keyOne, out IEnumerable<string>? firstValueSet);
        bool secondKeyExists = actualClientResponse.Headers.TryGetValues(keyTwo, out IEnumerable<string>? secondValueSet);

        Assert.True(firstKeyExists);
        Assert.True(secondKeyExists);

        Assert.Equal(2, firstValueSet.Count());
        Assert.Single(secondValueSet);

        Assert.True(firstValueSet.Contains(valueOneA) && firstValueSet.Contains(valueOneB));
        Assert.Equal(valueTwo, secondValueSet.FirstOrDefault());
    }

    [Theory]
    [MemberData(nameof(HttpMethodsForTask))]
    public async Task ShouldReturnTaskHeaders(HttpMethod httpMethod)
    {
        string url = RestClientTestExtensions.GenerateRelativeUrl();
        IEnumerable<SimpleClass> request = RestClientTestExtensions.GenerateResponseObjects();
        System.Net.HttpStatusCode successStatusCode = RestClientTestExtensions.GenerateSuccessStatusCode();
        SimpleClass expectedResponse = RestClientTestExtensions.GenerateResponseObject();
        string keyOne = "X-IPv6-Address";
        string valueOneA = new Faker().Internet.Ipv6();
        string valueOneB = new Faker().Internet.Ipv6();
        string keyTwo = "X-IPv4-Address";
        string valueTwo = new Faker().Internet.Ip();

        simulatedHttp.AddResponseHeader(keyOne, valueOneA);
        simulatedHttp.AddResponseHeader(keyOne, valueOneB);
        simulatedHttp.AddResponseHeader(keyTwo, valueTwo);

        simulatedHttp.GetHttpSetup(httpMethod, url, request)
            .ReturnsAsync(successStatusCode, expectedResponse);

        RestClientTask actualTask = await MakeTryTaskRequest(httpMethod, url, request);

        actualTask.Headers.TryGetValues(keyOne, out IEnumerable<string>? firstValueSet);
        actualTask.Headers.TryGetValues(keyTwo, out IEnumerable<string>? secondValueSet);

        Assert.Equal(2, firstValueSet.Count());
        Assert.Single(secondValueSet);

        Assert.True(firstValueSet.Contains(valueOneA) && firstValueSet.Contains(valueOneB));
        Assert.Equal(valueTwo, secondValueSet.FirstOrDefault());
    }
}
