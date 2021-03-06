using BlazorFocused.Store;
using BlazorFocused.Test.Model;

namespace BlazorFocused.Test.Utility
{
    public class TestAction : TestClass, IAction<SimpleClass>
    {
        public SimpleClass Execute(SimpleClass simpleClass)
        {
            return SimpleClassUtilities.GetRandomSimpleClass();
        }
    }
}
