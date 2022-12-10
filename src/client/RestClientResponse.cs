// -------------------------------------------------------
// Copyright (c) Ken Swan All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

namespace BlazorFocused;

/// <summary>
/// Gives result of an http request using <see cref="IRestClient"/>
/// </summary>
/// <typeparam name="T">
/// Value expected from <see cref="HttpResponseMessage.Content"/> deserialization
/// </typeparam>
public class RestClientResponse<T> : RestClientTask
{
    /// <summary>
    /// Identifies whether request was successful or failed
    /// </summary>
    public override bool IsSuccess => HasSuccessStatusCode() && Value is not null;

    /// <summary>
    /// Value expected from <see cref="HttpResponseMessage.Content"/> deserialization
    /// </summary>
    public T Value { get; set; }
}
