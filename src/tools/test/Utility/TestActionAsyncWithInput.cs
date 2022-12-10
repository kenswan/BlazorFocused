// -------------------------------------------------------
// Copyright (c) Ken Swan All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using BlazorFocused.Tools.Model;

namespace BlazorFocused.Tools.Utility;

public class TestActionAsyncWithInput : TestActionStateAsync<SimpleClass, string>
{
    private readonly TestService testService;

    public TestActionAsyncWithInput() { }

    public TestActionAsyncWithInput(TestService testService)
    {
        this.testService = testService;
    }

    public override async ValueTask<SimpleClass> ExecuteAsync(string input)
    {
        return await testService.GetValueAsync<string, SimpleClass>(input);
    }
}
