// -------------------------------------------------------
// Copyright (c) Ken Swan All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using System.Net;
using System.Net.Http.Headers;

namespace BlazorFocused;

/// <summary>
/// Gives result of an http request using <see cref="IRestClient"/>
/// </summary>
public class RestClientTask
{
    /// <summary>
    /// Exception that occurred during request
    /// </summary>
    /// <remarks>This value will be "null" if exception did not occur</remarks>
    public Exception Exception { get; internal set; }

    /// <summary>
    /// Response headers received from executing request
    /// </summary>
    public HttpResponseHeaders Headers { get; internal set; }

    /// <summary>
    /// Identifies whether request was successful or failed
    /// </summary>
    public virtual bool IsSuccess => HasSuccessStatusCode();

    /// <summary>
    /// Status of http request
    /// </summary>
    /// <remarks>This may be null if url passed in is not valid relative or absolute path</remarks>
    public HttpStatusCode? StatusCode { get; internal set; }

    /// <summary>
    /// String representation of content returned from http request
    /// </summary>
    public string Content { get; internal set; }

    protected bool HasSuccessStatusCode()
    {
        return StatusCode.HasValue && new HttpResponseMessage(StatusCode.Value).IsSuccessStatusCode;
    }
}
