using BlazorFocused.Model;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace BlazorFocused.Store
{
    public partial class StoreTests
    {
        protected readonly ServiceCollection serviceCollection;

        public StoreTests()
        {
            serviceCollection = new();
        }

        [Fact(DisplayName = "Should Store and Return Initial Value")]
        public void ShouldStoreAndReturnInitialValue()
        {
            SimpleClass inputSimpleClass = SimpleClassUtilities.GetRandomSimpleClass();
            SimpleClass expectedSimpleClass = inputSimpleClass;
            var serviceProvider = new ServiceCollection().BuildServiceProvider();
            var store = new Store<SimpleClass>(inputSimpleClass, serviceProvider);

            SimpleClass actualSimpleClass = store.GetState();

            actualSimpleClass.Should().BeEquivalentTo(expectedSimpleClass);
        }

        [Fact(DisplayName = "Should Return 'null' when initialized as null")]
        public void ShouldReturnNullWhenInitializedAsNull()
        {
            var serviceProvider = new ServiceCollection().BuildServiceProvider();
            var store = new Store<SimpleClass>(null, serviceProvider);

            SimpleClass actualSimpleClass = store.GetState();

            actualSimpleClass.Should().BeNull();
        }
    }
}
