using System.Threading.Tasks;
using BlazorFocused.Client;
using BlazorFocused.Core.Test.Model;
using BlazorFocused.Store;

namespace BlazorFocused.Core.Test.Utility
{
    public class TestActionAsync : IActionAsync<SimpleClass>
    {
        private string url;

        /* public TestActionAsync(string url)
        {
            this.url = url;
        } */

        public async ValueTask<SimpleClass> ExecuteAsync(IRestClient restClient, SimpleClass state)
        {
            url = "api/test";

            return await restClient.GetAsync<SimpleClass>(url);
        }
    }
}
