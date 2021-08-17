using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Xunit;

namespace BlazorFocused.Client.Test
{
    public partial class ServiceCollectionExtensionsTests
    {
        [Fact]
        public void ShouldUseRestClientConfiguration()
        {
            var expectedRequestHeaders = new Dictionary<string, string[]>()
            {
                ["Accept"] = new string[] { "application/json" },
                ["Accept-Enconding"] = new string[] { "gzip" },
                ["Cache-Control"] = new string[] { "max-age=0" }
            };

            var appSettings = new Dictionary<string, string>()
            {
                ["restclient:baseAddress"] = "http://test.com",
                ["restclient:defaultRequestHeaders:Accept"] = "application/json",
                ["restclient:defaultRequestHeaders:Accept-Enconding"] = "gzip",
                ["restclient:defaultRequestHeaders:Cache-Control"] = "max-age=0",
                ["restclient:maxResponseContentBufferSize"] = "500000",
                ["restclient:timeout"] = "300000"
            };

            using var host = GetRestClientHost(appSettings);

            var restClient = host.Services.GetRequiredService<IRestClient>();

            HttpClient httpClient = default; 

            restClient.UpdateHttpClient(innerClient =>
            {
                httpClient = innerClient;
            });

            Assert.Equal("http://test.com", httpClient.BaseAddress.OriginalString);
            Assert.Equal(3, httpClient.DefaultRequestHeaders.Count());
            httpClient.DefaultRequestHeaders.Should().BeEquivalentTo(expectedRequestHeaders);
            Assert.Equal(500000, httpClient.MaxResponseContentBufferSize);
            Assert.Equal(300000, httpClient.Timeout.TotalMilliseconds);
        }

        [Fact]
        public void ShouldUseOAuthRestClientConfiguration()
        {
            var expectedRequestHeaders = new Dictionary<string, string[]>()
            {
                ["Accept"] = new string[] { "application/json" },
                ["Accept-Enconding"] = new string[] { "gzip" },
                ["Cache-Control"] = new string[] { "max-age=0" }
            };

            var appSettings = new Dictionary<string, string>()
            {
                ["restclient:baseAddress"] = "http://test.com",
                ["restclient:defaultRequestHeaders:Accept"] = "application/json",
                ["restclient:defaultRequestHeaders:Accept-Enconding"] = "gzip",
                ["restclient:defaultRequestHeaders:Cache-Control"] = "max-age=0",
                ["restclient:maxResponseContentBufferSize"] = "500000",
                ["restclient:timeout"] = "300000"
            };

            using var host = GetOAuthRestClientHost(appSettings);

            var oAuthRestClient = host.Services.GetRequiredService<IOAuthRestClient>();

            HttpClient httpClient = default;

            oAuthRestClient.UpdateHttpClient(innerClient =>
            {
                httpClient = innerClient;
            });

            Assert.Equal("http://test.com", httpClient.BaseAddress.OriginalString);
            Assert.Equal(3, httpClient.DefaultRequestHeaders.Count());
            httpClient.DefaultRequestHeaders.Should().BeEquivalentTo(expectedRequestHeaders);
            Assert.Equal(500000, httpClient.MaxResponseContentBufferSize);
            Assert.Equal(300000, httpClient.Timeout.TotalMilliseconds);
        }

        [Fact]
        public void ShouldNotConfigureRestClientWithoutConfiguration()
        {
            using var host = GetRestClientHost(new Dictionary<string, string>());

            var restClient = host.Services.GetRequiredService<IRestClient>();

            HttpClient httpClient = default;

            restClient.UpdateHttpClient(innerClient =>
            {
                httpClient = innerClient;
            });

            Assert.Null(httpClient.BaseAddress);
            Assert.Empty(httpClient.DefaultRequestHeaders);
            Assert.Equal(2147483647, httpClient.MaxResponseContentBufferSize); // Default value
            Assert.Equal(100000, httpClient.Timeout.TotalMilliseconds); // Default value
        }

        [Fact]
        public void ShouldNotConfigureOAuthRestClientWithoutConfiguration()
        {
            using var host = GetOAuthRestClientHost(new Dictionary<string, string>());

            var oAuthRestClient = host.Services.GetRequiredService<IOAuthRestClient>();

            HttpClient httpClient = default;

            oAuthRestClient.UpdateHttpClient(innerClient =>
            {
                httpClient = innerClient;
            });

            Assert.Null(httpClient.BaseAddress);
            Assert.Empty(httpClient.DefaultRequestHeaders);
            Assert.Equal(2147483647, httpClient.MaxResponseContentBufferSize); // Default value
            Assert.Equal(100000, httpClient.Timeout.TotalMilliseconds); // Default value
        }

        private IHost GetRestClientHost(Dictionary<string, string> appSettings) =>
            Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration(configBuilder =>
                {
                    configBuilder.AddInMemoryCollection(appSettings);
                })
                .ConfigureServices(services =>
                {
                    services.AddRestClient();
                })
                .Build();

        private IHost GetOAuthRestClientHost(Dictionary<string, string> appSettings) =>
            Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration(configBuilder =>
                {
                    configBuilder.AddInMemoryCollection(appSettings);
                })
                .ConfigureServices(services =>
                {
                    services.AddOAuthRestClient();
                })
                .Build();
    }
}
