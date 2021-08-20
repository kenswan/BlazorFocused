using BlazorFocused.Model;

namespace BlazorFocused.Utility
{
    public class TestActionWithInput : TestActionState<SimpleClass, string>
    {
        public override SimpleClass Execute(string input)
        {
            return SimpleClassUtilities.GetStaticSimpleClass(input);
        }
    }
}
