using System.Threading.Tasks;
using BlazorFocused.Test.Model;
using BlazorFocused.Store;

namespace BlazorFocused.Test.Utility
{
    public class TestActionAsync : TestClass, IActionAsync<SimpleClass>
    {
        private readonly TestService testService;

        public TestActionAsync() { }

        public TestActionAsync(TestService testService)
        {
            this.testService = testService;
        }

        public async ValueTask<SimpleClass> ExecuteAsync(SimpleClass state)
        {
            return await testService.GetValueAsync<SimpleClass>();
        }
    }
}
