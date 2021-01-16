using System.Threading.Tasks;
using BlazorFocused.Client;
using BlazorFocused.Core.Test.Model;
using BlazorFocused.Store;

namespace BlazorFocused.Core.Test.Utility
{
    public class TestActionAsync : IActionAsync<SimpleClass>
    {
        private readonly string url;

        public TestActionAsync()
        {
            this.url = "api/with/type";
        }

        public TestActionAsync(string url)
        {
            this.url = url;
        }

        public async ValueTask<SimpleClass> ExecuteAsync(IRestClient restClient, SimpleClass state)
        {
            return await restClient.GetAsync<SimpleClass>(url);
        }
    }
}
