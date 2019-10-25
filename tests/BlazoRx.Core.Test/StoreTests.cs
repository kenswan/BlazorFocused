using System;
using BlazoRx.Core;
using Xunit;

namespace BlazoRx.Core.Test
{
    public class StoreTests
    {
        private IStore<SimpleClass> underTest;

        public StoreTests()
        {
            underTest = new Store<SimpleClass>();
        }

        [Fact]
        public void ShouldInitializeStateWithNull()
        {
            var test = new SimpleClass();

            var subscription = underTest.Connect().Subscribe((observer) =>
            {
                test = observer;

                Assert.Null(test);
            });
        }

        [Fact]
        public void ShouldUpdateValue()
        {
            var expectedResult = new SimpleClass() { FieldOne = "Test1" };

            var actualResult = new SimpleClass();

            var subscription = underTest.Connect().Subscribe((observer) =>
            {
                actualResult = observer;
            });

            underTest.Update(expectedResult);

            Assert.Equal(expectedResult.FieldOne, actualResult.FieldOne);
        }

        private class SimpleClass
        {
            public string FieldOne { get; set; }

            public string FieldTwo { get; set; }

            public string FieldThree { get; set; }
        }
    }
}
