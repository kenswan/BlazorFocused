namespace BlazorFocused.Tools.Http
{
    internal partial class SimulatedHttp
    {
        public IEnumerable<string> GetHeaderValues(HttpMethod method, string url, string key)
        {
            var match = headers
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
    }
}
