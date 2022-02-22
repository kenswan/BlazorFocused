using BlazorFocused.Tools;
using BlazorFocused.Tools.Extensions;
using Bogus;
using Microsoft.Extensions.Options;
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
    }
}
