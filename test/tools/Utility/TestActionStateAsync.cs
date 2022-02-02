using BlazorFocused.Store;

namespace BlazorFocused.Tools.Utility
{
    public abstract class TestActionStateAsync<TState> : StoreActionAsync<TState>
    {
        public string CheckedPropertyId { get; set; }
    }

    public abstract class TestActionStateAsync<TState, TInput> : StoreActionAsync<TState, TInput>
    {
        public string CheckedPropertyId { get; set; }
    }
}
