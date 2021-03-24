using System.Threading.Tasks;
using BlazorFocused.Test.Model;
using BlazorFocused.Store;

namespace BlazorFocused.Test.Utility
{
    public class TestActionAsync : TestActionState<SimpleClass>, IActionAsync<SimpleClass>
    {
        private readonly TestService testService;

        public TestActionAsync() { }

        public TestActionAsync(TestService testService)
        {
            this.testService = testService;
        }

        public async ValueTask<SimpleClass> ExecuteAsync()
        {
            return await testService.GetValueAsync<SimpleClass>();
        }
    }
}
