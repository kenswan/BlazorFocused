using BlazorFocused.Model;

namespace BlazorFocused.Utility
{
    public class TestAction : TestActionState<SimpleClass>
    {
        public override SimpleClass Execute()
        {
            return SimpleClassUtilities.GetRandomSimpleClass();
        }
    }
}
