using BlazorFocused.Core.Test.Model;

namespace BlazorFocused.Store.Test
{
    public partial class StoreBuilderTests
    {
        private readonly IStoreBuilder<SimpleClass> storeBuilder;

        public StoreBuilderTests()
        {
            storeBuilder = new StoreBuilder<SimpleClass>();
        }
    }
}
