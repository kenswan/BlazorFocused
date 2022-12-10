// -------------------------------------------------------
// Copyright (c) Ken Swan All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using BlazorFocused.Tools.Extensions;
using BlazorFocused.Tools.Model;
using Bogus;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using Xunit;

namespace BlazorFocused.Tools.Http;

public partial class SimulatedHttpTests
{
    [Fact]
    public async Task ShouldProvideMockDataThroughDependencyInjection()
    {
        var relativeUrl = new Faker().Internet.UrlRootedPath();
        SimpleClass expectedResponse = SimpleClassUtilities.GetRandomSimpleClass();
        var serviceCollection = new ServiceCollection();

        serviceCollection
            .AddConfiguration()
            .AddRestClient(baseAddress)
            .AddSimulatedHttp(simulatedHttp);

        using ServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();
        IRestClient restClient = serviceProvider.GetRequiredService<IRestClient>();

        simulatedHttp.SetupGET(relativeUrl)
            .ReturnsAsync(HttpStatusCode.OK, expectedResponse);

        SimpleClass actualResponse = await restClient.GetAsync<SimpleClass>(relativeUrl);

        actualResponse.Should().BeEquivalentTo(expectedResponse);
    }
}
