using ClientPage = BlazorFocused.Integration.Client.Pages;
using Bunit;
using Xunit;
using BlazorFocused.Integration.Test.Utility;

namespace BlazorFocused.Integration.Test.Client.Pages
{
    public class IndexTests
    {
        private readonly TestContext context;

        public IndexTests()
        {
            context = new TestContext();
        }

        [Trait(nameof(Category), nameof(Category.Integration))]
        [Fact(DisplayName = "Should render intro page")]
        public void ShouldRenderWelcome()
        {
            var component = context.RenderComponent<ClientPage.Index>();
            
            component.Find("h1").MarkupMatches("<h1>Integration Overview</h1>");
        }
    }
}
