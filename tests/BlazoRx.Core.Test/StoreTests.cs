using System;
using BlazoRx.Core;
using Xunit;

namespace BlazoRx.Core.Test
{
    public class StoreTests
    {
        private IStore<MockClass> underTest;

        public StoreTests()
        {
            underTest = new Store<MockClass>();
        }

        [Fact]
        public void ShouldInitializeStateWithNull()
        {
            var test = new MockClass();

            var subscription = underTest.Connect().Subscribe((observer) =>
            {
                test = observer;

                Assert.Null(test);
            });
        }

        [Fact]
        public void ShouldUpdateValue()
        {
            var expectedResult = new MockClass() { FieldOne = "Test1" };

            var actualResult = new MockClass();

            var subscription = underTest.Connect().Subscribe((observer) =>
            {
                actualResult = observer;
            });

            underTest.Update(expectedResult);

            Assert.Equal(expectedResult.FieldOne, actualResult.FieldOne);
        }

        private class MockClass
        {
            public string FieldOne { get; set; }

            public string FieldTwo { get; set; }

            public string FieldThree { get; set; }
        }
    }
}
