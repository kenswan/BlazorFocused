using BlazorFocused.Client;
using BlazorFocused.Extensions;
using BlazorFocused.Model;
using Bogus;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace BlazorFocused.Testing
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

            simulatedHttp.Setup(HttpMethod.Get, relativeUrl)
                .ReturnsAsync(HttpStatusCode.OK, expectedResponse);

            var actualResponse = await restClient.GetAsync<SimpleClass>(relativeUrl);

            actualResponse.Should().BeEquivalentTo(expectedResponse);
        }
    }
}
