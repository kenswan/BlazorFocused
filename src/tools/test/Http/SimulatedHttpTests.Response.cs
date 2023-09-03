// -------------------------------------------------------
// Copyright (c) Ken Swan All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using BlazorFocused.Tools.Model;
using FluentAssertions;
using System.Net;
using System.Text.Json;
using Xunit;

namespace BlazorFocused.Tools.Http;

public partial class SimulatedHttpTests
{
    [Fact]
    public void ShouldSetBaseUri()
    {
        string actualBaseAddress = simulatedHttp.HttpClient.BaseAddress.OriginalString;

        Assert.Equal(baseAddress, actualBaseAddress);
    }

    [Theory]
    [MemberData(nameof(HttpData))]
    public async Task ShouldReturnRequestedResponse(
        HttpMethod httpMethod,
        HttpStatusCode httpStatusCode,
        string relativeRequestUrl,
        SimpleClass requestObject,
        SimpleClass responseObject)
    {
        ISimulatedHttpSetup setup = GetHttpSetup(httpMethod, relativeRequestUrl, requestObject);
        setup.ReturnsAsync(httpStatusCode, responseObject);

        using HttpClient client = simulatedHttp.HttpClient;

        HttpResponseMessage actualResponse =
            await MakeRequest(client, httpMethod, relativeRequestUrl, requestObject);

        string actualResponseString = await actualResponse.Content.ReadAsStringAsync();
        SimpleClass actualResponseObject = JsonSerializer.Deserialize<SimpleClass>(actualResponseString);

        Assert.Equal(httpStatusCode, actualResponse.StatusCode);
        actualResponseObject.Should().BeEquivalentTo(responseObject);
    }

    [Theory]
    [MemberData(nameof(HttpData))]
    public async Task ShouldReturnEmptyStringForNullResponse(
        HttpMethod httpMethod,
        HttpStatusCode httpStatusCode,
        string relativeRequestUrl,
        SimpleClass requestObject,
        SimpleClass responseObject)
    {
        responseObject = null;

        ISimulatedHttpSetup setup = GetHttpSetup(httpMethod, relativeRequestUrl, requestObject);
        setup.ReturnsAsync(httpStatusCode, responseObject);

        using HttpClient client = simulatedHttp.HttpClient;

        HttpResponseMessage actualResponse =
            await MakeRequest(client, httpMethod, relativeRequestUrl, requestObject);

        string actualResponseString = await actualResponse.Content.ReadAsStringAsync();

        Assert.Equal(httpStatusCode, actualResponse.StatusCode);
        Assert.Equal(string.Empty, actualResponseString);
    }
}
