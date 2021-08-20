using FluentAssertions;
using Integration.Utility;
using System.Threading.Tasks;
using Xunit;

namespace Integration.Server.Services
{
    public class UserServiceTests
    {
        private IUserService userService;

        public UserServiceTests()
        {
            userService = new UserService();
        }

        [Trait(nameof(Category), nameof(Category.Unit))]
        [Fact(DisplayName = "Should generate random user")]
        public async Task ShouldGenerateRandomUser()
        {
            var user = await userService.GetDefaultUser();

            user.Should().NotBeNull();
        }
    }
}
