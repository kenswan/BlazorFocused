﻿// -------------------------------------------------------
// Copyright (c) Ken Swan All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

namespace BlazorFocused.Tools.Http;

/// <summary>
/// Exception given when request was not verified with
/// <see cref="ISimulatedHttp"/>
/// </summary>
internal class SimulatedHttpTestException : Exception
{
    /// <summary>
    /// Initializes a new instance of <see cref="SimulatedHttpTestException"/>
    /// with exception message
    /// </summary>
    /// <param name="message"></param>
    public SimulatedHttpTestException(string message)
        : base(message) { }
}
