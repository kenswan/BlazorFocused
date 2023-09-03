// -------------------------------------------------------
// Copyright (c) Ken Swan All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using BlazorFocused.Tools;
using Bogus;
using FluentAssertions;
using Xunit;

namespace BlazorFocused.Client;

public class OAuthRestClientTests
{
    private readonly IOAuthRestClient oAuthRestClient;
    private readonly ISimulatedHttp simulatedHttp;
    private readonly ITestLogger<OAuthRestClient> testLogger;
    private readonly IOAuthToken oAuthToken;

    public OAuthRestClientTests()
    {
        oAuthToken = new OAuthToken();
        simulatedHttp = ToolsBuilder.CreateSimulatedHttp();
        testLogger = ToolsBuilder.CreateTestLogger<OAuthRestClient>();

        oAuthRestClient =
            new OAuthRestClient(
                oAuthToken,
                simulatedHttp.HttpClient,
                default,
                new RestClientRequestHeaders(),
                testLogger);
    }

    [Fact]
    public void ShouldAddAuthorization()
    {
        string scheme = "Bearer";
        string token = GetRandomToken();
        string expectedAuthorization = $"{scheme} {token}";

        oAuthRestClient.AddAuthorization(scheme, token);

        oAuthRestClient.RetrieveAuthorization().Should().BeEquivalentTo(expectedAuthorization);
        Assert.True(oAuthRestClient.HasAuthorization());
    }

    [Fact]
    public void ShouldClearAuthorization()
    {
        string scheme = "Bearer";
        string token = GetRandomToken();
        string addedAuthorization = $"{scheme} {token}";
        string clearedAuthorization = string.Empty;

        oAuthRestClient.AddAuthorization(scheme, token);

        Assert.Equal(addedAuthorization, oAuthRestClient.RetrieveAuthorization());
        Assert.True(oAuthRestClient.HasAuthorization());

        oAuthRestClient.ClearAuthorization();

        Assert.Equal(clearedAuthorization, oAuthRestClient.RetrieveAuthorization());
        Assert.False(oAuthRestClient.HasAuthorization());
    }

    [Fact]
    public void ShouldDetectAuthorization()
    {
        string scheme = "Bearer";
        string token = GetRandomToken();

        oAuthRestClient.AddAuthorization(scheme, token);

        Assert.True(oAuthRestClient.HasAuthorization());

        oAuthRestClient.ClearAuthorization();

        Assert.False(oAuthRestClient.HasAuthorization());
    }

    [Fact]
    public void ShouldReturnAuthorization()
    {
        string scheme = "Bearer";
        string token = GetRandomToken();
        string expectedAuthorization = $"{scheme} {token}";

        oAuthRestClient.AddAuthorization(scheme, token);

        Assert.Equal(expectedAuthorization, oAuthRestClient.RetrieveAuthorization());
    }

    private static string GetRandomToken() => new Faker().Random.AlphaNumeric(new Faker().Random.Int(10, 20));
}
