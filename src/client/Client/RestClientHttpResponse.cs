// -------------------------------------------------------
// Copyright (c) Ken Swan All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using System.Net;
using System.Net.Http.Headers;

namespace BlazorFocused.Client;

internal class RestClientHttpResponse
{
    public HttpResponseHeaders Headers { get; set; }

    public HttpStatusCode? StatusCode { get; set; }
}

internal class RestClientHttpResponse<T> : RestClientHttpResponse
{
    public T Content { get; set; }
}
