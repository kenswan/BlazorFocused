// -------------------------------------------------------
// Copyright (c) Ken Swan All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

namespace BlazorFocused.Tools.Utility;

public interface ITestHttpService
{
    HttpClient HttpClient { get; }

    ValueTask<T> GetValueAsync<T>(string url);
}
