namespace BlazorFocused.Store
{
    public interface IStoreAction<TState>
    {
        TState State { get; set; }
    }
}
