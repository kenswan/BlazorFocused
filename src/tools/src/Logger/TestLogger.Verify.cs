﻿// -------------------------------------------------------
// Copyright (c) Ken Swan All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using Microsoft.Extensions.Logging;

namespace BlazorFocused.Tools.Logger;

internal partial class TestLogger<T>
{
    public void VerifyWasCalled()
    {
        if (!logs.Any())
        {
            throw new TestLoggerException("Logger was not called");
        }
    }

    public void VerifyWasCalledWith(LogLevel logLevel)
    {
        if (!logs.Where(log => log.LogLevel == logLevel).Any())
        {
            throw new TestLoggerException($"Logger was not called with log level {logLevel}");
        }
    }

    public void VerifyWasCalledWith(LogLevel logLevel, string message)
    {
        if (!logs.Where(log => log.LogLevel == logLevel && log.Message.Contains(message)).Any())
        {
            throw new TestLoggerException(
                $"Logger was not called with log level {logLevel} message containing {message}");
        }
    }

    public void VerifyWasCalledWith<TException>(LogLevel logLevel, TException exception, string message)
        where TException : Exception
    {
        if (!logs.Where(log => log.LogLevel == logLevel &&
                               log.Message.Contains(message) &&
                               log.Exception.GetType() == exception.GetType() &&
                               log.Exception.Message == exception.Message).Any())
        {
            throw new TestLoggerException(
                $"Logger was not called with log level {logLevel}, {exception.GetType()}, and  message containing {message}");
        }
    }

    public void VerifyWasCalledWithException<TException>(LogLevel logLevel)
        where TException : Exception
    {
        if (!logs.Where(log => log.LogLevel == logLevel &&
                               log.Exception.GetType() == typeof(TException)).Any())
        {
            throw new TestLoggerException(
                $"Logger was not called with log level {logLevel} and, {typeof(TException)}");
        }
    }
}
