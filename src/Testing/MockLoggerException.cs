using System;

namespace BlazorFocused.Testing
{
    public class MockLoggerException : Exception
    {
        public MockLoggerException(string message) : base(message) { }

        public MockLoggerException(string message, Exception exception) : base(message, exception) { }
    }
}
