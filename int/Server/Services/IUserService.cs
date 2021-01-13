using System.Threading.Tasks;
using BlazorFocused.Integration.Shared.Models;

namespace BlazorFocused.Integration.Server.Services
{
    public interface IUserService
    {
        public ValueTask<User> GetDefaultUser();
    }
}
