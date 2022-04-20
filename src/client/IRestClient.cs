namespace BlazorFocused;

/// <summary>
/// Standardizes the handling of HTTP requests/responses within a given application.
/// </summary>
public interface IRestClient
{
    /// <summary>
    /// Adds header key/value pair to the Http Request Headers
    /// </summary>
    /// <param name="key">Header Key</param>
    /// <param name="value">Header Value</param>
    /// <param name="global">Use in all subsequent requests in other clients</param>
    void AddHeader(string key, string value, bool global = true);

    /// <summary>
    /// Performs DELETE http request
    /// </summary>
    /// <typeparam name="T">Object type of response from http request</typeparam>
    /// <param name="relativeUrl">Relative url for http request</param>
    /// <returns>Http response body of type <see cref="{T}"/></returns>
    /// <remarks>
    /// This method will throw an exception for non-success status codes 
    /// </remarks>
    Task<T> DeleteAsync<T>(string relativeUrl);

    /// <summary>
    /// Performs DELETE http request
    /// </summary>
    /// <param name="relativeUrl">Relative url for http request</param>
    /// <returns>Task for completion detection</returns>
    /// <remarks>
    /// This method will throw an exception for non-success status codes 
    /// </remarks>
    Task DeleteTaskAsync(string relativeUrl);

    /// <summary>
    /// Performs GET http request
    /// </summary>
    /// <typeparam name="T">Object type of response from http request</typeparam>
    /// <param name="relativeUrl">Relative url for http request</param>
    /// <returns>Http response body of type <see cref="{T}"/></returns>
    /// <remarks>
    /// This method will throw an exception for non-success status codes 
    /// </remarks>
    Task<T> GetAsync<T>(string relativeUrl);

    /// <summary>
    /// Performs PATCH http request
    /// </summary>
    /// <typeparam name="T">Object type of response from http request</typeparam>
    /// <param name="relativeUrl">Relative url for http request</param>
    /// <param name="data">Http request object body</param>
    /// <returns>Http response body of type <see cref="{T}"/></returns>
    /// <remarks>
    /// This method will throw an exception for non-success status codes
    /// </remarks>
    Task<T> PatchAsync<T>(string relativeUrl, object data);

    /// <summary>
    /// Performs PATCH http request
    /// </summary>
    /// <param name="relativeUrl">Relative url for http request</param>
    /// <param name="data">Http request object body</param>
    /// <returns>Task for completion detection</returns>
    /// <remarks>
    /// This method will throw an exception for non-success status codes 
    /// </remarks>
    Task PatchTaskAsync(string relativeUrl, object data);

    /// <summary>
    /// Performs POST http request
    /// </summary>
    /// <typeparam name="T">Object type of response from http request</typeparam>
    /// <param name="relativeUrl">Relative url for http request</param>
    /// <param name="data">Http request object body</param>
    /// <returns>Http response body of type <see cref="{T}"/></returns>
    /// <remarks>
    /// This method will throw an exception for non-success status codes
    /// </remarks>
    Task<T> PostAsync<T>(string relativeUrl, object data);

    /// <summary>
    /// Performs POST http request
    /// </summary>
    /// <param name="relativeUrl">Relative url for http request</param>
    /// <param name="data">Http request object body</param>
    /// <returns>Task for completion detection</returns>
    /// <remarks>
    /// This method will throw an exception for non-success status codes 
    /// </remarks>
    Task PostTaskAsync(string relativeUrl, object data);

    /// <summary>
    /// Performs PUT http request
    /// </summary>
    /// <typeparam name="T">Object type of response from http request</typeparam>
    /// <param name="relativeUrl">Relative url for http request</param>
    /// <param name="data">Http request object body</param>
    /// <returns>Http response body of type <see cref="{T}"/></returns>
    /// <remarks>
    /// This method will throw an exception for non-success status codes
    /// </remarks>
    Task<T> PutAsync<T>(string relativeUrl, object data);

    /// <summary>
    /// Performs PUT http request
    /// </summary>
    /// <param name="relativeUrl">Relative url for http request</param>
    /// <param name="data">Http request object body</param>
    /// <returns>Task for completion detection</returns>
    /// <remarks>
    /// This method will throw an exception for non-success status codes 
    /// </remarks>
    Task PutTaskAsync(string relativeUrl, object data);


    /// <summary>
    /// Updates HttpClient used within <see cref="IRestClient"/>
    /// </summary>
    /// <param name="updateHttpClient"></param>
    void UpdateHttpClient(Action<HttpClient> updateHttpClient);
}
