// -------------------------------------------------------
// Copyright (c) Ken Swan All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using BlazorFocused.Extensions;
using BlazorFocused.Tools;
using Bogus;
using Xunit.Abstractions;

namespace BlazorFocused.Client;

public partial class StandaloneClientTests
{
    private readonly string baseAddress;
    private readonly ISimulatedHttp simulatedHttp;
    private readonly IRestClient restClient;

    public StandaloneClientTests(ITestOutputHelper testOutputHelper)
    {
        baseAddress = new Faker().Internet.Url();
        simulatedHttp = ToolsBuilder.CreateSimulatedHttp(baseAddress);

        restClient =
            RestClientExtensions.CreateRestClient(simulatedHttp.HttpClient);
    }
}
