﻿// -------------------------------------------------------
// Copyright (c) Ken Swan All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

namespace BlazorFocused.Tools.Http;

internal partial class SimulatedHttp
{
    public IEnumerable<string> GetRequestHeaderValues(HttpMethod method, string url, string key)
    {
        SimulatedHttpHeaders match = requestHeaders
                .Where(request => request.Method == method && request.Url == GetFullUrl(url))
                .FirstOrDefault();

        if (match is not null)
        {
            bool containsKey = match.Headers.TryGetValue(key, out IEnumerable<string> values);

            if (containsKey)
            {
                return values;
            }
        }

        return Enumerable.Empty<string>();
    }

    public void AddResponseHeader(string key, string value)
    {
        if (ResponseHeaders.ContainsKey(key))
        {
            ResponseHeaders[key].Add(value);
        }
        else
        {
            ResponseHeaders.Add(key, new List<string> { value });
        }
    }
}
