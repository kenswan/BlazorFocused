using BlazoRx.Store;
using BlazoRx.Core.Test.Model;

namespace BlazoRx.Core.Test.Utility
{
    public class TestAction : IAction<SimpleClass>
    {
        public SimpleClass Execute(SimpleClass simpleClass)
        {
            return SimpleClassUtilities.GetRandomSimpleClass();
        }
    }
}
