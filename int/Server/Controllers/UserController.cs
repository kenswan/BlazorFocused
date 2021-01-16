using System.Threading.Tasks;
using BlazorFocused.Integration.Server.Services;
using BlazorFocused.Integration.Shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace BlazorFocused.Integration.Server.Controllers
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
