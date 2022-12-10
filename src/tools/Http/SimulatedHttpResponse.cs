// -------------------------------------------------------
// Copyright (c) Ken Swan All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using System.Net;

namespace BlazorFocused.Tools.Http;

internal class SimulatedHttpResponse : SimulatedHttpRequest
{
    public HttpStatusCode StatusCode { get; set; }

    public string ResponseContent { get; set; }
}
