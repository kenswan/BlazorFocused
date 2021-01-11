using BlazoRx.Store;
using BlazoRx.Core.Test.Model;

namespace BlazoRx.Core.Test.Utility
{
    public class TestReducer : IReducer<SimpleClass, SimpleClassSubset>
    {
        public SimpleClassSubset Execute(SimpleClass input)
        {
            return new SimpleClassSubset
            {
                FieldOne = input.FieldOne,
                FieldThree = input.FieldThree,
                FieldFive = input.FieldFive
            };
        }
    }
}
