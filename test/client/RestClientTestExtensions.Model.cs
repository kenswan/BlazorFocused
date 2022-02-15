using BlazorFocused.Tools.Model;
using Bogus;
using System.Net;

namespace BlazorFocused
{
    internal static partial class RestClientTestExtensions
    {
        public static HttpStatusCode GenerateSuccessStatusCode() =>
            new Faker().PickRandom(
                HttpStatusCode.OK, HttpStatusCode.Created, HttpStatusCode.Accepted);

        public static HttpStatusCode GenerateErrorStatusCode() =>
            new Faker().PickRandom(
                HttpStatusCode.BadRequest, HttpStatusCode.NotFound, HttpStatusCode.InternalServerError);

        public static string GenerateRelativeUrl() =>
            new Faker().Internet.UrlRootedPath();

        public static IEnumerable<SimpleClass> GenerateResponseObjects() =>
            GetSimpleClassFaker().Generate(new Faker().Random.Int(2, 5));

        public static SimpleClass GenerateResponseObject() =>
            GetSimpleClassFaker().Generate();

        private static Faker<SimpleClass> GetSimpleClassFaker() =>
            new Faker<SimpleClass>()
                .RuleForType(typeof(string), faker => faker.Lorem.Sentence());
    }
}
