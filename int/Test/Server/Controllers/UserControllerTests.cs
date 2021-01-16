using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using BlazorFocused.Integration.Server;
using BlazorFocused.Integration.Test.Utility;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace BlazorFocused.Integration.Test.Server.Controllers
{
    public class UserControllerTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> webApplicationFactory;

        public UserControllerTests(WebApplicationFactory<Startup> webApplicationFactory)
        {
            this.webApplicationFactory = webApplicationFactory;
        }

        [Trait(nameof(Category), nameof(Category.Integration))]
        [Fact(DisplayName = "Should Get Default User")]
        public async ValueTask ShouldGetDefaultUser()
        {
            var client = webApplicationFactory.CreateClient();
            var url = "api/user";

            var response = await client.GetAsync(url);

            response.Should().NotBeNull()
                .And.Match<HttpResponseMessage>(message => message.StatusCode == HttpStatusCode.OK);
        }
    }
}
