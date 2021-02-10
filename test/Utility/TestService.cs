using System;
using System.Threading.Tasks;

namespace BlazorFocused.Core.Test.Utility
{
    public class TestService : TestClass
    {
        public TestService() { }

        public virtual ValueTask<T> GetValueAsync<T>()
        {
            return new ValueTask<T>(default(T));
        }
    }
}
