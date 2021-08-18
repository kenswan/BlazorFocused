using Integration.Server.Extensions;
using Integration.Server.Models;
using Integration.Server.Providers;
using Integration.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Integration.Server.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ITokenProvider tokenProvider;
        private readonly UserManager<IntegrationUser> userManager;

        public UserController(
            IHttpContextAccessor httpContextAccessor,
            ITokenProvider tokenProvider,
            UserManager<IntegrationUser> userManager)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.tokenProvider = tokenProvider;
            this.userManager = userManager;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<User>> GetUser()
        {
            var token = httpContextAccessor.
                HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (string.IsNullOrWhiteSpace(token))
            {
                return Unauthorized();
            }

            var userId = tokenProvider.GetUserIdFromToken(token);

            if (userId == Guid.Empty)
            {
                return NotFound();
            }

            var user = await userManager.FindByIdAsync(userId.ToString());

            return (user is not null) ? Ok(user.ToUser()) : NotFound();
        }
    }
}
