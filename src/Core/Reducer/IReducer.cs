using System;
namespace BlazorRx.Core.Reducer
{
    public interface IReducer<TInput, TOutput> where TInput : class
    {
        TOutput Reduce(TInput input);
    }
}
