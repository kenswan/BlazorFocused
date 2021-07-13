using System;
using BlazorFocused.Client;
using Bogus;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace BlazorFocused.Test.Client
{
    public partial class RestClientTests
    {
        [Fact]
        public void ShouldProvideHttpClientBaseAddress()
        {
            var url = new Faker().Internet.Url();
            ServiceCollection services = new();

            services.AddRestClient(url);
            var serviceProvider = services.BuildServiceProvider();
            var restClient = serviceProvider.GetRequiredService<IRestClient>();

            Assert.Equal(url, restClient.Settings.BaseAddress);
        }

        [Fact]
        public void ShouldUpdateHttpClientUrl()
        {
            var originalUrl = new Faker().Internet.Url();
            var newUrl = new Faker().Internet.Url();
            ServiceCollection services = new();

            services.AddRestClient(originalUrl);
            var serviceProvider = services.BuildServiceProvider();

            var restClient = serviceProvider.GetRequiredService<IRestClient>();

            restClient.UpdateHttpClient(httpClient => {
                httpClient.BaseAddress = new Uri(newUrl);
            });

            Assert.Equal(newUrl, restClient.Settings.BaseAddress);
        }
    }
}
