using Integration.Server.Models;
using Microsoft.IdentityModel.Tokens;
using System;

namespace Integration.Server.Providers
{
    public interface ITokenProvider
    {
        string GenerateUserToken(IntegrationUser identityUser);

        TokenValidationParameters GetTokenValidationParameters();

        Guid GetUserIdFromToken(string token);
    }
}
