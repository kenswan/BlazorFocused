namespace BlazorFocused
{
    /// <summary>
    /// Handles authorization functionality on top of <see cref="IRestClient"/>
    /// </summary>
    public interface IOAuthRestClient : IRestClient
    {
        /// <summary>
        /// Add authorization token for subsequent requests
        /// </summary>
        /// <param name="scheme">Authentication scheme</param>
        /// <param name="token">Authentication token</param>
        /// <remarks>
        /// This authorization information will be appended to all
        /// subsequent HTTP requests. To remove this information, you must 
        /// make another request with <see cref="AddAuthorization(string, string)"/>
        /// or use <see cref="ClearAuthorization"/>
        /// </remarks>
        void AddAuthorization(string scheme, string token);

        /// <summary>
        /// Clear previously stored authorization information.
        /// </summary>
        void ClearAuthorization();

        /// <summary>
        /// Determines if authorization information is stored
        /// </summary>
        /// <returns>"True" if information is stored and "False" if not found</returns>
        bool HasAuthorization();

        /// <summary>
        /// Returns current authorization information
        /// </summary>
        /// <returns>
        /// Current authorization infomration. Will return empty string
        /// if nothing is stored.
        /// </returns>
        /// <remarks>Example output: "Bearer your-token"</remarks>
        string RetrieveAuthorization();
    }
}
