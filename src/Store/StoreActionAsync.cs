using System.Threading.Tasks;

namespace BlazorFocused.Store
{
    /// <inheritdoc cref="IActionAsync{TState}"/>
    public abstract class StoreActionAsync<TState> : IActionAsync<TState>
    {
        public TState State { get; set; }

        public abstract ValueTask<TState> ExecuteAsync();
    }

    /// <inheritdoc cref="IActionAsync{TState, TInput}"/>
    public abstract class StoreActionAsync<TState, TInput> : IActionAsync<TState, TInput>
    {
        public TState State { get; set; }

        public abstract ValueTask<TState> ExecuteAsync(TInput input);
    }
}
