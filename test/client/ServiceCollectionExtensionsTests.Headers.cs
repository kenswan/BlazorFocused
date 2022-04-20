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
        var baseAddress = new Faker().Internet.Url();
        var relativeUrl = new Faker().Internet.UrlRootedPath();
        var responseObject = RestClientTestExtensions.GenerateResponseObject();
        var simulatedHttp = ToolsBuilder.CreateSimulatedHttp(baseAddress);
        var headerKey = new Faker().Random.AlphaNumeric(5);
        var headerValue = new Faker().Internet.Ipv6();

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

        using var scope = serviceProvider.CreateScope();
        var restClient = scope.ServiceProvider.GetRequiredService<IRestClient>();

        restClient.AddHeader(headerKey, headerValue, global: true);

        _ = await restClient.GetAsync<SimpleClass>(relativeUrl);

        var actualHeaderValue =
            simulatedHttp.GetRequestHeaderValues(HttpMethod.Get, relativeUrl, headerKey)
                .FirstOrDefault();

        Assert.Equal(headerValue, actualHeaderValue);
    }

    [Fact]
    public async Task ShouldAddOAuthRestClientHeaders()
    {
        var baseAddress = new Faker().Internet.Url();
        var relativeUrl = new Faker().Internet.UrlRootedPath();
        var responseObject = RestClientTestExtensions.GenerateResponseObject();
        var simulatedHttp = ToolsBuilder.CreateSimulatedHttp(baseAddress);
        var headerKey = new Faker().Random.AlphaNumeric(5);
        var headerValue = new Faker().Internet.Ipv6();

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

        using var scope = serviceProvider.CreateScope();
        var oAuthClient = scope.ServiceProvider.GetRequiredService<IOAuthRestClient>();

        oAuthClient.AddHeader(headerKey, headerValue, global: true);

        _ = await oAuthClient.GetAsync<SimpleClass>(relativeUrl);

        var actualHeaderValue =
            simulatedHttp.GetRequestHeaderValues(HttpMethod.Get, relativeUrl, headerKey)
                .FirstOrDefault();

        Assert.Equal(headerValue, actualHeaderValue);
    }

    [Fact]
    public async Task ShouldOnlyAddHeadersToOneRequestWhenLocal()
    {
        var baseAddress = new Faker().Internet.Url();
        var firstRelativeUrl = new Faker().Internet.UrlRootedPath();
        var secondRelativeUrl = new Faker().Internet.UrlRootedPath();
        var responseObject = RestClientTestExtensions.GenerateResponseObject();
        var simulatedHttp = ToolsBuilder.CreateSimulatedHttp(baseAddress);
        var headerKey = new Faker().Random.AlphaNumeric(5);
        var headerValue = new Faker().Internet.Ipv6();

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

        using var firstScope = serviceProvider.CreateScope();
        var firstRestClient = firstScope.ServiceProvider.GetRequiredService<IRestClient>();

        firstRestClient.AddHeader(headerKey, headerValue, global: false);

        _ = await firstRestClient.GetAsync<SimpleClass>(firstRelativeUrl);

        var firstHeaderValue =
            simulatedHttp.GetRequestHeaderValues(HttpMethod.Get, firstRelativeUrl, headerKey)
                .FirstOrDefault();

        Assert.Equal(headerValue, firstHeaderValue);

        using var secondScope = serviceProvider.CreateScope();
        var secondRestClient = firstScope.ServiceProvider.GetRequiredService<IRestClient>();

        _ = await secondRestClient.GetAsync<SimpleClass>(firstRelativeUrl);

        var secondHeaderValue =
            simulatedHttp.GetRequestHeaderValues(HttpMethod.Get, secondRelativeUrl, headerKey)
                .FirstOrDefault();

        Assert.Null(secondHeaderValue);
    }
}
