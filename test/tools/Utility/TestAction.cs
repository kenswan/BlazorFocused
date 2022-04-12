using BlazorFocused.Tools.Model;

namespace BlazorFocused.Tools.Utility;

public class TestAction : TestActionState<SimpleClass>
{
    public override SimpleClass Execute()
    {
        return SimpleClassUtilities.GetRandomSimpleClass();
    }
}
