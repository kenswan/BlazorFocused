using BlazoRx.Core.Store;
using BlazoRx.Core.Test.Model;
using FluentAssertions;
using Xunit;

namespace BlazoRx.Core.Test.Store
{
    public class StoreTests
    {
        [Fact(DisplayName = "Should Store and Return Initial Value")]
        public void ShouldStoreAndReturnInitialValue()
        {
            SimpleClass inputSimpleClass = SimpleClassUtilities.GetRandomSimpleClass();
            SimpleClass expectedSimpleClass = inputSimpleClass;

            var store = new Store<SimpleClass>(inputSimpleClass);

            SimpleClass actualSimpleClass = store.GetCurrentState();

            actualSimpleClass.Should().BeEquivalentTo(expectedSimpleClass);
        }

        [Fact(DisplayName = "Should Return 'null' when initialized as null")]
        public void ShouldReturnNullWhenInitializedAsNull()
        {
            var store = new Store<SimpleClass>(null);

            SimpleClass actualSimpleClass = store.GetCurrentState();

            actualSimpleClass.Should().BeNull();
        }

        [Fact(DisplayName = "Should update state")]
        public void ShouldUpdateState()
        {
            var originalState = new SimpleClass { FieldOne = "Original" };
            var expectedState = new SimpleClass { FieldOne = "Expected" };

            var store = new Store<SimpleClass>(originalState);

            store.SetState(state => expectedState);

            var actualState = store.GetCurrentState();

            actualState.Should().BeEquivalentTo(expectedState);
        }
    }
}
