// -------------------------------------------------------
// Copyright (c) Ken Swan All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using BlazorFocused.Tools;
using BlazorFocused.Tools.Extensions;
using Bogus;
using Microsoft.Extensions.Options;
using Moq;
using Xunit.Abstractions;

namespace BlazorFocused.Client;

public partial class RestClientTests
{
    private readonly string baseAddress;
    private readonly Mock<IRestClientRequestHeaders> restClientRequestHeadersMock;
    private readonly ISimulatedHttp simulatedHttp;
    private readonly ITestLogger<RestClient> testLogger;
    private readonly IRestClient restClient;

    public RestClientTests(ITestOutputHelper testOutputHelper)
    {
        baseAddress = new Faker().Internet.Url();
        restClientRequestHeadersMock = new();
        simulatedHttp = ToolsBuilder.CreateSimulatedHttp(baseAddress);

        void logAction(Microsoft.Extensions.Logging.LogLevel level, string message, Exception exception)
        {
            testOutputHelper.WriteTestLoggerMessage(level, message, exception);
        }

        testLogger = ToolsBuilder.CreateTestLogger<RestClient>(logAction);

        IOptions<RestClientOptions> restClientOptions = Options.Create<RestClientOptions>(default);

        restClient =
            new RestClient(simulatedHttp.HttpClient, restClientOptions, restClientRequestHeadersMock.Object, testLogger);
    }
}
