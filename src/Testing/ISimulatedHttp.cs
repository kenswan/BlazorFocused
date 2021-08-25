using System.Net.Http;

namespace BlazorFocused.Testing
{
    /// <summary>
    /// Simulates <see cref="HttpClient"/> transactions for testing
    /// and providing mock data
    /// </summary>
    public interface ISimulatedHttp
    {
        /// <summary>
        /// Handler that can be passed into <see cref="HttpClient"/> for
        /// making simulated requests
        /// </summary>
        DelegatingHandler DelegatingHandler { get; }

        /// <summary>
        /// Returns <see cref="HttpClient"/> to perform requests
        /// </summary>
        HttpClient HttpClient { get; }

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
