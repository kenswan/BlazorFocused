// -------------------------------------------------------
// Copyright (c) Ken Swan All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using BlazorFocused.Tools;
using Bogus;
using Xunit;

namespace BlazorFocused.Client;
public class RestClientHeaderHandlerTests
{
    private readonly ISimulatedHttp simulatedHttp;
    private readonly ITestLogger<RestClientHeaderHandler> testLogger;
    private readonly string baseAddress = new Faker().Internet.Url();

    public RestClientHeaderHandlerTests()
    {
        simulatedHttp = ToolsBuilder.CreateSimulatedHttp(baseAddress);
        testLogger = ToolsBuilder.CreateTestLogger<RestClientHeaderHandler>(); ;
    }

    [Fact]
    public async Task ShouldAddHeadersToRequest()
    {
        var keyOne = "X-IPv6-Address";
        var valueOneA = new Faker().Internet.Ipv6();
        var valueOneB = new Faker().Internet.Ipv6();
        var keyTwo = "X-IPv4-Address";
        var valueTwo = new Faker().Internet.Ip();
        var relativePath = new Faker().Internet.UrlRootedPath();

        var restClientRequestHeaders = new RestClientRequestHeaders();

        restClientRequestHeaders.AddHeader(keyOne, valueOneA);
        restClientRequestHeaders.AddHeader(keyOne, valueOneB);
        restClientRequestHeaders.AddHeader(keyTwo, valueTwo);

        simulatedHttp.SetupGET(relativePath)
            .ReturnsAsync(System.Net.HttpStatusCode.OK, string.Empty);

        var restClientHeaderHandler = new RestClientHeaderHandler(restClientRequestHeaders, testLogger)
        {
            InnerHandler = simulatedHttp.DelegatingHandler
        };

        using var httpClient = new HttpClient(restClientHeaderHandler)
        {
            BaseAddress = new Uri(baseAddress)
        };

        HttpResponseMessage httpResponseMessage = await httpClient.GetAsync(relativePath);

        httpResponseMessage.EnsureSuccessStatusCode();

        IEnumerable<string> firstValueSet = simulatedHttp.GetRequestHeaderValues(HttpMethod.Get, relativePath, keyOne);
        IEnumerable<string> secondValueSet = simulatedHttp.GetRequestHeaderValues(HttpMethod.Get, relativePath, keyTwo);

        Assert.Equal(2, firstValueSet.Count());
        Assert.Single(secondValueSet);

        Assert.True(firstValueSet.Contains(valueOneA) && firstValueSet.Contains(valueOneB));
        Assert.Equal(valueTwo, secondValueSet.FirstOrDefault());
    }
}
