using System.Threading.Tasks;
using BlazorFocused.Core.Test.Model;
using BlazorFocused.Store;

namespace BlazorFocused.Core.Test.Utility
{
    public class TestActionAsync : TestClass, IActionAsync<SimpleClass>
    {
        public static string DefaultUrl { get; } = "api/with/type";

        private readonly ITestHttpService testHttpService;
        private string url;

        public TestActionAsync() { }

        public TestActionAsync(TestHttpService testHttpService)
        {
            this.testHttpService = testHttpService;
            url = DefaultUrl;
        }

        public TestActionAsync(string url)
        {
            this.url = url;
        }

        public async ValueTask<SimpleClass> ExecuteAsync(SimpleClass state)
        {
            return await testHttpService.TestGetValueAsync<SimpleClass>(url);
        }
    }
}
