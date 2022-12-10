// -------------------------------------------------------
// Copyright (c) Ken Swan All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using Microsoft.Extensions.Logging;

namespace BlazorFocused.Client;

internal abstract partial class BaseRestClient : AbstractRestClient, IRestClient
{
    public BaseRestClient(HttpClient httpClient, ILogger logger)
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

    public async Task<T> DeleteAsync<T>(string relativeUrl)
    {
        return await GetResponseValue<T>(HttpMethod.Delete, relativeUrl);
    }

    public async Task DeleteTaskAsync(string relativeUrl)
    {
        await SendAndTaskAsync(HttpMethod.Delete, relativeUrl);
    }

    public async Task<T> GetAsync<T>(string relativeUrl)
    {
        return await GetResponseValue<T>(HttpMethod.Get, relativeUrl);
    }

    public async Task<T> PatchAsync<T>(string relativeUrl, object data)
    {
        return await GetResponseValue<T>(HttpMethod.Patch, relativeUrl, data);
    }

    public async Task PatchTaskAsync(string relativeUrl, object data)
    {
        await SendAndTaskAsync(HttpMethod.Patch, relativeUrl, data);
    }

    public async Task<T> PostAsync<T>(string relativeUrl, object data)
    {
        return await GetResponseValue<T>(HttpMethod.Post, relativeUrl, data);
    }

    public async Task PostTaskAsync(string relativeUrl, object data)
    {
        await SendAndTaskAsync(HttpMethod.Post, relativeUrl, data);
    }

    public async Task<T> PutAsync<T>(string relativeUrl, object data)
    {
        return await GetResponseValue<T>(HttpMethod.Put, relativeUrl, data);
    }

    public async Task PutTaskAsync(string relativeUrl, object data)
    {
        await SendAndTaskAsync(HttpMethod.Put, relativeUrl, data);
    }

    public void UpdateHttpClient(Action<HttpClient> updateHttpClient)
    {
        updateHttpClient(httpClient);
    }
}
