using System.Threading.Tasks;
using FluentAssertions;
using Xunit;
using Integration.Server.Services;
using Integration.Test.Utility;

namespace Integration.Test.Server.Services
{
    public class UserServiceTests
    {
        private IUserService userService;

        public UserServiceTests()
        {
            userService = new UserService();
        }

        [Trait(nameof(Category), nameof(Category.Unit))]
        [Fact(DisplayName = "Should genrate random user")]
        public async Task ShouldGenerateRandomUser()
        {
            var user = await userService.GetDefaultUser();

            user.Should().NotBeNull();
        }
    }
}
