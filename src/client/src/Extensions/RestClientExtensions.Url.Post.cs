// -------------------------------------------------------
// Copyright (c) Ken Swan All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

namespace BlazorFocused.Extensions;

public partial class RestClientExtensions
{
    /// <summary>
    /// Performs POST http request
    /// </summary>
    /// <typeparam name="T">Object type of response from http request</typeparam>
    /// <param name="restClient"><see cref="IRestClient"/> implementation being extended</param>
    /// <param name="urlBuilder">
    /// <see cref="IRestClientUrlBuilder"/> used to construct relative or absolute url for request
    /// </param>
    /// <param name="data">Http request object body</param>
    /// <returns>Http response body of type <see cref="{T}"/></returns>
    /// <remarks>
    /// Rules/Details on <see cref="IRestClient.PostAsync{T}(string, object)"/> apply
    /// </remarks>
    public static Task<T> PostAsync<T>(
        this IRestClient restClient, Action<IRestClientUrlBuilder> urlBuilder, object data)
    {
        return restClient.PostAsync<T>(GetUrlString(urlBuilder), data);
    }

    /// <summary>
    /// Performs POST http request
    /// </summary>
    /// <param name="restClient"><see cref="IRestClient"/> implementation being extended</param>
    /// <param name="urlBuilder">
    /// <see cref="IRestClientUrlBuilder"/> used to construct relative or absolute url for request
    /// </param>
    /// <param name="data">Http request object body</param>
    /// <returns>Task for completion detection</returns>
    /// <remarks>
    /// Rules/Details on <see cref="IRestClient.PostAsync{T}(string, object)"/> apply
    /// </remarks>
    public static Task PostTaskAsync(
        this IRestClient restClient, Action<IRestClientUrlBuilder> urlBuilder, object data)
    {
        return restClient.PostTaskAsync(GetUrlString(urlBuilder), data);
    }

    /// <summary>
    /// Performs POST http request
    /// </summary>
    /// <typeparam name="T">Object type of response from http request</typeparam>
    /// <param name="restClient"><see cref="IRestClient"/> implementation being extended</param>
    /// <param name="urlBuilder">
    /// <see cref="IRestClientUrlBuilder"/> used to construct relative or absolute url for request
    /// </param>
    /// <param name="data">Http request object body</param>
    /// <returns>
    /// Http response attributes including value, status code, or any exceptions.
    /// See <see cref="RestClientResponse{T}"/>
    /// </returns>
    /// <remarks>
    /// Rules/Details on <see cref="TryPostAsync{T}(IRestClient, string, object)"/> apply
    /// </remarks>
    public static Task<RestClientResponse<T>> TryPostAsync<T>(
        this IRestClient restClient, Action<IRestClientUrlBuilder> urlBuilder, object data)
    {
        return GetRestClientResponse<T>(restClient, HttpMethod.Post, GetUrlString(urlBuilder), data);
    }

    /// <summary>
    /// Performs POST http request
    /// </summary>
    /// <param name="restClient"><see cref="IRestClient"/> implementation being extended</param>
    /// <param name="urlBuilder">
    /// <see cref="IRestClientUrlBuilder"/> used to construct relative or absolute url for request
    /// </param>
    /// <param name="data">Http request object body</param>
    /// <returns>
    /// <see cref="RestClientTask"/> with http response attributes including status code or any exceptions.
    /// </returns>
    /// <remarks>
    /// Rules/Details on <see cref="TryPostTaskAsync(IRestClient, string, object)"/> apply
    /// </remarks>
    public static Task<RestClientTask> TryPostTaskAsync(
        this IRestClient restClient, Action<IRestClientUrlBuilder> urlBuilder, object data)
    {
        return GetRestClientTask(restClient, HttpMethod.Post, GetUrlString(urlBuilder), data);
    }
}
