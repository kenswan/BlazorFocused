using Integration.Server.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Integration.Server.Providers
{
    public class TokenProvider : ITokenProvider
    {
        private readonly SecurityOptions securityOptions;

        public TokenProvider(IOptions<SecurityOptions> securityOptions)
        {
            this.securityOptions = securityOptions.Value ??
                throw new ArgumentNullException(nameof(securityOptions));
        }

        public TokenProvider(IConfiguration configuration)
        {
            securityOptions = new();

            configuration.GetSection(nameof(SecurityOptions)).Bind(securityOptions);
        }

        public string GenerateUserToken(IntegrationUser identityUser)
        {
            var jwt = new JwtSecurityToken(
                    securityOptions.Issuer,
                    securityOptions.Audience,
                    new[] { new Claim(ClaimTypes.NameIdentifier, identityUser.Id.ToString()) },
                    null,
                    null,
                    new SigningCredentials(GetSigningKey(), SecurityAlgorithms.HmacSha256));

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }

        public TokenValidationParameters GetTokenValidationParameters()
        {
            return new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = securityOptions.Issuer,

                ValidateAudience = true,
                ValidAudience = securityOptions.Audience,

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = GetSigningKey(),

                RequireExpirationTime = false,
            };
        }

        public Guid GetUserIdFromToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            tokenHandler.ValidateToken(token, GetTokenValidationParameters(), out SecurityToken validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;

            var userId = jwtToken.Claims.First(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

            if (Guid.TryParse(userId, out Guid actualId))
            {
                return actualId;
            }
            else
            {
                return Guid.Empty;
            }
        }

        private SymmetricSecurityKey GetSigningKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(securityOptions.Key));
        }
    }
}
