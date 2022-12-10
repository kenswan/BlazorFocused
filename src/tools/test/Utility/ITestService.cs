// -------------------------------------------------------
// Copyright (c) Ken Swan All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

namespace BlazorFocused.Tools.Utility;

public interface ITestService
{
    ValueTask<T> GetValueAsync<T>();

    ValueTask<TOutput> GetValueAsync<TInput, TOutput>(TInput input);
}
