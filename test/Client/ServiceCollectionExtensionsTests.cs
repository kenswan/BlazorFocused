using Bogus;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace BlazorFocused.Client.Test
{
    public class ServiceCollectionExtensionsTests
    {
        [Fact]
        public void ShouldProvideHttpClientBaseAddress()
        {
            var url = new Faker().Internet.Url();
            ServiceCollection services = new();
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
    }
}
