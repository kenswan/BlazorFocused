using System;
using BlazorFocused.Test.Utility;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace BlazorFocused.Store.Test
{
    public partial class StoreBuilderTests
    {
        [Fact]
        public void ShouldRegisterServiceByType()
        {
            storeBuilder.RegisterService<TestService>();

            var services = storeBuilder.BuildServices();

            Assert.NotNull(services.GetRequiredService<TestService>());
        }

        [Fact]
        public void ShouldRegisterServiceByInstance()
        {
            var testService = new TestService() { CheckedPropertyId = Guid.NewGuid().ToString() };

            storeBuilder.RegisterService(testService);

            var services = storeBuilder.BuildServices();

            var providerService = services.GetRequiredService<TestService>();

            Assert.Equal(testService.CheckedPropertyId, providerService.CheckedPropertyId);
        }

        [Fact]
        public void ShouldRegisterServiceWithInterface()
        {
            storeBuilder.RegisterService<ITestService, TestService>();

            var services = storeBuilder.BuildServices();

            var providerService = services.GetRequiredService<ITestService>();

            Assert.NotNull(providerService);
        }
    }
}
