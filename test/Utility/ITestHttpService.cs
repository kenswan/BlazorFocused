using System.Net.Http;
using System.Threading.Tasks;

namespace BlazorFocused.Core.Test.Utility
{
    public interface ITestHttpService
    {
        HttpClient HttpClient { get; }

        ValueTask<T> GetValueAsync<T>(string url);
    }
}
