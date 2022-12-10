// -------------------------------------------------------
// Copyright (c) Ken Swan All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

namespace BlazorFocused.Tools.Utility;

public abstract class TestActionStateAsync<TState> : StoreActionAsync<TState>
{
    public string CheckedPropertyId { get; set; }
}

public abstract class TestActionStateAsync<TState, TInput> : StoreActionAsync<TState, TInput>
{
    public string CheckedPropertyId { get; set; }
}
