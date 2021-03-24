using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("BlazorFocused.Test")]

namespace BlazorFocused.Client
{
    /// <summary>
    /// Strongly types url request parameter configuration
    /// </summary>
    internal interface IUrlParameterBuilder
    {
        /// <summary>
        /// Add parameter to parameter collection
        /// </summary>
        /// <param name="name">Url variable name</param>
        /// <param name="value">Url variable value</param>
        /// <returns>Returns current instance of <see cref="IUrlParameterBuilder"/> to continue
        ///     parameter construction.
        /// </returns>
        IUrlParameterBuilder AddParameter(string name, string value);

        /// <summary>
        /// Constructs url parameters to append to url
        /// </summary>
        /// <returns>Url parameter string (not encoded)</returns>
        string Build();

        /// <summary>
        /// Returns original url with appended url variables
        /// </summary>
        /// <param name="url">Url without parameters</param>
        /// <returns>Url with parameters included</returns>
        string Build(string url);
    }
}
