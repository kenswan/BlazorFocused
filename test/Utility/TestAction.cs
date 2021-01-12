using BlazorFocused.Store;
using BlazorFocused.Core.Test.Model;

namespace BlazorFocused.Core.Test.Utility
{
    public class TestAction : IAction<SimpleClass>
    {
        public SimpleClass Execute(SimpleClass simpleClass)
        {
            return SimpleClassUtilities.GetRandomSimpleClass();
        }
    }
}
