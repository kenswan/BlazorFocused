// -------------------------------------------------------
// Copyright (c) Ken Swan All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using FluentAssertions;
using Microsoft.Extensions.Options;
using Xunit;

namespace BlazorFocused.Client;

public partial class RestClientTests
{
    [Fact]
    public void ShouldConfigureHttpClientWhenOptionsPresent()
    {
        var address = "https://blazorfocused.net";
        var expectedBaseAddress = new Uri(address);

        var expectedRequestHeaders = new Dictionary<string, string[]>()
        {
            ["Accept"] = new string[] { "application/json" },
        };

        var restClientOptions = new RestClientOptions
        {
            BaseAddress = address,
            MaxResponseContentBufferSize = 600000,
            Timeout = 500000,
            DefaultRequestHeaders = new Dictionary<string, string>()
            {
                ["Accept"] = "application/json",
            }
        };

        using HttpClient httpClient = new RestClient(
            new HttpClient(), Options.Create(restClientOptions), default, testLogger).GetClient();

        Assert.Equal(expectedBaseAddress, httpClient.BaseAddress);

        Assert.Equal(restClientOptions.MaxResponseContentBufferSize,
            httpClient.MaxResponseContentBufferSize);

        Assert.Equal(restClientOptions.Timeout, httpClient.Timeout.TotalMilliseconds);

        Assert.Single(httpClient.DefaultRequestHeaders);

        httpClient.DefaultRequestHeaders.Should().BeEquivalentTo(expectedRequestHeaders);
    }
}
