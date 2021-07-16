using BlazorFocused.Store;
using BlazorFocused.Test.Model;
using System.Threading.Tasks;

namespace BlazorFocused.Test.Utility
{
    public class TestActionAsyncWithInput : TestActionStateAsync<SimpleClass, string>
    {
        private readonly TestService testService;

        public TestActionAsyncWithInput() { }

        public TestActionAsyncWithInput(TestService testService)
        {
            this.testService = testService;
        }

        public override async ValueTask<SimpleClass> ExecuteAsync(string input)
        {
            return await testService.GetValueAsync<string, SimpleClass>(input);
        }
    }
}
