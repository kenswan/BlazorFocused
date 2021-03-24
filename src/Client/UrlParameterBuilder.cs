using System;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("BlazorFocused.Test")]

namespace BlazorFocused.Client
{
    /// <inheritdoc cref="IUrlParameterBuilder"/>
    internal class UrlParameterBuilder : IUrlParameterBuilder
    {
        public IUrlParameterBuilder AddParameter(string name, string value)
        {
            throw new NotImplementedException();
        }

        public string Build()
        {
            throw new NotImplementedException();
        }

        public string Build(string url)
        {
            throw new NotImplementedException();
        }
    }
}
