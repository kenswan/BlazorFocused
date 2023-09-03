// -------------------------------------------------------
// Copyright (c) Ken Swan All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

namespace BlazorFocused.Tools.Utility;

public class TestService : TestClass, ITestService
{
    public TestService() { }

    public virtual ValueTask<T> GetValueAsync<T>() => new(default(T));

    public virtual ValueTask<TOutput> GetValueAsync<TInput, TOutput>(TInput input) => new(default(TOutput));
}
