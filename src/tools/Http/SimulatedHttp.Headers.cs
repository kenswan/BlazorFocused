namespace BlazorFocused.Tools.Http;

internal partial class SimulatedHttp
{
    public IEnumerable<string> GetRequestHeaderValues(HttpMethod method, string url, string key)
    {
        var match = requestHeaders
                .Where(request => request.Method == method && request.Url == GetFullUrl(url))
                .FirstOrDefault();

        if (match is not null)
        {
            var containsKey = match.Headers.TryGetValue(key, out IEnumerable<string> values);

            if (containsKey)
                return values;
        }

        return Enumerable.Empty<string>();
    }

    public void AddResponseHeader(string key, string value)
    {
        if (responseHeaders.ContainsKey(key))
            responseHeaders[key].Add(value);
        else
            responseHeaders.Add(key, new List<string> { value });
    }
}
