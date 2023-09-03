// -------------------------------------------------------
// Copyright (c) Ken Swan All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using BlazorFocused.Tools.Extensions;
using Bogus;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace BlazorFocused.Client;

public class OAuthRestClientOptionsTests
{
    private readonly IServiceCollection serviceCollection;

    public OAuthRestClientOptionsTests()
    {
        serviceCollection = new ServiceCollection();
    }

    [Fact]
    public void ShouldUseOAuthRestClientOptionsIfPresent()
    {
        string restClientBaseAddress = new Faker().Internet.Url();
        string oAuthRestClientBaseAddress = new Faker().Internet.Url();

        var appSettings = new Dictionary<string, string>()
        {
            ["restclient:baseAddress"] = restClientBaseAddress,
            ["oauthrestclient:baseAddress"] = oAuthRestClientBaseAddress
        };

        serviceCollection.AddConfiguration(appSettings);
        serviceCollection.AddRestClient();
        serviceCollection.AddOAuthRestClient();

        using ServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();
        IRestClient restClient = serviceProvider.GetRequiredService<IRestClient>();
        IOAuthRestClient oAuthRestClient = serviceProvider.GetRequiredService<IOAuthRestClient>();

        string actualRestClientAddress =
            (restClient as RestClient).GetClient().BaseAddress.OriginalString;

        string actualOAuthAddress =
            (oAuthRestClient as OAuthRestClient).GetClient().BaseAddress.OriginalString;

        Assert.Equal(restClientBaseAddress, actualRestClientAddress);
        Assert.Equal(oAuthRestClientBaseAddress, actualOAuthAddress);
    }

    [Fact]
    public void ShouldUseRestClientOptionsIfOAuthRestClientOptionsNotPresent()
    {
        string restClientBaseAddress = new Faker().Internet.Url();

        var appSettings = new Dictionary<string, string>()
        {
            ["restclient:baseAddress"] = restClientBaseAddress,
        };

        serviceCollection.AddConfiguration(appSettings);
        serviceCollection.AddRestClient();
        serviceCollection.AddOAuthRestClient();

        using ServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();
        IRestClient restClient = serviceProvider.GetRequiredService<IRestClient>();
        IOAuthRestClient oAuthRestClient = serviceProvider.GetRequiredService<IOAuthRestClient>();

        string actualRestClientAddress =
            (restClient as RestClient).GetClient().BaseAddress.OriginalString;

        string actualOAuthAddress =
            (oAuthRestClient as OAuthRestClient).GetClient().BaseAddress.OriginalString;

        Assert.Equal(restClientBaseAddress, actualRestClientAddress);
        Assert.Equal(restClientBaseAddress, actualOAuthAddress);
    }
}
