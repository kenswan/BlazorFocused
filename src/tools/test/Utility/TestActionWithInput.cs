// -------------------------------------------------------
// Copyright (c) Ken Swan All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using BlazorFocused.Tools.Model;

namespace BlazorFocused.Tools.Utility;

public class TestActionWithInput : TestActionState<SimpleClass, string>
{
    public override SimpleClass Execute(string input) => SimpleClassUtilities.GetStaticSimpleClass(input);
}
