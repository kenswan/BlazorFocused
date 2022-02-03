namespace BlazorFocused.Tools.Logger
{
    /// <summary>
    /// Exception returned when log entry from <see cref="IMockLogger{T}"/>
    /// was not verified
    /// </summary>
    public class MockLoggerException : Exception
    {
        /// <summary>
        /// Initializes a new instance of <see cref="IMockLogger{T}"/>
        /// with exception message
        /// </summary>
        /// <param name="message"></param>
        public MockLoggerException(string message) : base(message) { }

        /// <summary>
        /// Initializes a new instance of <see cref="IMockLogger{T}"/>
        /// with exception message and inner exception
        /// </summary>
        /// <param name="message">Exception message</param>
        /// <param name="exception">Inner exception</param>
        public MockLoggerException(string message, Exception exception) : base(message, exception) { }
    }
}
