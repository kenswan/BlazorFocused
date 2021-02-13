using System;
namespace BlazorFocused.Testing
{
    public class FocusedTestException : Exception
    {
        public FocusedTestException(string message)
            : base(message) { }
    }
}
