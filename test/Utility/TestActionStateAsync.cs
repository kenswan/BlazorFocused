using BlazorFocused.Store;

namespace BlazorFocused.Test.Utility
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
