// -------------------------------------------------------
// Copyright (c) Ken Swan All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using BlazorFocused.Tools;
using Bogus;
using Xunit;

namespace BlazorFocused.Client;

public class RestClientAuthHandlerTests
{
    private readonly ISimulatedHttp simulatedHttp;
    private readonly ITestLogger<RestClientAuthHandler> testLogger;
    private readonly string baseAddress = new Faker().Internet.Url();

    public RestClientAuthHandlerTests()
    {
        simulatedHttp = ToolsBuilder.CreateSimulatedHttp(baseAddress);
        testLogger = ToolsBuilder.CreateTestLogger<RestClientAuthHandler>(); ;
    }

    [Fact]
    public async Task ShouldAddAuthTokenToRequest()
    {
        string scheme = "Bearer";
        string token = GetRandomToken();
        var oAuthToken = new OAuthToken(scheme, token);
        string relativePath = new Faker().Internet.UrlRootedPath();

        simulatedHttp.SetupGET(relativePath)
            .ReturnsAsync(System.Net.HttpStatusCode.OK, string.Empty);

        var restClientAuthHandler = new RestClientAuthHandler(oAuthToken, testLogger)
        {
            InnerHandler = simulatedHttp.DelegatingHandler
        };

        using var httpClient = new HttpClient(restClientAuthHandler)
        {
            BaseAddress = new Uri(baseAddress)
        };

        HttpResponseMessage httpResponseMessage = await httpClient.GetAsync(relativePath);

        httpResponseMessage.EnsureSuccessStatusCode();
        Assert.Equal(scheme, httpResponseMessage.RequestMessage.Headers.Authorization.Scheme);
        Assert.Equal(token, httpResponseMessage.RequestMessage.Headers.Authorization.Parameter);
    }

    [Fact]
    public async Task ShouldNotAddAuthTokenToRequestIfTokenEmpty()
    {
        var oAuthToken = new OAuthToken();
        string relativePath = new Faker().Internet.UrlRootedPath();

        simulatedHttp.SetupGET(relativePath)
            .ReturnsAsync(System.Net.HttpStatusCode.OK, string.Empty);

        var restClientAuthHandler = new RestClientAuthHandler(oAuthToken, testLogger)
        {
            InnerHandler = simulatedHttp.DelegatingHandler
        };

        using var httpClient = new HttpClient(restClientAuthHandler)
        {
            BaseAddress = new Uri(baseAddress)
        };

        HttpResponseMessage httpResponseMessage = await httpClient.GetAsync(relativePath);

        httpResponseMessage.EnsureSuccessStatusCode();
        Assert.Equal(default, httpResponseMessage.RequestMessage.Headers.Authorization);
    }

    private static string GetRandomToken() => new Faker().Random.AlphaNumeric(new Faker().Random.Int(10, 20));
}
