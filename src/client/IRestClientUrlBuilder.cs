namespace BlazorFocused;

/// <summary>
/// URL building utility that implements full url path with parameters
/// </summary>
public interface IRestClientUrlBuilder
{
    /// <summary>
    /// Compiles original set path with parameters
    /// </summary>
    /// <returns>Full URL path</returns>
    string Build();

    /// <summary>
    /// Sets absolute or relative url for building
    /// </summary>
    /// <param name="absoluteOrRelativeUrl">Absolute or relative url parameters will be placed on</param>
    /// <returns>Continuation of <see cref="IRestClientUrlBuilder"/> to further build url</returns>
    IRestClientUrlBuilder SetPath(string absoluteOrRelativeUrl);

    /// <summary>
    /// Adds parameters to url
    /// </summary>
    /// <param name="key">Url parameter key</param>
    /// <param name="value">Url parameter value</param>
    /// <returns>Continuation of <see cref="IRestClientUrlBuilder"/> to further build url</returns>
    IRestClientUrlBuilder WithParameter(string key, string value);
}
