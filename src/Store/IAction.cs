namespace BlazoRx.Store
{
    public interface IAction<TState>
    {
        TState Execute(TState state);
    }
}
