using System;
using Bogus;

namespace BlazorFocused.Test.Model
{
    public class SimpleClassUtilities
    {
        public static SimpleClass GetRandomSimpleClass()
        {
            return new Faker<SimpleClass>()
                .RuleForType(typeof(string), fake => fake.Random.AlphaNumeric(GetRandomInteger()));
        }

        public static SimpleClass GetRandomSimpleClass(string input)
        {
            return new Faker<SimpleClass>()
                .RuleForType(typeof(string), _ => input);
        }

        public static int GetRandomInteger() =>
            new Random().Next(5, 20);
    }
}
