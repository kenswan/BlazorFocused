using BlazorFocused.Utility;
using Bogus;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace BlazorFocused.Client
{
    public partial class ServiceCollectionExtensionsTests
    {
        [Fact]
        public void ShouldProvideHttpClientBaseAddress()
        {
            var url = new Faker().Internet.Url();
            ServiceCollection services = new();
            services.AddEmptyConfiguration();
            services.AddRestClient(url);

            var serviceProvider = services.BuildServiceProvider();
            var restClient = serviceProvider.GetRequiredService<IRestClient>();

            restClient.UpdateHttpClient(client =>
            {
                Assert.Equal(url, client.BaseAddress.OriginalString);
            });
        }

        [Fact]
        public void ShouldConfigureHttpClientProperties()
        {
            var url = new Faker().Internet.Url();
            var headerKey = "X-PORT-NUMBER";
            var headerValue = new Faker().Internet.Port().ToString();
            ServiceCollection services = new();
            services.AddEmptyConfiguration();

            services.AddRestClient(client =>
            {
                client.BaseAddress = new Uri(url);
                client.DefaultRequestHeaders.Add(headerKey, headerValue);
            });

            var serviceProvider = services.BuildServiceProvider();
            var restClient = serviceProvider.GetRequiredService<IRestClient>();

            restClient.UpdateHttpClient(client =>
            {
                Assert.True(client.DefaultRequestHeaders.TryGetValues(headerKey, out IEnumerable<string> actualValues));
                Assert.Single(actualValues);
                Assert.Equal(headerValue, actualValues.FirstOrDefault());
                Assert.Equal(url, client.BaseAddress.OriginalString);
            });
        }

        [Fact]
        public void ShouldConfigureOAuthRestClient()
        {
            var url = new Faker().Internet.Url();
            ServiceCollection services = new();
            services.AddEmptyConfiguration();
            services.AddOAuthRestClient(url);

            var serviceProvider = services.BuildServiceProvider();
            var oAuthRestClient = serviceProvider.GetRequiredService<IOAuthRestClient>();

            oAuthRestClient.UpdateHttpClient(client =>
            {
                Assert.Equal(url, client.BaseAddress.OriginalString);
            });
        }

        [Fact]
        public void ShouldConfigureOAuthHttpProperties()
        {
            var url = new Faker().Internet.Url();
            var headerKey = "X-PORT-NUMBER";
            var headerValue = new Faker().Internet.Port().ToString();
            ServiceCollection services = new();
            services.AddEmptyConfiguration();

            services.AddOAuthRestClient(client =>
            {
                client.BaseAddress = new Uri(url);
                client.DefaultRequestHeaders.Add(headerKey, headerValue);
            });

            var serviceProvider = services.BuildServiceProvider();
            var oAuthRestClient = serviceProvider.GetRequiredService<IOAuthRestClient>();

            oAuthRestClient.UpdateHttpClient(client =>
            {
                Assert.True(client.DefaultRequestHeaders.TryGetValues(headerKey, out IEnumerable<string> actualValues));
                Assert.Single(actualValues);
                Assert.Equal(headerValue, actualValues.FirstOrDefault());
                Assert.Equal(url, client.BaseAddress.OriginalString);
            });
        }

        [Fact]
        public void ShouldConfigureOAuthTokenProperties()
        {
            ServiceCollection services = new();
            services.AddEmptyConfiguration();
            services.AddOAuthRestClient(new Faker().Internet.Url());
            var serviceProvider = services.BuildServiceProvider();
            var expectedScheme = "Bearer";
            var expectedToken = "TestToken";

            using var firstScope = serviceProvider.CreateScope();
            var oAuthRestClient = firstScope.ServiceProvider.GetRequiredService<IOAuthRestClient>();
            oAuthRestClient.AddAuthorization(expectedScheme, expectedToken);

            using var secondScope = serviceProvider.CreateScope();
            var oAuthToken = secondScope.ServiceProvider.GetRequiredService<OAuthToken>();
            Assert.Equal(expectedScheme, oAuthToken.Scheme);
            Assert.Equal(expectedToken, oAuthToken.Token);
        }
    }
}
