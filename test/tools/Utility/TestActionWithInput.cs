using BlazorFocused.Tools.Model;

namespace BlazorFocused.Tools.Utility
{
    public class TestActionWithInput : TestActionState<SimpleClass, string>
    {
        public override SimpleClass Execute(string input)
        {
            return SimpleClassUtilities.GetStaticSimpleClass(input);
        }
    }
}
