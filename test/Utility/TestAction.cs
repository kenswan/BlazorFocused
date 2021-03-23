using BlazorFocused.Store;
using BlazorFocused.Test.Model;

namespace BlazorFocused.Test.Utility
{
    public class TestAction : TestActionState<SimpleClass>, IAction<SimpleClass>
    {
        public SimpleClass Execute()
        {
            return SimpleClassUtilities.GetRandomSimpleClass();
        }
    }
}
