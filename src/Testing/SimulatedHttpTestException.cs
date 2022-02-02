namespace BlazorFocused.Testing
{
    /// <summary>
    /// Exception given when request was not verified with
    /// <see cref="ISimulatedHttp"/>
    /// </summary>
    public class SimulatedHttpTestException : Exception
    {
        /// <summary>
        /// Initializes a new instance of <see cref="SimulatedHttpTestException"/>
        /// with exception message
        /// </summary>
        /// <param name="message"></param>
        public SimulatedHttpTestException(string message)
            : base(message) { }
    }
}
