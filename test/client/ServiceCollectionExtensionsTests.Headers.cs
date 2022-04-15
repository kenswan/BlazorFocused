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
    public async Task ShouldAddRestClientHeaders()
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

        restClient.AddHeader(headerKey, headerValue);

        _ = await restClient.GetAsync<SimpleClass>(relativeUrl);

        var actualHeaderValue =
            simulatedHttp.GetRequestHeaderValues(HttpMethod.Get, relativeUrl, headerKey)
                .FirstOrDefault();

        Assert.Equal(headerValue, actualHeaderValue);
    }

    [Fact]
    public void ShouldAddOAuthRestClientHeaders()
    {

    }
}
