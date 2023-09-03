// -------------------------------------------------------
// Copyright (c) Ken Swan All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using BlazorFocused;
using ToDoList.Models;
using ToDoList.Stores;

namespace ToDoList.Reducers;

public class ToDoCountReducer : IReducer<ToDoStore, ToDoCount>
{
    public ToDoCount Execute(ToDoStore input)
    {
        int complete = input.Complete.Count;
        int incomplete = input.InComplete.Count;

        return new ToDoCount
        {
            Complete = complete,
            InComplete = incomplete,
            Total = complete + incomplete
        };
    }
}
