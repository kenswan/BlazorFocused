﻿// -------------------------------------------------------
// Copyright (c) Ken Swan All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using Samples.Model;

namespace ToDoList.Stores;

public class ToDoStore
{
    public List<ToDo> InComplete { get; set; } = new();

    public List<ToDo> Complete { get; set; } = new();
}
