using BlazorFocused.Test.Model;

namespace BlazorFocused.Test.Utility
{
    public class TestActionWithInput : TestActionState<SimpleClass, string>
    {
        public override SimpleClass Execute(string input)
        {
            return SimpleClassUtilities.GetStaticSimpleClass(input);
        }
    }
}
