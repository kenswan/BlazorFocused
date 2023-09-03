// -------------------------------------------------------
// Copyright (c) Ken Swan All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

namespace BlazorFocused;

public interface IApplicationClientBuilder
{
    IApplicationClientRequestBuilder Create(string name);

    IApplicationClientRequestBuilder Create(Uri baseAddressUri);

    IApplicationClientRequestBuilder Create(HttpClient internalHttpClient);
}
