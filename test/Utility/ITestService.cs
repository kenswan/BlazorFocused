namespace BlazorFocused.Utility
{
    public interface ITestService
    {
        ValueTask<T> GetValueAsync<T>();

        ValueTask<TOutput> GetValueAsync<TInput, TOutput>(TInput input);
    }
}
