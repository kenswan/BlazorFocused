using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Integration.Server.Services;
using Integration.Shared.Models;

namespace Integration.Server.Controllers
{
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly IUserService userService;

        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpGet]
        public async ValueTask<User> GetDefaultUser() =>
            await userService.GetDefaultUser();
    }
}
