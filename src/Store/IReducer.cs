namespace BlazorFocused.Store
{
    public interface IReducer<TInput, TOutput> where TInput : class
    {
        TOutput Execute(TInput input);
    }
}
