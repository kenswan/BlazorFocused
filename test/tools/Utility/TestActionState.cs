// -------------------------------------------------------
// Copyright (c) Ken Swan All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

namespace BlazorFocused.Tools.Utility;

public abstract class TestActionState<TState> : StoreAction<TState>
{
    public string CheckedPropertyId { get; set; }
}

public abstract class TestActionState<TState, TInput> : StoreAction<TState, TInput>
{
    public string CheckedPropertyId { get; set; }
}
