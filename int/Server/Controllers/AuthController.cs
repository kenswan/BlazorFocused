using Integration.Sdk.Models;
using Integration.Server.Extensions;
using Integration.Server.Models;
using Integration.Server.Providers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Integration.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ITokenProvider tokenProvider;
        private readonly UserManager<IntegrationUser> userManager;

        public AuthController(ITokenProvider tokenProvider, UserManager<IntegrationUser> userManager)
        {
            this.tokenProvider = tokenProvider;
            this.userManager = userManager;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<User>> Login([FromBody] Login login)
        {
            var identityUser = await userManager.FindByNameAsync(login.UserName);

            if (await userManager.CheckPasswordAsync(identityUser, login.Password))
            {
                var user = identityUser.ToUser();
                user.Token = tokenProvider.GenerateUserToken(identityUser);

                return Ok(user);
            }
            else
            {
                return Unauthorized();
            }
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult<User>> Register([FromBody] Register register)
        {
            var integrationUser = register.ToIntegrationUser();

            var result = await userManager.CreateAsync(integrationUser, register.Password);

            return (result.Succeeded) ? Ok(integrationUser.ToUser()) : BadRequest();
        }
    }
}
