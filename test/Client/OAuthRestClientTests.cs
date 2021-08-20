﻿using BlazorFocused.Testing;
using Bogus;
using FluentAssertions;
using System;
using System.Net.Http;
using Xunit;

namespace BlazorFocused.Client
{
    public class OAuthRestClientTests : IDisposable
    {
        private readonly IOAuthRestClient oAuthRestClient;
        private readonly ISimulatedHttp simulatedHttp;
        private readonly MockLogger<OAuthRestClient> mockLogger;
        private readonly HttpClient httpClient;
        private readonly OAuthToken oAuthToken;

        public OAuthRestClientTests()
        {
            oAuthToken = new();
            simulatedHttp = new SimulatedHttp();
            mockLogger = new();
            httpClient = simulatedHttp.Client();

            oAuthRestClient =
                new OAuthRestClient(oAuthToken, httpClient, default, mockLogger);
        }

        [Fact]
        public void ShouldAddAuthorization()
        {
            var scheme = "Bearer";
            var token = GetRandomToken();
            var expectedAuthorization = $"{scheme} {token}";

            oAuthRestClient.AddAuthorization(scheme, token);

            oAuthRestClient.RetrieveAuthorization().Should().BeEquivalentTo(expectedAuthorization);
            Assert.True(oAuthRestClient.HasAuthorization());
        }

        [Fact]
        public void ShouldClearAuthorization()
        {
            var scheme = "Bearer";
            var token = GetRandomToken();
            var addedAuthorization = $"{scheme} {token}";
            var clearedAuthorization = string.Empty;

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
            var scheme = "Bearer";
            var token = GetRandomToken();

            oAuthRestClient.AddAuthorization(scheme, token);

            Assert.True(oAuthRestClient.HasAuthorization());

            oAuthRestClient.ClearAuthorization();

            Assert.False(oAuthRestClient.HasAuthorization());
        }

        [Fact]
        public void ShouldReturnAuthorization()
        {
            var scheme = "Bearer";
            var token = GetRandomToken();
            var expectedAuthorization = $"{scheme} {token}";

            oAuthRestClient.AddAuthorization(scheme, token);

            Assert.Equal(expectedAuthorization, oAuthRestClient.RetrieveAuthorization());
        }

        private static string GetRandomToken() =>
            new Faker().Random.AlphaNumeric(new Faker().Random.Int(10, 20));

        public void Dispose() => (simulatedHttp as SimulatedHttp).Dispose();
    }
}
