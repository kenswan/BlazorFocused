using BlazorFocused.Store;
using BlazorFocused.Test.Model;

namespace BlazorFocused.Test.Utility
{
    public class TestActionWithInput : TestActionState<SimpleClass>, IAction<SimpleClass, string>
    { 
        public SimpleClass Execute(string input)
        {
            return SimpleClassUtilities.GetRandomSimpleClass(input);
        }
    }
}
