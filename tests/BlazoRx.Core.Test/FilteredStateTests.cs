using System;
using System.Reactive.Subjects;
using Xunit;

namespace BlazoRx.Core.Test
{
    public class FilteredStateTests
    {
        [Fact]
        public void ShouldReduceOriginalState()
        {
            var fieldOne = "First Name";
            var fieldTwo = "Second Name";
            var expectedValue = fieldOne + fieldTwo;

            var stateSubject = new BehaviorSubject<OriginalClass>(new OriginalClass());

            var filteredState = new FilteredState<OriginalClass, string>(stateSubject, (state) => state.FirstField + state.SecondField);

            var actualValue = "";

            IDisposable filteredSubscription = filteredState.Reduce().Subscribe((filtered) => actualValue = filtered);

            stateSubject.OnNext(new OriginalClass() { FirstField = fieldOne, SecondField = fieldTwo });

            Assert.Equal(expectedValue, actualValue);

            stateSubject.Dispose();
        }

        public class OriginalClass
        {
            public string FirstField { get; set; }

            public string SecondField { get; set; }
        }
    }
}
