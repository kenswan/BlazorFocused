// -------------------------------------------------------
// Copyright (c) Ken Swan All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

namespace BlazorFocused.Extensions;

public partial class RestClientExtensions
{
    /// <summary>
    /// Performs DELETE http request
    /// </summary>
    /// <typeparam name="T">Object type of response from http request</typeparam>
    /// <param name="restClient"><see cref="IRestClient"/> implementation being extended</param>
    /// <param name="urlBuilder">
    /// <see cref="IRestClientUrlBuilder"/> used to construct relative or absolute url for request
    /// </param>
    /// <returns>Http response body of type <see cref="{T}"/></returns>
    /// <remarks>
    /// Rules/Details on <see cref="IRestClient.DeleteAsync{T}(string)"/> apply
    /// </remarks>
    public static Task<T> DeleteAsync<T>(
        this IRestClient restClient, Action<IRestClientUrlBuilder> urlBuilder)
    {
        return restClient.DeleteAsync<T>(GetUrlString(urlBuilder));
    }

    /// <summary>
    /// Performs DELETE http request
    /// </summary>
    /// <param name="restClient"><see cref="IRestClient"/> implementation being extended</param>
    /// <param name="urlBuilder">
    /// <see cref="IRestClientUrlBuilder"/> used to construct relative or absolute url for request
    /// </param>
    /// <returns>Task for completion detection</returns>
    /// <remarks>
    /// Rules/Details on <see cref="IRestClient.DeleteTaskAsync(string)"/> apply
    /// </remarks>
    public static Task DeleteTaskAsync(
        this IRestClient restClient, Action<IRestClientUrlBuilder> urlBuilder)
    {
        return restClient.DeleteTaskAsync(GetUrlString(urlBuilder));
    }

    /// <summary>
    /// Performs DELETE http request
    /// </summary>
    /// <typeparam name="T">Object type of response from http request</typeparam>
    /// <param name="restClient"><see cref="IRestClient"/> implementation being extended</param>
    /// <param name="urlBuilder">
    /// <see cref="IRestClientUrlBuilder"/> used to construct relative or absolute url for request
    /// </param>
    /// <returns>
    /// Http response attributes including value, status code, or any exceptions.
    /// See <see cref="RestClientResponse{T}"/>
    /// </returns>
    /// <remarks>
    /// Rules/Details on <see cref="TryDeleteAsync{T}(IRestClient, string)"/> apply
    /// </remarks>
    public static Task<RestClientResponse<T>> TryDeleteAsync<T>(
        this IRestClient restClient, Action<IRestClientUrlBuilder> urlBuilder)
    {
        return restClient.SendAsync<T>(HttpMethod.Delete, GetUrlString(urlBuilder));
    }

    /// <summary>
    /// Performs DELETE http request
    /// </summary>
    /// <param name="restClient"><see cref="IRestClient"/> implementation being extended</param>
    /// <param name="urlBuilder">
    /// <see cref="IRestClientUrlBuilder"/> used to construct relative or absolute url for request
    /// </param>
    /// <returns>
    /// <see cref="RestClientTask"/> with http response attributes including status code or any exceptions.
    /// </returns>
    /// <remarks>
    /// Rules/Details on <see cref="TryDeleteAsync{T}(IRestClient, string)"/> apply
    /// </remarks>
    public static Task<RestClientTask> TryDeleteTaskAsync(
        this IRestClient restClient, Action<IRestClientUrlBuilder> urlBuilder)
    {
        return restClient.SendAsync(HttpMethod.Delete, GetUrlString(urlBuilder));
    }
}
