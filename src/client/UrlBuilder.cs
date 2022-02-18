using BlazorFocused.Client;

namespace BlazorFocused
{
    /// <summary>
    /// Provides implementation of <see cref="IRestClientUrlBuilder"/>
    /// </summary>
    public static class UrlBuilder
    {
        /// <summary>
        /// Provides implementation of <see cref="IRestClientUrlBuilder"/> with desginated
        /// absolute or relative url for building
        /// </summary>
        /// <param name="relativeOrAbsoluteUrl">Absolute or relative url parameters will be placed on</param>
        /// <returns>Continuation of <see cref="IRestClientUrlBuilder"/> to further build url</returns>
        public static IRestClientUrlBuilder SetPath(string relativeOrAbsoluteUrl) =>
            new RestClientUrlBuilder().SetPath(relativeOrAbsoluteUrl);
    }
}
