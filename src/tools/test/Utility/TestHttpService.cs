// -------------------------------------------------------
// Copyright (c) Ken Swan All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

namespace BlazorFocused.Tools.Utility;

public class TestHttpService : ITestHttpService
{
    public HttpClient HttpClient { get; private set; }

    public TestHttpService(HttpClient httpClient)
    {
        this.HttpClient = httpClient;
    }

    public ValueTask<T> GetValueAsync<T>(string url)
    {
        return new ValueTask<T>(default(T));
    }
}
