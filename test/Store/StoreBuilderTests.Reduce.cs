using System;
using BlazorFocused.Core.Test.Model;
using BlazorFocused.Core.Test.Utility;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace BlazorFocused.Store.Test
{
    public partial class StoreBuilderTests
    {
        [Fact]
        public void ShouldRegisterReducerByInstance()
        {
            var testReducer = new TestReducer() { CheckedPropertyId = Guid.NewGuid().ToString() };

            storeBuilder.RegisterReducer(testReducer);

            var services = storeBuilder.BuildServices();

            var providerReducer = (TestReducer)services.GetRequiredService<IReducer<SimpleClass, SimpleClassSubset>>();

            Assert.Equal(testReducer.CheckedPropertyId, providerReducer.CheckedPropertyId);
        }

        [Fact]
        public void ShouldRegisterReducerByType()
        {
            storeBuilder.RegisterReducer<TestReducer, SimpleClassSubset>();

            var services = storeBuilder.BuildServices();

            var providerReducer = services.GetRequiredService<IReducer<SimpleClass, SimpleClassSubset>>();

            Assert.NotNull(providerReducer);
        }
    }
}
