using Integration.Sdk.Models;
using System.Threading.Tasks;

namespace Integration.Server.Services
{
    public interface IUserService
    {
        public ValueTask<User> GetDefaultUser();
    }
}
