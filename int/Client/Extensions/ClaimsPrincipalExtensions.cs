using Integration.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Integration.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static User ToUser(this ClaimsPrincipal claimsPrincipal) =>
            new()
            {
                FirstName = claimsPrincipal.FindFirst(ClaimTypes.GivenName.ToString()).Value,
                LastName = claimsPrincipal.FindFirst(ClaimTypes.Surname.ToString()).Value,
                UserName = claimsPrincipal.FindFirst(ClaimTypes.Name.ToString()).Value,
                Email = claimsPrincipal.FindFirst(ClaimTypes.Email.ToString()).Value
            };

        public static ClaimsPrincipal ToClaimsPrincipal(this User user) =>
            new(new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.GivenName, user.FirstName),
                    new Claim(ClaimTypes.Surname, user.LastName),
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Email, user.Email),
                }));
    }
}
