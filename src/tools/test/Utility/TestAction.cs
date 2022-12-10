// -------------------------------------------------------
// Copyright (c) Ken Swan All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using BlazorFocused.Tools.Model;

namespace BlazorFocused.Tools.Utility;

public class TestAction : TestActionState<SimpleClass>
{
    public override SimpleClass Execute()
    {
        return SimpleClassUtilities.GetRandomSimpleClass();
    }
}
