using System;

namespace BlazorFocused.Testing
{
    public class SimulatedHttpTestException : Exception
    {
        public SimulatedHttpTestException(string message)
            : base(message) { }
    }
}
