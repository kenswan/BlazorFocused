using Integration.Sdk.Models;
using Integration.Server.Models;

namespace Integration.Server.Extensions
{
    public static class UserExtensions
    {
        public static User ToUser(this IntegrationUser integrationUser, string token = default) =>
            new()
            {
                FirstName = integrationUser.FirstName,
                LastName = integrationUser.LastName,
                Email = integrationUser.Email,
                UserName = integrationUser.UserName,
                Token = token
            };

        public static IntegrationUser ToIntegrationUser(this Register register) =>
            new()
            {
                FirstName = register.FirstName,
                LastName = register.LastName,
                UserName = register.UserName,
                Email = register.Email
            };
    }
}
