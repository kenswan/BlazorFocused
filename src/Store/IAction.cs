namespace BlazorFocused.Store
{
    public interface IAction<TState>
    {
        TState Execute(TState state);
    }
}
