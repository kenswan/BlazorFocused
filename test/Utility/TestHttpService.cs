using System.Net.Http;
using System.Threading.Tasks;

namespace BlazorFocused.Core.Test.Utility
{
    public class TestHttpService : ITestHttpService
    {
        public HttpClient HttpClient { get; private set; }

        public TestHttpService(HttpClient httpClient)
        {
            this.HttpClient = httpClient;
        }

        public ValueTask<T> TestGetValueAsync<T>(string url)
        {
            return new ValueTask<T>(default(T));
        }
    }
}
