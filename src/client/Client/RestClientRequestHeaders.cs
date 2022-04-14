namespace BlazorFocused.Client;
internal class RestClientRequestHeaders
{
    public bool Enabled { get; set; }

    public Dictionary<string, List<string>> HeaderCache { get; set; }

    public void ClearCache()
    {
        HeaderCache.Clear();
    }
}
