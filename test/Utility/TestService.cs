using System;
using System.Threading.Tasks;

namespace BlazorFocused.Core.Test.Utility
{
    public class TestService : TestClass
    {
        public TestService()
        {
        }

        public ValueTask<T> GetValueAsync<T>()
        {
            throw new NotImplementedException();
        }
    }
}
