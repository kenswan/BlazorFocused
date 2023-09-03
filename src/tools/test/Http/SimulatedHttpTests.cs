// -------------------------------------------------------
// Copyright (c) Ken Swan All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using BlazorFocused.Tools.Model;
using Bogus;
using System.Net;
using System.Text;
using System.Text.Json;
using Xunit;

namespace BlazorFocused.Tools.Http;

public partial class SimulatedHttpTests
{
    private readonly ISimulatedHttp simulatedHttp;
    private readonly string baseAddress;

    public SimulatedHttpTests()
    {
        baseAddress = GetRandomUrl();
        simulatedHttp = new SimulatedHttp(baseAddress);
    }

    public static TheoryData<HttpMethod, HttpStatusCode, string, SimpleClass, SimpleClass> HttpData =>
        new()
        {
            {
                HttpMethod.Delete,
                GetRandomStatusCode(),
                GetRandomRelativeUrl(),
                null,                    // Request
                GetRandomSimpleClass() // Response
            },
            {
                HttpMethod.Get,
                GetRandomStatusCode(),
                GetRandomRelativeUrl(),
                null,
                GetRandomSimpleClass()
            },
            {
                HttpMethod.Patch,
                GetRandomStatusCode(),
                GetRandomRelativeUrl(),
                GetRandomSimpleClass(),
                GetRandomSimpleClass()
            },
            {
                HttpMethod.Post,
                GetRandomStatusCode(),
                GetRandomRelativeUrl(),
                GetRandomSimpleClass(),
                GetRandomSimpleClass()
            },
            {
                HttpMethod.Put,
                GetRandomStatusCode(),
                GetRandomRelativeUrl(),
                GetRandomSimpleClass(),
                GetRandomSimpleClass()
            }
        };

    private ISimulatedHttpSetup GetHttpSetup(HttpMethod httpMethod, string url, object request) => httpMethod switch
    {
        { } when httpMethod == HttpMethod.Delete => simulatedHttp.SetupDELETE(url),
        { } when httpMethod == HttpMethod.Get => simulatedHttp.SetupGET(url),
        { } when httpMethod == HttpMethod.Patch => simulatedHttp.SetupPATCH(url, request),
        { } when httpMethod == HttpMethod.Post => simulatedHttp.SetupPOST(url, request),
        { } when httpMethod == HttpMethod.Put => simulatedHttp.SetupPUT(url, request),
        _ => null
    };

    private static Task<HttpResponseMessage> MakeRequest(
        HttpClient client, HttpMethod httpMethod, string url, object request) => httpMethod switch
        {
            HttpMethod method when method == HttpMethod.Delete => client.DeleteAsync(url),
            HttpMethod method when method == HttpMethod.Get => client.GetAsync(url),

            HttpMethod method when method == HttpMethod.Patch =>
                client.PatchAsync(url, GetHttpContent(request)),

            HttpMethod method when method == HttpMethod.Post =>
                client.PostAsync(url, GetHttpContent(request)),

            HttpMethod method when method == HttpMethod.Put =>
                client.PutAsync(url, GetHttpContent(request)),

            _ => throw new SimulatedHttpTestException($"{httpMethod} not supported"),
        };

    private static HttpContent GetHttpContent(object content)
    {
        string contentString = JsonSerializer.Serialize(content);

        return content is not null ?
            new StringContent(contentString, Encoding.UTF8, "application/json") : default;
    }
    private static HttpMethod PickDifferentMethod(HttpMethod httpMethod)
    {
        var methods = new List<HttpMethod>
        {
            HttpMethod.Delete,
            HttpMethod.Get,
            HttpMethod.Patch,
            HttpMethod.Post,
            HttpMethod.Put
        };

        methods.Remove(httpMethod);

        return new Faker().PickRandom(methods);
    }

    private static HttpStatusCode GetRandomStatusCode() => new Faker().PickRandom<HttpStatusCode>();

    private static string GetRandomUrl() => new Faker().Internet.Url();

    private static string GetRandomRelativeUrl() => new Faker().Internet.UrlRootedPath();

    private static SimpleClass GetRandomSimpleClass() => new Faker<SimpleClass>()
            .RuleForType(typeof(string), fake => fake.Lorem.Sentence(GetRandomNumber()))
            .Generate();

    private static int GetRandomNumber() => new Faker().Random.Int(4, 10);
}
