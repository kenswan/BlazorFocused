﻿// -------------------------------------------------------
// Copyright (c) Ken Swan All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

namespace BlazorFocused.Tools.Http;

internal class SimulatedHttpHeaders
{
    public HttpMethod Method { get; set; }

    public string Url { get; set; }

    public Dictionary<string, IEnumerable<string>> Headers { get; set; }
}
