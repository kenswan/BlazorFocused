namespace BlazoRx.Core.Action
{
    public interface IAction<TState>
    {
        TState Execute(TState state);
    }
}
