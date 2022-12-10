// -------------------------------------------------------
// Copyright (c) Ken Swan All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using BlazorFocused.Client;
using BlazorFocused.Tools.Extensions;
using Bogus;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Xunit.Abstractions;

namespace BlazorFocused;

public partial class ServiceCollectionExtensionsTests
{
    private readonly ITestOutputHelper testOutpuHelper;

    public ServiceCollectionExtensionsTests(ITestOutputHelper testOutpuHelper)
    {
        this.testOutpuHelper = testOutpuHelper;
    }

    [Fact]
    public void ShouldProvideHttpClientBaseAddress()
    {
        var url = new Faker().Internet.Url();
        ServiceCollection services = new();
        services.AddConfiguration();
        services.AddRestClient(url);

        using ServiceProvider serviceProvider = services.BuildServiceProvider();
        IRestClient restClient = serviceProvider.GetRequiredService<IRestClient>();
        using HttpClient httpClient = (restClient as RestClient).GetClient();

        Assert.Equal(url, httpClient.BaseAddress.OriginalString);
    }

    [Fact]
    public void ShouldConfigureHttpClientProperties()
    {
        var url = new Faker().Internet.Url();
        var headerKey = "X-PORT-NUMBER";
        var headerValue = new Faker().Internet.Port().ToString();
        ServiceCollection services = new();
        services.AddConfiguration();

        services.AddRestClient(client =>
        {
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Add(headerKey, headerValue);
        });

        using ServiceProvider serviceProvider = services.BuildServiceProvider();
        IRestClient restClient = serviceProvider.GetRequiredService<IRestClient>();
        using HttpClient httpClient = (restClient as RestClient).GetClient();

        Assert.True(httpClient.DefaultRequestHeaders.TryGetValues(headerKey, out IEnumerable<string> actualValues));
        Assert.Single(actualValues);
        Assert.Equal(headerValue, actualValues.FirstOrDefault());
        Assert.Equal(url, httpClient.BaseAddress.OriginalString);
    }

    [Fact]
    public void ShouldConfigureOAuthRestClient()
    {
        var url = new Faker().Internet.Url();
        ServiceCollection services = new();
        services.AddConfiguration();
        services.AddOAuthRestClient(url);

        using ServiceProvider serviceProvider = services.BuildServiceProvider();
        IOAuthRestClient oAuthRestClient = serviceProvider.GetRequiredService<IOAuthRestClient>();
        using HttpClient httpClient = (oAuthRestClient as OAuthRestClient).GetClient();

        Assert.Equal(url, httpClient.BaseAddress.OriginalString);
    }

    [Fact]
    public void ShouldConfigureOAuthHttpProperties()
    {
        var url = new Faker().Internet.Url();
        var headerKey = "X-PORT-NUMBER";
        var headerValue = new Faker().Internet.Port().ToString();
        ServiceCollection services = new();
        services.AddConfiguration();

        services.AddOAuthRestClient(client =>
        {
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Add(headerKey, headerValue);
        });

        using ServiceProvider serviceProvider = services.BuildServiceProvider();
        IOAuthRestClient oAuthRestClient = serviceProvider.GetRequiredService<IOAuthRestClient>();
        using HttpClient httpClient = (oAuthRestClient as OAuthRestClient).GetClient();

        Assert.True(httpClient.DefaultRequestHeaders.TryGetValues(headerKey, out IEnumerable<string> actualValues));
        Assert.Single(actualValues);
        Assert.Equal(headerValue, actualValues.FirstOrDefault());
        Assert.Equal(url, httpClient.BaseAddress.OriginalString);
    }

    [Fact]
    public void ShouldConfigureOAuthTokenProperties()
    {
        ServiceCollection services = new();
        services.AddConfiguration();
        services.AddOAuthRestClient(new Faker().Internet.Url());
        using ServiceProvider serviceProvider = services.BuildServiceProvider();
        var expectedScheme = "Bearer";
        var expectedToken = "TestToken";

        using IServiceScope firstScope = serviceProvider.CreateScope();
        IOAuthRestClient oAuthRestClient = firstScope.ServiceProvider.GetRequiredService<IOAuthRestClient>();
        oAuthRestClient.AddAuthorization(expectedScheme, expectedToken);

        using IServiceScope secondScope = serviceProvider.CreateScope();
        IOAuthToken oAuthToken = secondScope.ServiceProvider.GetRequiredService<IOAuthToken>();
        Assert.Equal(expectedScheme, oAuthToken.Scheme);
        Assert.Equal(expectedToken, oAuthToken.Token);
    }
}
