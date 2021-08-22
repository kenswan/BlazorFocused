using Microsoft.Extensions.Logging;
using System;

namespace BlazorFocused.Testing
{
    /// <summary>
    /// Mock Logger used to capture/test logs within a given class
    /// </summary>
    /// <typeparam name="T">Type of class utilizing logger</typeparam>
    public interface IMockLogger<T>
    {
        /// <summary>
        /// Verify logger was called at any level
        /// </summary>
        void VerifyWasCalled();

        /// <summary>
        /// Verify logger was called with a given level
        /// </summary>
        /// <param name="logLevel">Expected level of expected log entry</param>
        void VerifyWasCalledWith(LogLevel logLevel);

        /// <summary>
        /// Verify logger was called with a given level and message
        /// </summary>
        /// <param name="logLevel">Expected level of expected log entry</param>
        /// <param name="message">Expected message of expected log entry</param>
        void VerifyWasCalledWith(LogLevel logLevel, string message);

        /// <summary>
        /// Verify logger was called with a given level, exception, and message
        /// </summary>
        /// <typeparam name="TException">Type of Exception in log entry</typeparam>
        /// <param name="logLevel">Expected level of log entry</param>
        /// <param name="exception">Expected exception given within log entry</param>
        /// <param name="message">Expected message given with log entry</param>
        void VerifyWasCalledWith<TException>(LogLevel logLevel, TException exception, string message)
            where TException : Exception;

        /// <summary>
        /// Verify logger was called with a given level and exception
        /// </summary>
        /// <typeparam name="TException">Type of exception in log entry</typeparam>
        /// <param name="logLevel">Expected level of log entry</param>
        void VerifyWasCalledWithException<TException>(LogLevel logLevel)
            where TException : Exception;
    }
}
