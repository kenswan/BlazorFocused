using Bogus;
using System;

namespace BlazorFocused.Test.Model
{
    public class SimpleClassUtilities
    {
        public static SimpleClass GetRandomSimpleClass()
        {
            return new Faker<SimpleClass>()
                .RuleForType(typeof(string), fake => fake.Random.AlphaNumeric(GetRandomInteger()))
                .Generate();
        }

        public static SimpleClass GetStaticSimpleClass(string input)
        {
            return new Faker<SimpleClass>()
                .RuleForType(typeof(string), _ => input)
                .Generate();
        }

        public static int GetRandomInteger() =>
            new Random().Next(5, 20);
    }
}
