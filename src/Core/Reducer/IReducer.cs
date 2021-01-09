using System;
namespace BlazoRx.Core.Reducer
{
    public interface IReducer<TInput, TOutput> where TInput : class
    {
        TOutput Reduce(TInput input);
    }
}
