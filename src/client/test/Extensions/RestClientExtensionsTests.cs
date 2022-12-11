// -------------------------------------------------------
// Copyright (c) Ken Swan All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using BlazorFocused.Client;
using BlazorFocused.Tools;
using BlazorFocused.Tools.Extensions;
using Bogus;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace BlazorFocused.Extensions;

public partial class RestClientExtensionsTests
{
    private readonly string baseAddress;
    private readonly ISimulatedHttp simulatedHttp;
    private readonly ITestLogger<RestClient> testLogger;
    private readonly IRestClient restClient;
    private readonly Mock<IRestClient> restClientMock; // Use mock client for 'Try' requests

    public RestClientExtensionsTests(ITestOutputHelper testOutputHelper)
    {
        baseAddress = new Faker().Internet.Url();
        simulatedHttp = ToolsBuilder.CreateSimulatedHttp(baseAddress);

        void logAction(Microsoft.Extensions.Logging.LogLevel level, string message, Exception exception)
        {
            testOutputHelper.WriteTestLoggerMessage(level, message, exception);
        }

        testLogger = ToolsBuilder.CreateTestLogger<RestClient>(logAction);

        IOptions<RestClientOptions> restClientOptions = Options.Create<RestClientOptions>(default);

        restClient =
            new RestClient(simulatedHttp.HttpClient, restClientOptions, default, testLogger);

        restClientMock = new();
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
