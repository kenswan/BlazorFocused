using BlazorFocused.Store;

namespace BlazorFocused.Test.Utility
{
    public abstract class TestActionState<TState> : TestClass, IStoreAction<TState>
    {
        public TState State { get; set; }
    }
}
