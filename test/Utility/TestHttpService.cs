using System.Net.Http;
using System.Threading.Tasks;

namespace BlazorFocused.Test.Utility
{
    public class TestHttpService : ITestHttpService
    {
        public HttpClient HttpClient { get; private set; }

        public TestHttpService(HttpClient httpClient)
        {
            this.HttpClient = httpClient;
        }

        public ValueTask<T> GetValueAsync<T>(string url)
        {
            return new ValueTask<T>(default(T));
        }
    }
}
