﻿using BlazorFocused.Store;
using BlazorFocused.Model;

namespace BlazorFocused.Utility
{
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
}
