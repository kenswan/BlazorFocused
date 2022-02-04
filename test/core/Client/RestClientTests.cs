using BlazorFocused.Tools.Client;
using BlazorFocused.Tools.Extensions;
using BlazorFocused.Tools.Logger;
using BlazorFocused.Tools.Model;
using Bogus;
using Microsoft.Extensions.Options;
using Xunit.Abstractions;

namespace BlazorFocused.Client
{
    public partial class RestClientTests
    {
        private readonly string baseAddress;
        private readonly ISimulatedHttp simulatedHttp;
        private readonly IMockLogger<RestClient> logger;
        private readonly IRestClient restClient;

        public RestClientTests(ITestOutputHelper testOutputHelper)
        {
            baseAddress = new Faker().Internet.Url();
            simulatedHttp = new SimulatedHttp(baseAddress);

            logger = new MockLogger<RestClient>((level, message, exception) =>
                testOutputHelper.WriteMockLoggerMessage(level, message, exception));

            var restClientOptions = Options.Create<RestClientOptions>(default);

            restClient =
                new RestClient(simulatedHttp.HttpClient, restClientOptions, logger);
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
