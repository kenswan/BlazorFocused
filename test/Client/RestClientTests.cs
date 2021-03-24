using System.Collections.Generic;
using BlazorFocused.Test.Model;
using BlazorFocused.Testing;
using Bogus;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace BlazorFocused.Client.Test
{
    public partial class RestClientTests
    {
        private readonly string baseAddress;
        private readonly ISimulatedHttp simulatedHttp;
        private readonly ILogger<RestClient> nullLogger;
        private readonly IParameterBuilder parameterBuilder;
        private readonly IRestClient restClient;

        public RestClientTests()
        {
            baseAddress = new Faker().Internet.Url();
            simulatedHttp = new SimulatedHttp(baseAddress);
            nullLogger = NullLogger<RestClient>.Instance;
            parameterBuilder = new ParameterBuilder();
            restClient = new RestClient(simulatedHttp.Client(), parameterBuilder, nullLogger);
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
