using FluentAssertions;
using Integration.Server;
using Integration.Test.Utility;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Integration.Test.Server.Controllers
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
        public async Task ShouldGetDefaultUser()
        {
            HttpClient client = webApplicationFactory.CreateClient();
            string url = "api/user";

            var response = await client.GetAsync(url);

            response.Should().NotBeNull()
                .And.Match<HttpResponseMessage>(message => message.StatusCode == HttpStatusCode.OK);
        }
    }
}
