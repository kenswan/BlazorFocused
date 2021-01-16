using System.Threading.Tasks;
using BlazorFocused.Integration.Shared.Models;
using Bogus;

namespace BlazorFocused.Integration.Server.Services
{
    public class UserService : IUserService
    {
        public UserService() { }

        public ValueTask<User> GetDefaultUser() =>
            new ValueTask<User>(GetRandomUser());

        private User GetRandomUser() =>
            new Faker<User>()
                .RuleFor(user => user.FirstName, fake => fake.Name.FirstName())
                .RuleFor(user => user.LastName, fake => fake.Name.LastName())
                .RuleFor(user => user.Email, fake => fake.Internet.Email())
                .RuleFor(user => user.Email, fake => fake.Internet.UserName())
                .Generate();
    }
}
