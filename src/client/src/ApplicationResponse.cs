// -------------------------------------------------------
// Copyright (c) Ken Swan All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using System.Net;
using System.Net.Http.Headers;
using System.Text;

namespace BlazorFocused;

/// <summary>
/// Gives result of an http request using <see cref="IApplicationClient"/>
/// </summary>
public class ApplicationResponse
{
    /// <summary>
    /// Exception that occurred during request
    /// </summary>
    /// <remarks>This value will be "null" if exception did not occur</remarks>
    public Exception Exception { get; init; }

    /// <summary>
    /// Response headers received from executing request
    /// </summary>
    public HttpResponseHeaders Headers { get; init; }

    /// <summary>
    /// Identifies whether request was successful or failed
    /// </summary>
    public virtual bool IsSuccess => HasSuccessStatusCode();

    /// <summary>
    /// Status of http request
    /// </summary>
    /// <remarks>This may be null if url passed in is not valid relative or absolute path</remarks>
    public HttpStatusCode? StatusCode { get; init; }

    /// <summary>
    /// Stream representation of content returned from http request
    /// </summary>
    public Stream Content { get; init; }

    /// <summary>
    /// Receive string representation of content returned from http request
    /// </summary>
    public string GetContent()
    {
        using var reader = new StreamReader(Content, Encoding.UTF8);

        return reader.ReadToEnd();
    }

    protected bool HasSuccessStatusCode() => StatusCode.HasValue && new HttpResponseMessage(StatusCode.Value).IsSuccessStatusCode;
}

/// <summary>
/// Gives result of an http request using <see cref="IApplicationClient"/>
/// </summary>
/// <typeparam name="T">
/// Value expected from <see cref="HttpResponseMessage.Content"/> deserialization
/// </typeparam>
public class ApplicationResponse<T> : ApplicationResponse
{
    /// <summary>
    /// Identifies whether request was successful or failed
    /// </summary>
    public override bool IsSuccess => HasSuccessStatusCode() && Value is not null;

    /// <summary>
    /// Value expected from <see cref="HttpResponseMessage.Content"/> deserialization
    /// </summary>
    public T Value { get; init; }
}
