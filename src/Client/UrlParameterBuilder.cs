using System;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("BlazorFocused.Test")]

namespace BlazorFocused.Client
{
    /// <inheritdoc cref="IParameterBuilder"/>
    internal class UrlParameterBuilder : IUrlParameterBuilder
    {
        public IUrlParameterBuilder AddParameter(string name, string value)
        {
            throw new NotImplementedException();
        }

        public IUrlParameterBuilder AddParameter(string parameter)
        {
            throw new NotImplementedException();
        }

        public string Build()
        {
            throw new NotImplementedException();
        }
    }
}
