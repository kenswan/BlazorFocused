// -------------------------------------------------------
// Copyright (c) Ken Swan All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

namespace BlazorFocused;

/// <summary>
/// Base implementations of RestClient Operations
/// </summary>
public interface IBaseRestClient
{
    /// <summary>
    /// Sends http request for specified method and returns deserialized http response content
    /// </summary>
    /// <typeparam name="T">Object type of response from http request</typeparam>
    /// <param name="httpMethod">Http Method type</param>
    /// <param name="url">Absolute or relative url path</param>
    /// <param name="data">Http request body object</param>
    /// <returns>Http response body of type <see cref="{T}"/></returns>
    /// <remarks>Supports <see cref="class"/> objects, <see cref="MultipartFormDataContent"/>, and <see cref="FormUrlEncodedContent"/> in data payload</remarks>
    Task<RestClientResponse<T>> SendAsync<T>(HttpMethod httpMethod, string url, object data = null);

    /// <summary>
    /// Sends http request for specified method
    /// </summary>
    /// <param name="httpMethod">Http Method type</param>
    /// <param name="url">Absolute or relative url path</param>
    /// <param name="data">Http request body object</param>
    /// <returns>Task for completion detection</returns>
    Task<RestClientTask> SendAsync(HttpMethod httpMethod, string url, object data = null);

    /// <summary>
    /// Sends http request for specified method 
    /// </summary>
    /// <param name="httpRequestMessage">Message for Http Request operation</param>
    /// <returns>Response from Http Request</returns>
    Task<HttpResponseMessage> SendAsync(HttpRequestMessage httpRequestMessage);
}
