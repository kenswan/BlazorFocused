// -------------------------------------------------------
// Copyright (c) Ken Swan All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using System.Text.Json;

namespace BlazorFocused;

public interface IApplicationClient
{
    HttpClient InternalClient { get; }

    IApplicationClient AddHeader(string key, string value);

    IApplicationClient AddParameter(string key, string value);

    IApplicationClient AddSerializationOptions(JsonSerializerOptions jsonSerializerOptions);

    Task<T> ExecuteAsync<T>(CancellationToken cancellationToken);

    Task<ApplicationResponse> ExecuteAsync(CancellationToken cancellationToken);

    Task<HttpResponseMessage> SendAsync(HttpRequestMessage httpRequestMessage, CancellationToken cancellationToken);

    /// <summary>
    /// Performs Http Request and returns message content as deserialized value or exception (does not throw)
    /// </summary>
    /// <typeparam name="T">Object type of response from http request</typeparam>
    /// <param name="cancellationToken">Token responsible for delivering stoppage of request</param>
    /// <returns>
    /// Http response attributes including body, status code, or any exceptions that occurred.
    /// See <see cref="RestClientResponse{T}"/>
    /// </returns>
    /// <remarks>
    /// If request is successful, value is returned.
    /// If request is not successful, exception is returned.
    /// Success is indicated by "IsValid" property.
    /// See <see cref="ApplicationResponse{T}"/>
    /// </remarks>
    Task<ApplicationResponse<T>> TryExecuteAsync<T>(CancellationToken cancellationToken);

    Task<ApplicationResponse> TryExecuteAsync(CancellationToken cancellationToken);
}
