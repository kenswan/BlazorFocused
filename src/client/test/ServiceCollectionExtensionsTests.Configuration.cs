﻿// -------------------------------------------------------
// Copyright (c) Ken Swan All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using BlazorFocused.Client;
using BlazorFocused.Tools.Extensions;
using Bogus;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Xunit;

namespace BlazorFocused;

public partial class ServiceCollectionExtensionsTests
{
    [Fact]
    public void ShouldUseRestClientConfiguration()
    {
        string baseAddress = new Faker().Internet.Url();

        var expectedRequestHeaders = new Dictionary<string, string[]>()
        {
            ["Accept"] = new string[] { "application/json" },
            ["Accept-Enconding"] = new string[] { "gzip" },
            ["Cache-Control"] = new string[] { "max-age=0" }
        };

        var appSettings = new Dictionary<string, string>()
        {
            ["restclient:baseAddress"] = baseAddress,
            ["restclient:defaultRequestHeaders:Accept"] = "application/json",
            ["restclient:defaultRequestHeaders:Accept-Enconding"] = "gzip",
            ["restclient:defaultRequestHeaders:Cache-Control"] = "max-age=0",
            ["restclient:maxResponseContentBufferSize"] = "500000",
            ["restclient:timeout"] = "300000"
        };

        var serviceCollection = new ServiceCollection();
        serviceCollection.AddConfiguration(appSettings);
        serviceCollection.AddRestClient();

        using ServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();
        IRestClient restClient = serviceProvider.GetRequiredService<IRestClient>();
        using HttpClient httpClient = (restClient as RestClient).GetClient();

        Assert.Equal(baseAddress, httpClient.BaseAddress.OriginalString);
        Assert.Equal(3, httpClient.DefaultRequestHeaders.Count());
        httpClient.DefaultRequestHeaders.Should().BeEquivalentTo(expectedRequestHeaders);
        Assert.Equal(500000, httpClient.MaxResponseContentBufferSize);
        Assert.Equal(300000, httpClient.Timeout.TotalMilliseconds);
    }

    [Fact]
    public void ShouldUseRestClientConfigurationIfOAuthNotPresent()
    {
        string baseAddress = new Faker().Internet.Url();

        var expectedRequestHeaders = new Dictionary<string, string[]>()
        {
            ["Accept"] = new string[] { "application/json" },
            ["Accept-Enconding"] = new string[] { "gzip" },
            ["Cache-Control"] = new string[] { "max-age=0" }
        };

        var appSettings = new Dictionary<string, string>()
        {
            ["restclient:baseAddress"] = baseAddress,
            ["restclient:defaultRequestHeaders:Accept"] = "application/json",
            ["restclient:defaultRequestHeaders:Accept-Enconding"] = "gzip",
            ["restclient:defaultRequestHeaders:Cache-Control"] = "max-age=0",
            ["restclient:maxResponseContentBufferSize"] = "500000",
            ["restclient:timeout"] = "300000"
        };

        var serviceCollection = new ServiceCollection();
        serviceCollection.AddConfiguration(appSettings);
        serviceCollection.AddOAuthRestClient();

        using ServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();
        IOAuthRestClient oAuthRestClient = serviceProvider.GetRequiredService<IOAuthRestClient>();
        using HttpClient httpClient = (oAuthRestClient as OAuthRestClient).GetClient();

        Assert.Equal(baseAddress, httpClient.BaseAddress.OriginalString);
        Assert.Equal(3, httpClient.DefaultRequestHeaders.Count());
        httpClient.DefaultRequestHeaders.Should().BeEquivalentTo(expectedRequestHeaders);
        Assert.Equal(500000, httpClient.MaxResponseContentBufferSize);
        Assert.Equal(300000, httpClient.Timeout.TotalMilliseconds);
    }

    [Fact]
    public void ShouldNotConfigureRestClientWithoutConfiguration()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddConfiguration(new Dictionary<string, string>());
        serviceCollection.AddRestClient();

        using ServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();
        IRestClient restClient = serviceProvider.GetRequiredService<IRestClient>();
        using HttpClient httpClient = (restClient as RestClient).GetClient();

        Assert.Null(httpClient.BaseAddress);
        Assert.Empty(httpClient.DefaultRequestHeaders);
        Assert.Equal(2147483647, httpClient.MaxResponseContentBufferSize); // Default value
        Assert.Equal(100000, httpClient.Timeout.TotalMilliseconds); // Default value
    }

    [Fact]
    public void ShouldNotConfigureOAuthRestClientWithoutConfiguration()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddConfiguration(new Dictionary<string, string>());
        serviceCollection.AddOAuthRestClient();

        using ServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();
        IOAuthRestClient oAuthRestClient = serviceProvider.GetRequiredService<IOAuthRestClient>();
        using HttpClient httpClient = (oAuthRestClient as OAuthRestClient).GetClient();

        Assert.Null(httpClient.BaseAddress);
        Assert.Empty(httpClient.DefaultRequestHeaders);
        Assert.Equal(2147483647, httpClient.MaxResponseContentBufferSize); // Default value
        Assert.Equal(100000, httpClient.Timeout.TotalMilliseconds); // Default value
    }

    [Fact]
    public void ShouldThrowExceptionWithInvalidUri()
    {
        var appSettings = new Dictionary<string, string>()
        {
            ["restclient:baseAddress"] = "ThisIsTestingAFail",
        };

        var serviceCollection = new ServiceCollection();
        serviceCollection.AddConfiguration(appSettings);
        serviceCollection.AddRestClient();

        using ServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

        object? testCode()
        {
            return serviceProvider.GetRequiredService<IRestClient>();
        }

        _ = Assert.Throws<OptionsValidationException>(testCode);
    }
}
