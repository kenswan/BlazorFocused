using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using BlazorFocused.Integration.Server;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace BlazorFocused.Integration.Test.Server.Controllers
{
    public class WeatherForecastControllerTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> webApplicationFactory;

        public WeatherForecastControllerTests(WebApplicationFactory<Startup> webApplicationFactory)
        {
            this.webApplicationFactory = webApplicationFactory;
        }

        [Fact]
        public async ValueTask ShouldGetWeatherData()
        {
            var client = webApplicationFactory.CreateClient();
            var url = "api/weatherforecast";

            var response = await client.GetAsync(url);

            response.Should().NotBeNull()
                .And.Match<HttpResponseMessage>(message => message.StatusCode == HttpStatusCode.OK);
        }
    }
}
