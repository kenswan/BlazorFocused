using BlazorFocused.Extensions;
using BlazorFocused.Testing;
using Bogus;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using Xunit;

namespace BlazorFocused.Client
{
    public class OAuthRestClientOptionsTests : IDisposable
    {
        private readonly IServiceCollection serviceCollection;
        private readonly ISimulatedHttp simulatedHttp;

        public OAuthRestClientOptionsTests()
        {
            simulatedHttp = new SimulatedHttp();
            serviceCollection = new ServiceCollection();
        }

        [Fact]
        public void ShouldUseOAuthRestClientOptionsIfPresent()
        {
            var restClientBaseAddress = new Faker().Internet.Url();
            var oAuthRestClientBaseAddress = new Faker().Internet.Url();

            var appSettings = new Dictionary<string, string>()
            {
                ["restclient:baseAddress"] = restClientBaseAddress,
                ["oauthrestclient:baseAddress"] = oAuthRestClientBaseAddress
            };

            serviceCollection.AddConfiguration(appSettings);
            serviceCollection.AddRestClient();
            serviceCollection.AddOAuthRestClient();

            using var serviceProvider = serviceCollection.BuildServiceProvider();
            var restClient = serviceProvider.GetRequiredService<IRestClient>();
            var oAuthRestClient = serviceProvider.GetRequiredService<IOAuthRestClient>();

            var actualRestClientAddress =
                (restClient as RestClient).GetClient().BaseAddress.OriginalString;

            var actualOAuthAddress =
                (oAuthRestClient as OAuthRestClient).GetClient().BaseAddress.OriginalString;

            Assert.Equal(restClientBaseAddress, actualRestClientAddress);
            Assert.Equal(oAuthRestClientBaseAddress, actualOAuthAddress);
        }

        [Fact]
        public void ShouldUseRestClientOptionsIfOAuthRestClientOptionsNotPresent()
        {
            var restClientBaseAddress = new Faker().Internet.Url();

            var appSettings = new Dictionary<string, string>()
            {
                ["restclient:baseAddress"] = restClientBaseAddress,
            };

            serviceCollection.AddConfiguration(appSettings);
            serviceCollection.AddRestClient();
            serviceCollection.AddOAuthRestClient();

            using var serviceProvider = serviceCollection.BuildServiceProvider();
            var restClient = serviceProvider.GetRequiredService<IRestClient>();
            var oAuthRestClient = serviceProvider.GetRequiredService<IOAuthRestClient>();

            var actualRestClientAddress =
                (restClient as RestClient).GetClient().BaseAddress.OriginalString;

            var actualOAuthAddress =
                (oAuthRestClient as OAuthRestClient).GetClient().BaseAddress.OriginalString;

            Assert.Equal(restClientBaseAddress, actualRestClientAddress);
            Assert.Equal(restClientBaseAddress, actualOAuthAddress);
        }


        public void Dispose() => (simulatedHttp as SimulatedHttp).Dispose();
    }
}
