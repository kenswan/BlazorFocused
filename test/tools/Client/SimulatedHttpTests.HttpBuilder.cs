using BlazorFocused.Client;
using BlazorFocused.Tools.Extensions;
using BlazorFocused.Tools.Model;
using Bogus;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using System.Net;
using Xunit;

namespace BlazorFocused.Tools.Client
{
    public partial class SimulatedHttpTests
    {
        [Fact]
        public async Task ShouldProvideMockDataThroughDependencyInjection()
        {
            var relativeUrl = new Faker().Internet.UrlRootedPath();
            var expectedResponse = SimpleClassUtilities.GetRandomSimpleClass();
            var serviceCollection = new ServiceCollection();

            serviceCollection
                .AddConfiguration()
                .AddRestClient(baseAddress)
                .AddSimulatedHttp(simulatedHttp);

            using var serviceProvider = serviceCollection.BuildServiceProvider();
            var restClient = serviceProvider.GetRequiredService<IRestClient>();

            simulatedHttp.SetupGET(relativeUrl)
                .ReturnsAsync(HttpStatusCode.OK, expectedResponse);

            var actualResponse = await restClient.GetAsync<SimpleClass>(relativeUrl);

            actualResponse.Should().BeEquivalentTo(expectedResponse);
        }
    }
}
