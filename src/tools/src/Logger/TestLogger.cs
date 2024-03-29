﻿// -------------------------------------------------------
// Copyright (c) Ken Swan All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using Microsoft.Extensions.Logging;

namespace BlazorFocused.Tools.Logger;

/// <inheritdoc cref="ITestLogger{T}"/>
internal partial class TestLogger<T> : ITestLogger<T>
{
    private readonly List<TestLog> logs;
    private readonly Action<LogLevel, string, Exception> logAction;
    private readonly List<LogScope> scopes;

    /// <summary>
    /// Initializes a new instance of Mock Logger used to capture/test logs
    /// </summary>
    /// <param name="logAction">Optional: execute this action every time a log occurs.
    /// This is useful when using test output helpers (i.e. XUnit ITestOutputHelper)
    /// or event tracking 
    /// </param>
    public TestLogger(Action<LogLevel, string, Exception> logAction = null)
    {
        this.logAction = logAction;
        logs = new();
        scopes = new();
    }

    public IDisposable BeginScope<TState>(TState state)
    {
        var scope = new LogScope(state);
        scopes.Add(scope);
        return scope;
    }

    public bool IsEnabled(LogLevel logLevel) => true;

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
    {
        if (logAction is not null)
        {
            logAction(logLevel, state?.ToString(), exception);
        }

        logs.Add(new TestLog { LogLevel = logLevel, Exception = exception, Message = state.ToString() });
    }

    private class TestLog
    {
        public LogLevel LogLevel { get; set; }

        public string Message { get; set; }

        public Exception Exception { get; set; }
    }

    private class LogScope : IDisposable
    {
        private object scope;

        public LogScope(object scope)
        {
            this.scope = scope;
        }

        public void Dispose()
        {
            if (scope is not null)
            {
                scope = default;
            }

            GC.SuppressFinalize(this);
        }
    }
}
