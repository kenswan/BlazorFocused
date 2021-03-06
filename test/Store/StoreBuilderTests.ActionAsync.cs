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
        public void ShouldRegisterAsyncActionByType()
        {
            storeBuilder.RegisterAsyncAction<TestActionAsync>();

            var services = storeBuilder.BuildServices();

            Assert.NotNull(services.GetRequiredService<TestActionAsync>());
        }

        [Fact]
        public void ShouldRegisterAsyncActionByInstance()
        {
            var testAction = new TestActionAsync() { CheckedPropertyId = Guid.NewGuid().ToString() };

            storeBuilder.RegisterAsyncAction(testAction);

            var services = storeBuilder.BuildServices();

            var providerTestAction = services.GetRequiredService<TestActionAsync>();

            Assert.Equal(testAction.CheckedPropertyId, providerTestAction.CheckedPropertyId);
        }
    }
}
