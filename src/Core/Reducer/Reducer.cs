using System;
namespace BlazorRx.Core.Reducer
{
    public class Reducer<TInput, TOutput> : IReducer<TInput, TOutput> where TInput : class
    {
        private Func<TInput, TOutput> reduceInputFunction;

        public Reducer(Func<TInput, TOutput> reduceInputFunction)
        {
            this.reduceInputFunction = reduceInputFunction;
        }

        public TOutput Reduce(TInput input)
        {
            return reduceInputFunction(input);
        }
    }
}
