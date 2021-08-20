using System;

namespace BlazorFocused.Utility
{
    public class TestServiceLoggerException : Exception
    {
        public TestServiceLoggerException(string message) : base(message) { }
    }
}
