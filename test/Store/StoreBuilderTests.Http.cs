using System;
using System.Net.Http;
using BlazorFocused.Test.Utility;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace BlazorFocused.Store.Test
{
    public partial class StoreBuilderTests
    {
        [Fact]
        public void ShouldRegisterHttpClient()
        {
            storeBuilder.RegisterHttpClient();

            var services = storeBuilder.BuildServices();

            Assert.NotNull(services.GetRequiredService<HttpClient>());
        }

        [Fact]
        public void ShouldRegisterHttpClientWithTypedClient()
        {
            storeBuilder.RegisterHttpClient<ITestHttpService, TestHttpService>();

            var services = storeBuilder.BuildServices();

            var testHttpService = services.GetRequiredService<ITestHttpService>();

            Assert.NotNull(testHttpService.HttpClient);
        }

        [Fact]
        public void ShouldRegisterHttpClientWithTypedClientWithConfiguration()
        {
            var baseAddress = new Uri("http://this-is-test.io");

            storeBuilder.RegisterHttpClient<ITestHttpService, TestHttpService>(client => client.BaseAddress = baseAddress );

            var services = storeBuilder.BuildServices();

            var testHttpService = services.GetRequiredService<ITestHttpService>();

            Assert.Equal(baseAddress, testHttpService.HttpClient.BaseAddress);
        }
    }
}
