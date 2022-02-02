using BlazorFocused.Model;
using BlazorFocused.Testing;
using Bogus;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;

namespace BlazorFocused.Client
{
    public partial class RestClientTests
    {
        private readonly string baseAddress;
        private readonly ISimulatedHttp simulatedHttp;
        private readonly ILogger<RestClient> nullLogger;
        private readonly IRestClient restClient;

        public RestClientTests()
        {
            baseAddress = new Faker().Internet.Url();
            simulatedHttp = new SimulatedHttp(baseAddress);
            nullLogger = NullLogger<RestClient>.Instance;
            var restClientOptions = Options.Create<RestClientOptions>(default);

            restClient =
                new RestClient(simulatedHttp.HttpClient, restClientOptions, nullLogger);
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
