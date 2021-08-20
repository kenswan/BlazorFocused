using FluentAssertions;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Xunit;

namespace BlazorFocused.Client
{
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

            var clientWithOptions = 
                new RestClient(new HttpClient(), Options.Create(restClientOptions), nullLogger);

            HttpClient httpClient = default;

            clientWithOptions.UpdateHttpClient(innerClient =>
            {
                httpClient = innerClient;
            });

            Assert.Equal(expectedBaseAddress, httpClient.BaseAddress);

            Assert.Equal(restClientOptions.MaxResponseContentBufferSize, 
                httpClient.MaxResponseContentBufferSize);

            Assert.Equal(restClientOptions.Timeout, httpClient.Timeout.TotalMilliseconds);

            Assert.Single(httpClient.DefaultRequestHeaders);

            httpClient.DefaultRequestHeaders.Should().BeEquivalentTo(expectedRequestHeaders);
        }
    }
}
