using BlazorFocused.Client;
using BlazorFocused.Tools;
using BlazorFocused.Tools.Extensions;
using Bogus;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using Xunit;

namespace BlazorFocused
{
    public partial class ServiceCollectionExtensionsTests
    {
        [Fact]
        public void ShouldAdddAuthTokenInstantlyAfterReceiving()
        {
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

            using var serviceProvider = serviceCollection.BuildServiceProvider();
            using var scope = serviceProvider.CreateScope();
            var oAuthRestClient = scope.ServiceProvider.GetRequiredService<IOAuthRestClient>();
            using HttpClient httpClient = (oAuthRestClient as OAuthRestClient).GetClient();

            // TODO: Add upcoming implementation test for instant token request
        }
    }
}
