// -------------------------------------------------------
// Copyright (c) Ken Swan All rights reserved.
// Licensed under the MIT License
// -------------------------------------------------------

using BlazorFocused.Client;
using BlazorFocused.Tools;
using BlazorFocused.Tools.Extensions;
using BlazorFocused.Tools.Model;
using Bogus;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using Xunit;

namespace BlazorFocused;
public partial class ServiceCollectionExtensionsTests
{
    [Fact]
    public async Task ShouldAddRestClientHeadersGlobally()
    {
        string baseAddress = new Faker().Internet.Url();
        string relativeUrl = new Faker().Internet.UrlRootedPath();
        SimpleClass responseObject = RestClientTestExtensions.GenerateResponseObject();
        ISimulatedHttp simulatedHttp = ToolsBuilder.CreateSimulatedHttp(baseAddress);
        string headerKey = new Faker().Random.AlphaNumeric(5);
        string headerValue = new Faker().Internet.Ipv6();

        simulatedHttp.SetupGET(relativeUrl)
            .ReturnsAsync(HttpStatusCode.OK, responseObject);

        var serviceCollection = new ServiceCollection();
        serviceCollection
            .AddConfiguration()
            .AddRestClient(baseAddress)
            .AddSimulatedHttp(simulatedHttp);

        using var serviceProvider = serviceCollection
            .BuildProviderWithTestLoggers((services) =>
            {
                services.AddTestLoggerToCollection<RestClient>(testOutpuHelper);
                services.AddTestLoggerToCollection<RestClientHeaderHandler>(testOutpuHelper);
            }) as ServiceProvider;

        using IServiceScope scope = serviceProvider.CreateScope();
        IRestClient restClient = scope.ServiceProvider.GetRequiredService<IRestClient>();

        restClient.AddHeader(headerKey, headerValue, global: true);

        _ = await restClient.GetAsync<SimpleClass>(relativeUrl);

        string actualHeaderValue =
            simulatedHttp.GetRequestHeaderValues(HttpMethod.Get, relativeUrl, headerKey)
                .FirstOrDefault();

        Assert.Equal(headerValue, actualHeaderValue);
    }

    [Fact]
    public async Task ShouldAddOAuthRestClientHeaders()
    {
        string baseAddress = new Faker().Internet.Url();
        string relativeUrl = new Faker().Internet.UrlRootedPath();
        SimpleClass responseObject = RestClientTestExtensions.GenerateResponseObject();
        ISimulatedHttp simulatedHttp = ToolsBuilder.CreateSimulatedHttp(baseAddress);
        string headerKey = new Faker().Random.AlphaNumeric(5);
        string headerValue = new Faker().Internet.Ipv6();

        simulatedHttp.SetupGET(relativeUrl)
            .ReturnsAsync(HttpStatusCode.OK, responseObject);

        var serviceCollection = new ServiceCollection();
        serviceCollection
            .AddConfiguration()
            .AddOAuthRestClient(baseAddress)
            .AddSimulatedHttp(simulatedHttp);

        using var serviceProvider = serviceCollection
            .BuildProviderWithTestLoggers((services) =>
            {
                services.AddTestLoggerToCollection<OAuthRestClient>(testOutpuHelper);
                services.AddTestLoggerToCollection<RestClientHeaderHandler>(testOutpuHelper);
                services.AddTestLoggerToCollection<RestClientAuthHandler>(testOutpuHelper);
            }) as ServiceProvider;

        using IServiceScope scope = serviceProvider.CreateScope();
        IOAuthRestClient oAuthClient = scope.ServiceProvider.GetRequiredService<IOAuthRestClient>();

        oAuthClient.AddHeader(headerKey, headerValue, global: true);

        _ = await oAuthClient.GetAsync<SimpleClass>(relativeUrl);

        string actualHeaderValue =
            simulatedHttp.GetRequestHeaderValues(HttpMethod.Get, relativeUrl, headerKey)
                .FirstOrDefault();

        Assert.Equal(headerValue, actualHeaderValue);
    }

    [Fact]
    public async Task ShouldOnlyAddHeadersToOneRequestWhenLocal()
    {
        string baseAddress = new Faker().Internet.Url();
        string firstRelativeUrl = new Faker().Internet.UrlRootedPath();
        string secondRelativeUrl = new Faker().Internet.UrlRootedPath();
        SimpleClass responseObject = RestClientTestExtensions.GenerateResponseObject();
        ISimulatedHttp simulatedHttp = ToolsBuilder.CreateSimulatedHttp(baseAddress);
        string headerKey = new Faker().Random.AlphaNumeric(5);
        string headerValue = new Faker().Internet.Ipv6();

        simulatedHttp.SetupGET(firstRelativeUrl)
            .ReturnsAsync(HttpStatusCode.OK, responseObject);

        var serviceCollection = new ServiceCollection();
        serviceCollection
            .AddConfiguration()
            .AddRestClient(baseAddress)
            .AddSimulatedHttp(simulatedHttp);

        using var serviceProvider = serviceCollection
            .BuildProviderWithTestLoggers((services) =>
            {
                services.AddTestLoggerToCollection<RestClient>(testOutpuHelper);
                services.AddTestLoggerToCollection<RestClientHeaderHandler>(testOutpuHelper);
            }) as ServiceProvider;

        using IServiceScope firstScope = serviceProvider.CreateScope();
        IRestClient firstRestClient = firstScope.ServiceProvider.GetRequiredService<IRestClient>();

        firstRestClient.AddHeader(headerKey, headerValue, global: false);

        _ = await firstRestClient.GetAsync<SimpleClass>(firstRelativeUrl);

        string firstHeaderValue =
            simulatedHttp.GetRequestHeaderValues(HttpMethod.Get, firstRelativeUrl, headerKey)
                .FirstOrDefault();

        Assert.Equal(headerValue, firstHeaderValue);

        using IServiceScope secondScope = serviceProvider.CreateScope();
        IRestClient secondRestClient = firstScope.ServiceProvider.GetRequiredService<IRestClient>();

        _ = await secondRestClient.GetAsync<SimpleClass>(firstRelativeUrl);

        string secondHeaderValue =
            simulatedHttp.GetRequestHeaderValues(HttpMethod.Get, secondRelativeUrl, headerKey)
                .FirstOrDefault();

        Assert.Null(secondHeaderValue);
    }
}
