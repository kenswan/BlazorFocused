namespace BlazoRx.Core.Reducer
{
    public interface IReducer<TInput, TOutput> where TInput : class
    {
        TOutput Execute(TInput input);
    }
}
