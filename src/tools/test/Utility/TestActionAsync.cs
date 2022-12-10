// -------------------------------------------------------
// Copyright (c) Ken Swan All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using BlazorFocused.Tools.Model;

namespace BlazorFocused.Tools.Utility;

public class TestActionAsync : TestActionStateAsync<SimpleClass>
{
    private readonly TestService testService;

    public TestActionAsync() { }

    public TestActionAsync(TestService testService)
    {
        this.testService = testService;
    }

    public override async ValueTask<SimpleClass> ExecuteAsync()
    {
        return await testService.GetValueAsync<SimpleClass>();
    }
}
