using BlazorFocused.Tools;
using BlazorFocused.Tools.Extensions;
using BlazorFocused.Tools.Model;
using Bogus;
using Microsoft.Extensions.Options;
using System.Net;
using Xunit.Abstractions;

namespace BlazorFocused.Client
{
    public partial class RestClientTests
    {
        private readonly string baseAddress;
        private readonly ISimulatedHttp simulatedHttp;
        private readonly ITestLogger<RestClient> testLogger;
        private readonly IRestClient restClient;

        public RestClientTests(ITestOutputHelper testOutputHelper)
        {
            baseAddress = new Faker().Internet.Url();
            simulatedHttp = ToolsBuilder.CreateSimulatedHttp(baseAddress);

            testLogger = ToolsBuilder.CreateTestLogger<RestClient>((level, message, exception) =>
                testOutputHelper.WriteTestLoggerMessage(level, message, exception));

            var restClientOptions = Options.Create<RestClientOptions>(default);

            restClient =
                new RestClient(simulatedHttp.HttpClient, restClientOptions, testLogger);
        }

        private ISimulatedHttpSetup GetHttpSetup(HttpMethod httpMethod, string url, object request) =>
            httpMethod switch
            {
                { } when httpMethod == HttpMethod.Delete => simulatedHttp.SetupDELETE(url),
                { } when httpMethod == HttpMethod.Get => simulatedHttp.SetupGET(url),
                { } when httpMethod == HttpMethod.Patch => simulatedHttp.SetupPATCH(url, request),
                { } when httpMethod == HttpMethod.Post => simulatedHttp.SetupPOST(url, request),
                { } when httpMethod == HttpMethod.Put => simulatedHttp.SetupPUT(url, request),
                _ => null
            };

        private static HttpStatusCode GenerateSuccessStatusCode() =>
            new Faker().PickRandom(
                HttpStatusCode.OK, HttpStatusCode.Created, HttpStatusCode.Accepted);

        private static HttpStatusCode GenerateErrorStatusCode() =>
            new Faker().PickRandom(
                HttpStatusCode.BadRequest, HttpStatusCode.NotFound, HttpStatusCode.InternalServerError);

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
