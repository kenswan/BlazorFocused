// -------------------------------------------------------
// Copyright (c) Ken Swan All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

namespace BlazorFocused.Client;

/// <summary>
/// Exception returned with failed requests and operations within <see cref="IRestClient"/>
/// and <see cref="IOAuthRestClient"/>
/// </summary>
internal class RestClientException : Exception
{
    /// <summary>
    /// Initializes a new instance of <see cref="RestClientException"/>
    /// with an exception message
    /// </summary>
    /// <param name="message">Exception message</param>
    public RestClientException(string message) : base(message) { }

    /// <summary>
    /// Initializes a new instance of <see cref="RestClientException"/>
    /// with an exception message and inner exception
    /// </summary>
    /// <param name="message">Exception message</param>
    /// <param name="exception">Inner Exception</param>
    public RestClientException(string message, Exception exception) : base(message, exception) { }
}
