using BlazorFocused.Test.Model;

namespace BlazorFocused.Test.Utility
{
    public class TestAction : TestActionState<SimpleClass>
    {
        public override SimpleClass Execute()
        {
            return SimpleClassUtilities.GetRandomSimpleClass();
        }
    }
}
