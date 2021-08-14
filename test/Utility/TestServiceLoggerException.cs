using System;

namespace BlazorFocused.Test.Utility
{
    public class TestServiceLoggerException : Exception
    {
        public TestServiceLoggerException(string message) : base(message) { }
    }
}
