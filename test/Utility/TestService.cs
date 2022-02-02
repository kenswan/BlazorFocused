namespace BlazorFocused.Utility
{
    public class TestService : TestClass, ITestService
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
