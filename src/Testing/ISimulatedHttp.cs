using System.Net.Http;

namespace BlazorFocused.Testing
{
    /// <summary>
    /// Simulates <see cref="HttpClient"/> transactions for testing
    /// </summary>
    public interface ISimulatedHttp
    {
        /// <summary>
        /// Base address for inner <see cref="HttpClient"/>
        /// </summary>
        string BaseAddress { get; }

        /// <summary>
        /// Returns inner <see cref="HttpClient"/> for inspection
        /// </summary>
        /// <returns>Inner <see cref="HttpClient"/></returns>
        HttpClient Client();

        /// <summary>
        /// Begin setup for an expected request that will be used
        /// </summary>
        /// <param name="method"><see cref="HttpMethod"/> of expected request</param>
        /// <param name="url">Relative or full url expected request</param>
        /// <returns>Further configuration to handle simulated response object</returns>
        ISimulatedHttpSetup Setup(HttpMethod method, string url);

        /// <summary>
        /// Verify a request was made
        /// </summary>
        /// <param name="method">Expected request</param>
        /// <param name="url">Expected relative or full url</param>
        /// <exception cref="SimulatedHttpTestException">
        /// Exception will be thrown if expected request was not performed
        /// </exception>
        void VerifyWasCalled(HttpMethod method = default, string url = default);
    }
}
