// -------------------------------------------------------
// Copyright (c) Ken Swan All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

namespace BlazorFocused;

public interface IApplicationClientRequestBuilder
{
    IApplicationClient Get(string relativeUrl);

    IApplicationClient Delete(string relativeUrl);

    IApplicationClient Patch(string relativeUrl, object requestBody = null);

    IApplicationClient Post(string relativeUrl, object requestBody = null);

    IApplicationClient Put(string relativeUrl, object requestBody = null);
}
