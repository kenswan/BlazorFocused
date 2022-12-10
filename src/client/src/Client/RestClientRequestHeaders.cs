// -------------------------------------------------------
// Copyright (c) Ken Swan All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

namespace BlazorFocused.Client;

internal class RestClientRequestHeaders : IRestClientRequestHeaders
{
    private readonly Dictionary<string, List<string>> headerCache = new();

    public RestClientRequestHeaders()
    {
        headerCache = new();
    }

    public RestClientRequestHeaders(Dictionary<string, List<string>> headers)
    {
        headerCache = headers;
    }

    public void AddHeader(string key, string value)
    {
        if (headerCache.ContainsKey(key))
        {
            headerCache[key].Add(value);
        }
        else
        {
            headerCache.Add(key, new List<string> { value });
        }
    }

    public void ClearHeaders()
    {
        headerCache.Clear();
    }

    public IEnumerable<string> GetHeaderKeys()
    {
        return headerCache.Keys;
    }

    public IEnumerable<string> GetHeaderValues(string key)
    {
        var keyExists = headerCache.TryGetValue(key, out List<string> values);

        return keyExists ? values : Enumerable.Empty<string>();
    }

    public bool HasValues()
    {
        return headerCache.Any();
    }
}
