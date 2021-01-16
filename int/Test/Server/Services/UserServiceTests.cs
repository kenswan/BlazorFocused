using System;
using System.Threading.Tasks;
using BlazorFocused.Integration.Server.Services;
using BlazorFocused.Integration.Test.Utility;
using FluentAssertions;
using Xunit;

namespace BlazorFocused.Integration.Test.Server.Services
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
