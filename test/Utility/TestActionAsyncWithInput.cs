using BlazorFocused.Store;
using BlazorFocused.Test.Model;
using System.Threading.Tasks;

namespace BlazorFocused.Test.Utility
{
    public class TestActionAsyncWithInput : TestActionState<SimpleClass>, IActionAsync<SimpleClass, string>
    {
        private readonly TestService testService;

        public TestActionAsyncWithInput() { }

        public TestActionAsyncWithInput(TestService testService)
        {
            this.testService = testService;
        }

        public async ValueTask<SimpleClass> ExecuteAsync(string input)
        {
            return await testService.GetValueAsync<SimpleClass>();
        }
    }
}
