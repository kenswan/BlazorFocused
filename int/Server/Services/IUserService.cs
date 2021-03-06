using System.Threading.Tasks;
using Integration.Shared.Models;

namespace Integration.Server.Services
{
    public interface IUserService
    {
        public ValueTask<User> GetDefaultUser();
    }
}
