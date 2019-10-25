using System;
using System.Collections.Generic;
using System.Text;

namespace BlazoRx.Core
{
    public interface IReducer<TState, TOutput>
    {
        TOutput Value(TState state);
    }
}
