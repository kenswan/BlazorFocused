using System;

namespace BlazoRx.Store
{
    public class Reducer<TInput, TOutput> : IReducer<TInput, TOutput> where TInput : class
    {
        private Func<TInput, TOutput> reduceInputFunction;

        public Reducer(Func<TInput, TOutput> reduceInputFunction)
        {
            this.reduceInputFunction = reduceInputFunction;
        }

        public TOutput Execute(TInput input)
        {
            return reduceInputFunction(input);
        }
    }
}
