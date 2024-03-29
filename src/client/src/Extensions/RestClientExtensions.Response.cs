﻿// -------------------------------------------------------
// Copyright (c) Ken Swan All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

namespace BlazorFocused.Extensions;

public static partial class RestClientExtensions
{
    /// <summary>
    /// Performs DELETE http request
    /// </summary>
    /// <typeparam name="T">Object type of response from http request</typeparam>
    /// <param name="restClient"><see cref="IRestClient"/> implementation being extended</param>
    /// <param name="relativeUrl">Relative url for http request</param>
    /// <returns>
    /// Http response attributes including value, status code, or any exceptions.
    /// See <see cref="RestClientResponse{T}"/>
    /// </returns>
    /// <remarks>
    /// If request is successful, value is returned.
    /// If request is not successful, exception is returned.
    /// Success is indicated by "IsValid" property.
    /// See <see cref="RestClientResponse{T}"/>
    /// </remarks>
    public static Task<RestClientResponse<T>> TryDeleteAsync<T>(
        this IRestClient restClient, string relativeUrl) => restClient.SendAsync<T>(HttpMethod.Delete, relativeUrl);

    /// <summary>
    /// Performs GET http request
    /// </summary>
    /// <typeparam name="T">Object type of response from http request</typeparam>
    /// <param name="restClient"><see cref="IRestClient"/> implementation being extended</param>
    /// <param name="relativeUrl">Relative url for http request</param>
    /// <returns>
    /// Http response attributes including body, status code, or any exceptions that occurred.
    /// See <see cref="RestClientResponse{T}"/>
    /// </returns>
    /// <remarks>
    /// If request is successful, value is returned.
    /// If request is not successful, exception is returned.
    /// Success is indicated by "IsValid" property.
    /// See <see cref="RestClientResponse{T}"/>
    /// </remarks>
    public static Task<RestClientResponse<T>> TryGetAsync<T>(this IRestClient restClient, string relativeUrl) => restClient.SendAsync<T>(HttpMethod.Get, relativeUrl);

    /// <summary>
    /// Performs PATCH http request
    /// </summary>
    /// <typeparam name="T">Object type of response from http request</typeparam>
    /// <param name="restClient"><see cref="IRestClient"/> implementation being extended</param>
    /// <param name="relativeUrl">Relative url for http request</param>
    /// <param name="data">Http request object body</param>
    /// <returns>
    /// Http response attributes including body, status code, or any exceptions that occurred.
    /// See <see cref="RestClientResponse{T}"/>
    /// </returns>
    /// <remarks>
    /// If request is successful, value is returned.
    /// If request is not successful, exception is returned.
    /// Success is indicated by "IsValid" property.
    /// See <see cref="RestClientResponse{T}"/>
    /// </remarks>
    public static Task<RestClientResponse<T>> TryPatchAsync<T>(this IRestClient restClient, string relativeUrl, object data) => restClient.SendAsync<T>(HttpMethod.Patch, relativeUrl, data);

    /// <summary>
    /// Performs POST http request
    /// </summary>
    /// <typeparam name="T">Object type of response from http request</typeparam>
    /// <param name="restClient"><see cref="IRestClient"/> implementation being extended</param>
    /// <param name="relativeUrl">Relative url for http request</param>
    /// <param name="data">Http request object body</param>
    /// <returns>
    /// Http response attributes including body, status code, or any exceptions that occurred.
    /// See <see cref="RestClientResponse{T}"/>
    /// </returns>
    /// <remarks>
    /// If request is successful, value is returned.
    /// If request is not successful, exception is returned.
    /// Success is indicated by "IsValid" property.
    /// See <see cref="RestClientResponse{T}"/>
    /// </remarks>
    public static Task<RestClientResponse<T>> TryPostAsync<T>(this IRestClient restClient, string relativeUrl, object data) => restClient.SendAsync<T>(HttpMethod.Post, relativeUrl, data);

    /// <summary>
    /// Performs PUT http request
    /// </summary>
    /// <typeparam name="T">Object type of response from http request</typeparam>
    /// <param name="restClient"><see cref="IRestClient"/> implementation being extended</param>
    /// <param name="relativeUrl">Relative url for http request</param>
    /// <param name="data">Http request object body</param>
    /// <returns>
    /// Http response attributes including body, status code, or any exceptions that occurred.
    /// See <see cref="RestClientResponse{T}"/>
    /// </returns>
    /// <remarks>
    /// If request is successful, value is returned.
    /// If request is not successful, exception is returned.
    /// Success is indicated by "IsValid" property.
    /// See <see cref="RestClientResponse{T}"/>
    /// </remarks>
    public static Task<RestClientResponse<T>> TryPutAsync<T>(this IRestClient restClient, string relativeUrl, object data) => restClient.SendAsync<T>(HttpMethod.Put, relativeUrl, data);
}
