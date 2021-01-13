using ClientPage = BlazorFocused.Integration.Client.Pages;
using Bunit;
using Xunit;

namespace BlazorFocused.Integration.Test.Client.Pages
{
    public class IndexTests
    {
        private readonly TestContext context;

        public IndexTests()
        {
            context = new TestContext();
        }

        [Fact(DisplayName = "Should render intro page")]
        public void ShouldRenderWelcome()
        {
            var component = context.RenderComponent<ClientPage.Index>();
            
            component.Find("h1").MarkupMatches("<h1>Integration Overview</h1>");
        }
    }
}
