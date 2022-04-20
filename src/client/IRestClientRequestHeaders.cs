namespace BlazorFocused;
public interface IRestClientRequestHeaders
{
    public void AddHeader(string key, string value);

    public void ClearHeaders();

    public IEnumerable<string> GetHeaderKeys();

    public IEnumerable<string> GetHeaderValues(string key);

    public bool HasValues();
}
