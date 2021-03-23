using System;
using System.Threading.Tasks;

namespace BlazorFocused.Test.Utility
{
    public class TestService : TestClass
    {
        public TestService() { }

        public virtual ValueTask<T> GetValueAsync<T>()
        {
            return new ValueTask<T>(default(T));
        }

        public virtual ValueTask<TOutput> GetValueAsync<TInput, TOutput>(TInput input)
        {
            return new ValueTask<TOutput>(default(TOutput));
        }
    }
}
