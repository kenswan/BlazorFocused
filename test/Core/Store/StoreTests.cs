using System;
using BlazoRx.Core.Store;
using BlazoRx.Core.Test.Model;
using Bogus;
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
    }
}
