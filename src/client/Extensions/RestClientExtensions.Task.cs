namespace BlazorFocused.Extensions;

public static partial class RestClientExtensions
{
    /// <summary>
    /// Performs DELETE http request
    /// </summary>
    /// <param name="restClient"><see cref="IRestClient"/> implementation being extended</param>
    /// <param name="relativeUrl">Relative url for http request</param>
    /// <returns>
    /// <see cref="RestClientTask"/> with http response attributes including status code or any exceptions.
    /// <para>
    /// NOTE: Use this when not expecting to receive a deserialized response object.
    /// To received deserialized response object, use extension
    /// <see cref="TryDeleteAsync{T}(IRestClient, string)"/>
    /// </para>
    /// </returns>
    public static async Task<RestClientTask> TryDeleteTaskAsync(
        this IRestClient restClient, string relativeUrl) =>
            await GetRestClientTask(restClient, HttpMethod.Delete, relativeUrl);

    /// <summary>
    /// Performs PATCH http request
    /// </summary>
    /// <param name="restClient"><see cref="IRestClient"/> implementation being extended</param>
    /// <param name="relativeUrl">Relative url for http request</param>
    /// <param name="data">Http request object body</param>
    /// <returns>
    /// <see cref="RestClientTask"/> with http response attributes including status code or any exceptions.
    /// <para>
    /// NOTE: Use this when not expecting to receive a deserialized response object.
    /// To received deserialized response object, use extension
    /// <see cref="TryPatchAsync{T}(IRestClient, string, object)"/>
    /// </para>
    /// </returns>
    public static async Task<RestClientTask> TryPatchTaskAsync(this IRestClient restClient, string relativeUrl, object data) =>
        await GetRestClientTask(restClient, HttpMethod.Patch, relativeUrl, data);


    /// <summary>
    /// Performs POST http request
    /// </summary>
    /// <param name="restClient"><see cref="IRestClient"/> implementation being extended</param>
    /// <param name="relativeUrl">Relative url for http request</param>
    /// <param name="data">Http request object body</param>
    /// <returns>
    /// <see cref="RestClientTask"/> with http response attributes including status code or any exceptions.
    /// <para>
    /// NOTE: Use this when not expecting to receive a deserialized response object.
    /// To received deserialized response object, use extension
    /// <see cref="TryPostAsync{T}(IRestClient, string, object)"/>
    /// </para>
    /// </returns>
    public static async Task<RestClientTask> TryPostTaskAsync(this IRestClient restClient, string relativeUrl, object data) =>
        await GetRestClientTask(restClient, HttpMethod.Post, relativeUrl, data);

    /// <summary>
    /// Performs PUT http request
    /// </summary>
    /// <param name="restClient"><see cref="IRestClient"/> implementation being extended</param>
    /// <param name="relativeUrl">Relative url for http request</param>
    /// <param name="data">Http request object body</param>
    /// <returns>
    /// <see cref="RestClientTask"/> with http response attributes including status code or any exceptions.
    /// <para>
    /// NOTE: Use this when not expecting to receive a deserialized response object.
    /// To received deserialized response object, use extension
    /// <see cref="TryPutAsync{T}(IRestClient, string, object)"/>
    /// </para>
    /// </returns>
    public static async Task<RestClientTask> TryPutTaskAsync(this IRestClient restClient, string relativeUrl, object data) =>
       await GetRestClientTask(restClient, HttpMethod.Put, relativeUrl, data);
}
