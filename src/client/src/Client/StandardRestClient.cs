// -------------------------------------------------------
// Copyright (c) Ken Swan All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using Microsoft.Extensions.Logging;

namespace BlazorFocused.Client;

internal partial class StandardRestClient : BaseRestClient, IRestClient
{
    public StandardRestClient(HttpClient httpClient, ILogger logger)
        : base(httpClient, logger)
    { }

    public virtual void AddHeader(string key, string value, bool global = true) => httpClient.DefaultRequestHeaders.Add(key, value);

    /// <summary>
    /// Gets current instance of <see cref="HttpClient"/> being used for requests
    /// </summary>
    /// <returns>Current <see cref="HttpClient"/> from dependency injection</returns>
    /// <remarks>Primarily used for exposure/inspection in Test</remarks>
    internal HttpClient GetClient() => httpClient;

    public Task<T> DeleteAsync<T>(string relativeUrl) => ExtractOrThrowAsync<T>(HttpMethod.Delete, relativeUrl);

    public Task DeleteTaskAsync(string relativeUrl) => ExtractOrThrowAsync(HttpMethod.Delete, relativeUrl);

    public Task<T> GetAsync<T>(string relativeUrl) => ExtractOrThrowAsync<T>(HttpMethod.Get, relativeUrl);

    public Task<T> PatchAsync<T>(string relativeUrl, object data) => ExtractOrThrowAsync<T>(HttpMethod.Patch, relativeUrl, data);

    public Task PatchTaskAsync(string relativeUrl, object data) => ExtractOrThrowAsync(HttpMethod.Patch, relativeUrl, data);

    public Task<T> PostAsync<T>(string relativeUrl, object data) => ExtractOrThrowAsync<T>(HttpMethod.Post, relativeUrl, data);

    public Task PostTaskAsync(string relativeUrl, object data) => ExtractOrThrowAsync(HttpMethod.Post, relativeUrl, data);

    public Task<T> PutAsync<T>(string relativeUrl, object data) => ExtractOrThrowAsync<T>(HttpMethod.Put, relativeUrl, data);

    public Task PutTaskAsync(string relativeUrl, object data) => ExtractOrThrowAsync(HttpMethod.Put, relativeUrl, data);

    public void UpdateHttpClient(Action<HttpClient> updateHttpClient) => updateHttpClient(httpClient);
}
