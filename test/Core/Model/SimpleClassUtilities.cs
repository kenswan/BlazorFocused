using System;
using Bogus;

namespace BlazorRx.Core.Test.Model
{
    public class SimpleClassUtilities
    {
        public static SimpleClass GetRandomSimpleClass()
        {
            return new Faker<SimpleClass>()
                .RuleForType(typeof(string), fake => fake.Random.AlphaNumeric(GetRandomInteger()));
        }

        public static int GetRandomInteger() =>
            new Random().Next(5, 20);
    }
}
