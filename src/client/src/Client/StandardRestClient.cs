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

    public virtual void AddHeader(string key, string value, bool global = true)
    {
        httpClient.DefaultRequestHeaders.Add(key, value);
    }

    /// <summary>
    /// Gets current instance of <see cref="HttpClient"/> being used for requests
    /// </summary>
    /// <returns>Current <see cref="HttpClient"/> from dependency injection</returns>
    /// <remarks>Primarily used for exposure/inspection in Test</remarks>
    internal HttpClient GetClient()
    {
        return httpClient;
    }

    public Task<T> DeleteAsync<T>(string relativeUrl)
    {
        return ExtractOrThrowAsync<T>(HttpMethod.Delete, relativeUrl);
    }

    public Task DeleteTaskAsync(string relativeUrl)
    {
        return ExtractOrThrowAsync(HttpMethod.Delete, relativeUrl);
    }

    public Task<T> GetAsync<T>(string relativeUrl)
    {
        return ExtractOrThrowAsync<T>(HttpMethod.Get, relativeUrl);
    }

    public Task<T> PatchAsync<T>(string relativeUrl, object data)
    {
        return ExtractOrThrowAsync<T>(HttpMethod.Patch, relativeUrl, data);
    }

    public Task PatchTaskAsync(string relativeUrl, object data)
    {
        return ExtractOrThrowAsync(HttpMethod.Patch, relativeUrl, data);
    }

    public Task<T> PostAsync<T>(string relativeUrl, object data)
    {
        return ExtractOrThrowAsync<T>(HttpMethod.Post, relativeUrl, data);
    }

    public Task PostTaskAsync(string relativeUrl, object data)
    {
        return ExtractOrThrowAsync(HttpMethod.Post, relativeUrl, data);
    }

    public Task<T> PutAsync<T>(string relativeUrl, object data)
    {
        return ExtractOrThrowAsync<T>(HttpMethod.Put, relativeUrl, data);
    }

    public Task PutTaskAsync(string relativeUrl, object data)
    {
        return ExtractOrThrowAsync(HttpMethod.Put, relativeUrl, data);
    }

    public void UpdateHttpClient(Action<HttpClient> updateHttpClient)
    {
        updateHttpClient(httpClient);
    }
}
