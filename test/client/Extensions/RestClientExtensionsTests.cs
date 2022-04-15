using BlazorFocused.Client;
using BlazorFocused.Tools;
using BlazorFocused.Tools.Extensions;
using Bogus;
using Microsoft.Extensions.Options;
using Xunit;
using Xunit.Abstractions;

namespace BlazorFocused.Extensions;

public partial class RestClientExtensionsTests
{
    private readonly string baseAddress;
    private readonly ISimulatedHttp simulatedHttp;
    private readonly ITestLogger<RestClient> testLogger;
    private readonly IRestClient restClient;

    public RestClientExtensionsTests(ITestOutputHelper testOutputHelper)
    {
        baseAddress = new Faker().Internet.Url();
        simulatedHttp = ToolsBuilder.CreateSimulatedHttp(baseAddress);

        testLogger = ToolsBuilder.CreateTestLogger<RestClient>((level, message, exception) =>
            testOutputHelper.WriteTestLoggerMessage(level, message, exception));

        var restClientOptions = Options.Create<RestClientOptions>(default);

        restClient =
            new RestClient(simulatedHttp.HttpClient, restClientOptions, default, testLogger);
    }

    public static TheoryData<HttpMethod> HttpMethodsForResponse =>
        new()
        {
            { HttpMethod.Delete },
            { HttpMethod.Get },
            { HttpMethod.Patch },
            { HttpMethod.Post },
            { HttpMethod.Put },
        };

    public static TheoryData<HttpMethod> HttpMethodsForTask =>
        new()
        {
            { HttpMethod.Delete },
            { HttpMethod.Patch },
            { HttpMethod.Post },
            { HttpMethod.Put },
        };
}
