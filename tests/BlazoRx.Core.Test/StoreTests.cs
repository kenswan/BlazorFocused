using Moq;
using System;
using Xunit;

namespace BlazoRx.Core.Test
{
    public class StoreTests : IDisposable
    {
        private readonly IStore<SimpleClass> underTest;
        private IDisposable storeSubscription;

        public StoreTests()
        {
            underTest = new Store<SimpleClass>();
        }

        public void Dispose()
        {
            if (storeSubscription != null)
                storeSubscription.Dispose();
        }

        [Fact]
        public void ShouldInitializeStateWithNull()
        {
            var test = new SimpleClass();

            storeSubscription = underTest.Connect().Subscribe((observer) =>
            {
                test = observer;
            });

            Assert.Null(test);
        }

        [Fact]
        public void ShouldUpdateValueOnDispatch()
        {
            var expectedResult = new SimpleClass() { FieldOne = "ThisIsATest1", FieldTwo = "ThisIsATest2" };

            var actualSubscribedResult = new SimpleClass() { };

            storeSubscription = underTest.Connect().Subscribe((observer) =>
            {
                actualSubscribedResult = observer;
            });

            underTest.Dispatch((currentState) => expectedResult);

            Assert.Equal(expectedResult.FieldOne, actualSubscribedResult.FieldOne);
            Assert.Equal(expectedResult.FieldTwo, actualSubscribedResult.FieldTwo);
        }

        [Fact]
        public void ShouldSendFilteredValueWhenUpdated()
        {
            var fieldOne = "Test1";
            var fieldTwo = "Test2";
            var fieldThree = "Test3";
            var fieldFour = "Test4";

            var expectedFieldA = $"{fieldOne} {fieldTwo}";
            var expectedFieldB = $"{fieldThree} {fieldFour}";

            var simpleClass = new SimpleClass() { FieldOne = fieldOne, FieldTwo = fieldTwo, FieldThree = fieldThree, FieldFour = fieldFour };
            var minorClass = new MinorClass();

            storeSubscription = underTest.Connect<MinorClass>((currentState) =>
            {
                return new MinorClass()
                {
                    FieldA = $"{currentState?.FieldOne} {currentState?.FieldTwo}",
                    FieldB = $"{currentState?.FieldThree} {currentState?.FieldFour}"
                };
            }).Reduce().Subscribe(filteredState => minorClass = filteredState);

            underTest.Dispatch(currentState => simpleClass);

            Assert.Equal(expectedFieldA, minorClass.FieldA);
            Assert.Equal(expectedFieldB, minorClass.FieldB);
        }

        [Fact]
        public void ShouldProvideStateSnapshot()
        {
            var minorClass = new MinorClass() { FieldA = "Test1", FieldB = "Test2" };

            var initializedStateStore = new Store<MinorClass>(minorClass);

            Assert.Equal(minorClass.FieldA, initializedStateStore.GetCurrentState().FieldA);
            Assert.Equal(minorClass.FieldB, initializedStateStore.GetCurrentState().FieldB);
        }

        private class SimpleClass
        {
            public string FieldOne { get; set; }

            public string FieldTwo { get; set; }

            public string FieldThree { get; set; }

            public string FieldFour { get; set; }
        }

        private class MinorClass
        {
            public string FieldA { get; set; }

            public string FieldB { get; set; }
        }
    }
}
