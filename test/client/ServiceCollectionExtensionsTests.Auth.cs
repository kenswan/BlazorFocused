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
    public async Task ShouldAdddAuthTokenInstantlyAfterReceiving()
    {
        var token = new Faker().Internet.Password(20);
        var expectedAuthorization = $"Bearer {token}";
        var baseAddress = new Faker().Internet.Url();
        var relativeUrl = new Faker().Internet.UrlRootedPath();
        var responseObject = RestClientTestExtensions.GenerateResponseObject();

        var appSettings = new Dictionary<string, string>()
        {
            ["restclient:baseAddress"] = baseAddress
        };

        var simulatedHttp = ToolsBuilder.CreateSimulatedHttp(baseAddress);
        var serviceCollection = new ServiceCollection();
        serviceCollection
            .AddConfiguration(appSettings)
            .AddOAuthRestClient()
            .AddSimulatedHttp(simulatedHttp);

        simulatedHttp.SetupGET(relativeUrl)
            .ReturnsAsync(HttpStatusCode.OK, responseObject);

        using var serviceProvider = serviceCollection
            .BuildProviderWithTestLoggers((services) =>
            {
                services.AddTestLoggerToCollection<OAuthRestClient>(testOutpuHelper);
                services.AddTestLoggerToCollection<RestClientAuthHandler>(testOutpuHelper);
            }) as ServiceProvider;

        using var scope = serviceProvider.CreateScope();
        var oAuthRestClient = scope.ServiceProvider.GetRequiredService<IOAuthRestClient>();
        oAuthRestClient.AddAuthorization("Bearer", token);

        var response = await oAuthRestClient.GetAsync<SimpleClass>(relativeUrl);

        var actualAuthorization =
            simulatedHttp.GetRequestHeaderValues(HttpMethod.Get, relativeUrl, "Authorization")
                .FirstOrDefault();

        Assert.Equal(expectedAuthorization, actualAuthorization);
    }
}
