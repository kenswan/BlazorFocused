using System.Collections.Generic;
using BlazorFocused.Core.Test.Model;
using BlazorFocused.Testing;
using Bogus;
using Microsoft.Extensions.Logging;
using Moq;

namespace BlazorFocused.Client.Test
{
    public partial class RestClientTests
    {
        private readonly string baseAddress;
        private readonly FocusedHttp focusedHttp;
        private readonly Mock<ILogger<RestClient>> loggerMock;
        private readonly IRestClient restClient;

        public RestClientTests()
        {
            baseAddress = new Faker().Internet.Url();
            focusedHttp = new FocusedHttp(baseAddress);
            loggerMock = new Mock<ILogger<RestClient>>();
            restClient = new RestClient(focusedHttp.Client(), loggerMock.Object);
        }

        private static string GetRandomRelativeUrl() =>
            new Faker().Internet.UrlRootedPath();

        private static IEnumerable<SimpleClass> GetRandomResponseObjects() =>
            GetSimpleClassFaker().Generate(GetRandomNumber());

        private static SimpleClass GetRandomResponseObject() =>
            GetSimpleClassFaker().Generate();

        private static Faker<SimpleClass> GetSimpleClassFaker() =>
            new Faker<SimpleClass>()
                .RuleForType(typeof(string), faker => faker.Lorem.Sentence());

        private static int GetRandomNumber() =>
            new Faker().Random.Int(2, 5);
    }
}
