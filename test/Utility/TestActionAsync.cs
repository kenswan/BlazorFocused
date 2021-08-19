using BlazorFocused.Model;
using System.Threading.Tasks;

namespace BlazorFocused.Utility
{
    public class TestActionAsync : TestActionStateAsync<SimpleClass>
    {
        private readonly TestService testService;

        public TestActionAsync() { }

        public TestActionAsync(TestService testService)
        {
            this.testService = testService;
        }

        public override async ValueTask<SimpleClass> ExecuteAsync()
        {
            return await testService.GetValueAsync<SimpleClass>();
        }
    }
}
