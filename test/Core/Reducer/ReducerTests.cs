using System;
using BlazoRx.Core.Reducer;
using BlazoRx.Core.Test.Model;
using FluentAssertions;
using Xunit;

namespace BlazoRx.Core.Test.Reducer
{
    public class ReducerTests
    {
        private IReducer<SimpleClass, SimpleClassSubset> reducer;

        [Fact(DisplayName = "Should reduce original value")]
        public void ShouldReduceOriginalValue()
        {
            reducer = new Reducer<SimpleClass, SimpleClassSubset>(SimpleReducerFunction);

            SimpleClass inputSimpleClass = SimpleClassUtilities.GetRandomSimpleClass();

            SimpleClassSubset expectedSimpleClassSubset = SimpleReducerFunction(inputSimpleClass);

            SimpleClassSubset actualSimpleClassSubset = reducer.Execute(inputSimpleClass);

            actualSimpleClassSubset.Should().BeEquivalentTo(expectedSimpleClassSubset);
        }

        private SimpleClassSubset SimpleReducerFunction(SimpleClass simpleClass)
        {
            return new SimpleClassSubset
            {
                FieldOne = simpleClass.FieldOne,
                FieldThree = simpleClass.FieldThree,
                FieldFive = simpleClass.FieldFive
            };
        }
    }
}
