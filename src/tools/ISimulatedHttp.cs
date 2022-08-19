using BlazorFocused.Tools.Http;

namespace BlazorFocused.Tools;

/// <summary>
/// Simulates <see cref="HttpClient"/> transactions for testing
/// and providing mock data
/// </summary>
public interface ISimulatedHttp
{
    /// <summary>
    /// Handler that can be passed into <see cref="System.Net.Http.HttpClient"/> for
    /// making simulated requests
    /// </summary>
    DelegatingHandler DelegatingHandler { get; }

    /// <summary>
    /// Returns <see cref="System.Net.Http.HttpClient"/> to perform requests
    /// </summary>
    HttpClient HttpClient { get; }

    /// <summary>
    /// Retrieve header values under specified key for a given request
    /// </summary>
    /// <param name="method">Http Method of which request was made</param>
    /// <param name="url">Url of which request was made</param>
    /// <param name="key">Header key for values to obtain</param>
    /// <returns></returns>
    IEnumerable<string> GetRequestHeaderValues(HttpMethod method, string url, string key);

    void AddResponseHeader(string key, string value);

    /// <summary>
    /// Begin setup for an expected DELETE request that will be used
    /// </summary>
    /// <param name="url">Relative or full url expected request</param>
    /// <returns>Further configuration to handle simulated response object</returns>
    ISimulatedHttpSetup SetupDELETE(string url);

    /// <summary>
    /// Begin setup for an expected GET request that will be used
    /// </summary>
    /// <param name="url">Relative or full url expected request</param>
    /// <returns>Further configuration to handle simulated response object</returns>
    ISimulatedHttpSetup SetupGET(string url);

    /// <summary>
    /// Begin setup for an expected PATCH request that will be used
    /// </summary>
    /// <param name="url">Relative or full url expected request</param>
    /// <param name="content">Request body of expected request (will not compare if null)</param>
    /// <returns>Further configuration to handle simulated response object</returns>
    ISimulatedHttpSetup SetupPATCH(string url, object content = null);

    /// <summary>
    /// Begin setup for an expected POST request that will be used
    /// </summary>
    /// <param name="url">Relative or full url expected request</param>
    /// <param name="content">Request body of expected request (will not compare if null)</param>
    /// <returns>Further configuration to handle simulated response object</returns>
    ISimulatedHttpSetup SetupPOST(string url, object content = null);

    /// <summary>
    /// Begin setup for an expected PUT request that will be used
    /// </summary>
    /// <param name="url">Relative or full url expected request</param>
    /// <param name="content">Request body of expected request (will not compare if null)</param>
    /// <returns>Further configuration to handle simulated response object</returns>
    ISimulatedHttpSetup SetupPUT(string url, object content = null);

    /// <summary>
    /// Verify a DELETE request was made
    /// </summary>
    /// <param name="url">Expected relative or full url</param>
    /// <exception cref="SimulatedHttpTestException">
    /// Exception will be thrown if expected request was not performed
    /// </exception>
    void VerifyDELETEWasCalled(string url = default);

    /// <summary>
    /// Verify a GET request was made
    /// </summary>
    /// <param name="url">Expected relative or full url</param>
    /// <exception cref="SimulatedHttpTestException">
    /// Exception will be thrown if expected request was not performed
    /// </exception>
    void VerifyGETWasCalled(string url = default);

    /// <summary>
    /// Verify PATCH request was made
    /// </summary>
    /// <param name="url">Expected relative or full url</param>
    /// <param name="content">Expected request object</param>
    /// <exception cref="SimulatedHttpTestException">
    /// Exception will be thrown if expected request was not performed
    /// </exception>
    void VerifyPATCHWasCalled(string url = default, object content = default);

    /// <summary>
    /// Verify POST request was made
    /// </summary>
    /// <param name="url">Expected relative or full url</param>
    /// <param name="content">Expected request object</param>
    /// <exception cref="SimulatedHttpTestException">
    /// Exception will be thrown if expected request was not performed
    /// </exception>
    void VerifyPOSTWasCalled(string url = default, object content = default);

    /// <summary>
    /// Verify PUT request was made
    /// </summary>
    /// <param name="url">Expected relative or full url</param>
    /// <param name="content">Expected request object</param>
    /// <exception cref="SimulatedHttpTestException">
    /// Exception will be thrown if expected request was not performed
    /// </exception>
    void VerifyPUTWasCalled(string url = default, object content = default);
}
