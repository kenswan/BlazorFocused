﻿using BlazorFocused.Model;
using Bogus;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace BlazorFocused.Testing
{
    public partial class SimulatedHttpTests
    {
        private readonly ISimulatedHttp simulatedHttp;
        private readonly string baseAddress;

        public SimulatedHttpTests()
        {
            baseAddress = GetRandomUrl();
            simulatedHttp = new SimulatedHttp(baseAddress);
        }

        public static TheoryData<HttpMethod, HttpStatusCode, string, SimpleClass> HttpData =>
            new()
            {
                { HttpMethod.Delete, GetRandomStatusCode(), GetRandomRelativeUrl(), GetRandomSimpleClass() },
                { HttpMethod.Get, GetRandomStatusCode(), GetRandomRelativeUrl(), GetRandomSimpleClass() },
                { HttpMethod.Patch, GetRandomStatusCode(), GetRandomRelativeUrl(), GetRandomSimpleClass() },
                { HttpMethod.Post, GetRandomStatusCode(), GetRandomRelativeUrl(), GetRandomSimpleClass() },
                { HttpMethod.Put, GetRandomStatusCode(), GetRandomRelativeUrl(), GetRandomSimpleClass() }
            };

        private static Task<HttpResponseMessage> MakeRequest(HttpClient client, HttpMethod httpMethod, string url)
        {
            return httpMethod switch
            {
                HttpMethod method when method == HttpMethod.Delete => client.DeleteAsync(url),
                HttpMethod method when method == HttpMethod.Get => client.GetAsync(url),
                HttpMethod method when method == HttpMethod.Patch => client.PatchAsync(url, null),
                HttpMethod method when method == HttpMethod.Post => client.PostAsync(url, null),
                HttpMethod method when method == HttpMethod.Put => client.PutAsync(url, null),
                _ => throw new SimulatedHttpTestException($"{httpMethod} not supported"),
            };
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

        private static HttpStatusCode GetRandomStatusCode() =>
            new Faker().PickRandom<HttpStatusCode>();

        private static string GetRandomUrl() =>
            new Faker().Internet.Url();

        private static string GetRandomRelativeUrl() =>
            new Faker().Internet.UrlRootedPath();

        private static SimpleClass GetRandomSimpleClass() =>
            new Faker<SimpleClass>()
                .RuleForType(typeof(string), fake => fake.Lorem.Sentence(GetRandomNumber()))
                .Generate();

        private static int GetRandomNumber() =>
            new Faker().Random.Int(4, 10);
    }
}
