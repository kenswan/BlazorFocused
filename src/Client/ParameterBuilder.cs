using System;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("BlazorFocused.Test")]

namespace BlazorFocused.Client
{
    internal class ParameterBuilder : IParameterBuilder
    {
        public string GetParameterString(object[] parameters)
        {
            throw new NotImplementedException();
        }
    }
}
