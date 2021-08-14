using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace BlazorFocused.Testing
{
    public partial class MockLogger<T> : ILogger<T>
    {
        private readonly List<MockLog> logs;
        private readonly Action<LogLevel, string, Exception> logAction;

        /// <summary>
        /// Mock Logger used to capture/test logs within a given class
        /// </summary>
        /// <param name="logAction">Optional: execute this action every time a log occurs.
        /// This is useful when using test output helpers (i.e. XUnit ITestOutputHelper)
        /// or event tracking 
        /// </param>
        public MockLogger(Action<LogLevel, string, Exception> logAction = null)
        {
            this.logAction = logAction;
            logs = new();
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            throw new NotImplementedException();
        }

        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (logAction is not null)
            {
                logAction(logLevel, state?.ToString(), exception);
            }

            logs.Add(new MockLog { LogLevel = logLevel, Exception = exception, Message = state.ToString() });
        }

        private class MockLog
        { 
            public LogLevel LogLevel { get; set; }

            public string Message { get; set; }

            public Exception Exception { get; set; }
        }
    }
}
