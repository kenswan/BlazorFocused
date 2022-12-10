// -------------------------------------------------------
// Copyright (c) Ken Swan All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

namespace BlazorFocused.Client;

internal interface IOAuthToken
{
    public string Scheme { get; }

    public string Token { get; }

    public void Update(string scheme, string token);

    public bool IsEmpty();
}
