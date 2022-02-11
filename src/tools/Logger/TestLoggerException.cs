namespace BlazorFocused.Tools.Logger
{
    /// <summary>
    /// Exception returned when log entry from <see cref="ITestLogger{T}"/>
    /// was not verified
    /// </summary>
    internal class TestLoggerException : Exception
    {
        /// <summary>
        /// Initializes a new instance of <see cref="ITestLogger{T}"/>
        /// with exception message
        /// </summary>
        /// <param name="message"></param>
        public TestLoggerException(string message) : base(message) { }

        /// <summary>
        /// Initializes a new instance of <see cref="ITestLogger{T}"/>
        /// with exception message and inner exception
        /// </summary>
        /// <param name="message">Exception message</param>
        /// <param name="exception">Inner exception</param>
        public TestLoggerException(string message, Exception exception) : base(message, exception) { }
    }
}
