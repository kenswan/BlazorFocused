using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("BlazorFocused.Test")]

namespace BlazorFocused.Client
{
    internal interface IParameterBuilder
    {
        string GetParameterString(object[] parameters);
    }
}
