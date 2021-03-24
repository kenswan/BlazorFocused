using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("BlazorFocused.Test")]

namespace BlazorFocused.Client
{
    /// <summary>
    /// Strongly types url request parameter configuration
    /// </summary>
    internal interface IUrlParameterBuilder
    {
        IUrlParameterBuilder AddParameter(string name, string value);

        IUrlParameterBuilder AddParameter(string parameter);

        string Build();
    }
}
