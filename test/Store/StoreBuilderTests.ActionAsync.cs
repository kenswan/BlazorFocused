using System;
using BlazorFocused.Test.Utility;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace BlazorFocused.Store.Test
{ 
    public partial class StoreBuilderTests
    {
        [Fact]
        public void ShouldRegisterAsyncActionByType()
        {
            storeBuilder.RegisterAction<TestActionAsync>();

            var services = storeBuilder.BuildServices();

            Assert.NotNull(services.GetRequiredService<TestActionAsync>());
        }

        [Fact]
        public void ShouldRegisterAsyncActionWithInputByType()
        {
            storeBuilder.RegisterAction<TestActionAsyncWithInput>();

            var services = storeBuilder.BuildServices();

            Assert.NotNull(services.GetRequiredService<TestActionAsyncWithInput>());
        }

        [Fact]
        public void ShouldRegisterAsyncActionByInstance()
        {
            var testAction = new TestActionAsync() { CheckedPropertyId = Guid.NewGuid().ToString() };

            storeBuilder.RegisterAction(testAction);

            var services = storeBuilder.BuildServices();

            var providerTestAction = services.GetRequiredService<TestActionAsync>();

            Assert.Equal(testAction.CheckedPropertyId, providerTestAction.CheckedPropertyId);
        }

        [Fact]
        public void ShouldRegisterAsyncActionWithInputByInstance()
        {
            var testAction = new TestActionAsyncWithInput() { CheckedPropertyId = Guid.NewGuid().ToString() };

            storeBuilder.RegisterAction(testAction);

            var services = storeBuilder.BuildServices();

            var providerTestAction = services.GetRequiredService<TestActionAsyncWithInput>();

            Assert.Equal(testAction.CheckedPropertyId, providerTestAction.CheckedPropertyId);
        }
    }
}
