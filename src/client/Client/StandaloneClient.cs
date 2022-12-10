﻿// -------------------------------------------------------
// Copyright (c) Ken Swan All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using Microsoft.Extensions.Logging;

namespace BlazorFocused.Client;

internal class StandaloneClient : BaseRestClient
{
    public StandaloneClient(HttpClient httpClient, ILogger logger)
        : base(httpClient, logger)
    { }
}
