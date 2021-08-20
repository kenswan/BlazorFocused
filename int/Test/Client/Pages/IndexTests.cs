using Bunit;
using Integration.Utility;
using Xunit;

namespace Integration.Client.Pages
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
            var component = context.RenderComponent<Index>();

            component.Find("h1").MarkupMatches("<h1>Integration Overview</h1>");
        }
    }
}
