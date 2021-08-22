using FluentAssertions;
using Integration.Sdk.Models;
using Integration.Server.Models;
using Integration.Utility;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace Integration.Server.Controllers
{
    public class UserControllerTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> webApplicationFactory;

        public UserControllerTests(WebApplicationFactory<Startup> webApplicationFactory)
        {
            this.webApplicationFactory = webApplicationFactory;
        }

        [Trait(nameof(Category), nameof(Category.Integration))]
        [Fact(DisplayName = "Should Logged In User")]
        public async Task ShouldGetDefaultUser()
        {
            var client = webApplicationFactory.CreateClient();

            var adminOptions =
                webApplicationFactory.Services.GetRequiredService<IOptions<AdminOptions>>().Value;

            string loginUrl = "api/auth/login";
            string userUrl = "api/user";

            var login = new Login { UserName = adminOptions.UserName, Password = adminOptions.Password };
            var jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            var userResponse = await client.PostAsJsonAsync(loginUrl, login);
            var userJson = await userResponse.Content.ReadAsStringAsync();
            var user = JsonSerializer.Deserialize<User>(userJson, jsonOptions);

            Assert.NotNull(user);

            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", user.Token);

            var userWithAuthResponse = await client.GetFromJsonAsync<User>(userUrl, jsonOptions);

            userWithAuthResponse.Should().NotBeNull()
                .And.Match<User>(response =>
                    userWithAuthResponse.FirstName == user.FirstName &&
                    userWithAuthResponse.LastName == user.LastName &&
                    userWithAuthResponse.UserName == user.UserName &&
                    userWithAuthResponse.Email == user.Email);
        }
    }
}
