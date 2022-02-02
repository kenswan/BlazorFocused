namespace BlazorFocused.Utility
{
    public interface ITestHttpService
    {
        HttpClient HttpClient { get; }

        ValueTask<T> GetValueAsync<T>(string url);
    }
}
