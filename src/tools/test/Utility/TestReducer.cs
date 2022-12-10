// -------------------------------------------------------
// Copyright (c) Ken Swan All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using BlazorFocused.Tools.Model;

namespace BlazorFocused.Tools.Utility;

public class TestReducer : TestClass, IReducer<SimpleClass, SimpleClassSubset>
{
    public SimpleClassSubset Execute(SimpleClass input)
    {
        return new SimpleClassSubset
        {
            FieldOne = input.FieldOne,
            FieldThree = input.FieldThree,
            FieldFive = input.FieldFive
        };
    }
}
