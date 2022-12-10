// -------------------------------------------------------
// Copyright (c) Ken Swan All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using BlazorFocused.Tools.Model;
using Bogus;
using System.Net;

namespace BlazorFocused;

internal static partial class RestClientTestExtensions
{
    public static HttpStatusCode GenerateSuccessStatusCode()
    {
        return new Faker().PickRandom(
            HttpStatusCode.OK, HttpStatusCode.Created, HttpStatusCode.Accepted);
    }

    public static HttpStatusCode GenerateErrorStatusCode()
    {
        return new Faker().PickRandom(
            HttpStatusCode.BadRequest, HttpStatusCode.NotFound, HttpStatusCode.InternalServerError);
    }

    public static string GenerateRelativeUrl()
    {
        return new Faker().Internet.UrlRootedPath();
    }

    public static IEnumerable<SimpleClass> GenerateResponseObjects()
    {
        return GetSimpleClassFaker().Generate(new Faker().Random.Int(2, 5));
    }

    public static SimpleClass GenerateResponseObject()
    {
        return GetSimpleClassFaker().Generate();
    }

    public static Dictionary<string, string> GenerateRequestParameters(int count)
    {
        var parameters = new Dictionary<string, string>();

        for (var i = 0; i < count; i++)
        {
            parameters.Add(GenerateParameter(), GenerateParameter());
        }

        return parameters;
    }

    public static string GenerateParameter()
    {
        return new Faker().Random.String2(new Faker().Random.Int(20, 30));
    }

    private static Faker<SimpleClass> GetSimpleClassFaker()
    {
        return new Faker<SimpleClass>()
            .RuleForType(typeof(string), faker => faker.Lorem.Sentence());
    }
}
