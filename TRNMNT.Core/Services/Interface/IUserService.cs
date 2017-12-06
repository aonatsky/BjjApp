using System.Security.Claims;
using System.Threading.Tasks;
using TRNMNT.Data.Entities;

namespace TRNMNT.Core.Services.Interface
{
    public interface IUserService
    {
        Task<User> GetUserAsync(string userId);
        Task<User> GetUserAsync(ClaimsPrincipal claims);
    }
}
