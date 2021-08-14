using Bogus;
using Integration.Shared.Models;
using System.Threading.Tasks;

namespace Integration.Server.Services
{
    public class UserService : IUserService
    {
        public UserService() { }

        public ValueTask<User> GetDefaultUser() =>
            new(GetRandomUser());

        private static User GetRandomUser() =>
            new Faker<User>()
                .RuleFor(user => user.FirstName, fake => fake.Name.FirstName())
                .RuleFor(user => user.LastName, fake => fake.Name.LastName())
                .RuleFor(user => user.Email, fake => fake.Internet.Email())
                .RuleFor(user => user.Email, fake => fake.Internet.UserName())
                .Generate();
    }
}
