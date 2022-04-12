using BlazorFocused.Tools.Model;
using Bogus;
using System.Net;

namespace BlazorFocused;

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

    public static Dictionary<string, string> GenerateRequestParameters(int count)
    {
        var parameters = new Dictionary<string, string>();

        for (int i = 0; i < count; i++)
            parameters.Add(GenerateParameter(), GenerateParameter());

        return parameters;
    }

    public static string GenerateParameter() =>
        new Faker().Random.String2(new Faker().Random.Int(20, 30));

    private static Faker<SimpleClass> GetSimpleClassFaker() =>
        new Faker<SimpleClass>()
            .RuleForType(typeof(string), faker => faker.Lorem.Sentence());
}
