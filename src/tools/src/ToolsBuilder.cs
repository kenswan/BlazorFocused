// -------------------------------------------------------
// Copyright (c) Ken Swan All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using BlazorFocused.Tools.Http;
using BlazorFocused.Tools.Logger;
using Microsoft.Extensions.Logging;

namespace BlazorFocused.Tools;

/// <summary>
/// Provides implementations of <see cref="ISimulatedHttp"/> and <see cref="ITestLogger{T}"/> for testing
/// </summary>
public static class ToolsBuilder
{
    /// <summary>
    /// Provides implementations of <see cref="ISimulatedHttp"/> for testing
    /// </summary>
    /// <param name="baseAddress"></param>
    /// <returns></returns>
    public static ISimulatedHttp CreateSimulatedHttp(string baseAddress = null) => baseAddress is not null ?
            new SimulatedHttp(baseAddress) : new SimulatedHttp();

    /// <summary>
    /// Provides implementations of <see cref="ITestLogger{T}"/> for testing
    /// </summary>
    /// <typeparam name="T">Logger class used in test (<see cref="ILogger{T}"/>)</typeparam>
    /// <param name="logAction">Optional: Execute this action every time a log occurs.
    /// This is useful when using test output helpers (i.e. XUnit ITestOutputHelper)
    /// or event tracking
    /// </param>
    /// <returns></returns>
    public static ITestLogger<T> CreateTestLogger<T>(
        Action<LogLevel, string, Exception> logAction = null) => new TestLogger<T>(logAction);
}
