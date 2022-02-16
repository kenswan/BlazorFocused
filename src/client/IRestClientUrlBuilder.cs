namespace BlazorFocused
{
    public interface IRestClientUrlBuilder
    {
        string Build();

        IRestClientUrlBuilder SetPath(string absoluteOrRelativeUrl);

        IRestClientUrlBuilder WithParameter(string key, string value);
    }
}
