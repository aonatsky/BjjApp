using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using TRNMNT.Core.Services.Interface;
using TRNMNT.Data.Entities;

namespace TRNMNT.Core.Services.Impl
{
    public class UserService : IUserService
    {
        #region Dependencies

        private readonly UserManager<User> _userManager;

        #endregion

        #region .ctor

        public UserService(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        #endregion

        #region Public Methods

        public async Task<User> GetUserAsync(string userId)
        {
            return await _userManager.FindByIdAsync(userId);
        }

        public async Task<User> GetUserAsync(ClaimsPrincipal claims)
        {
            return await _userManager.GetUserAsync(claims);
        }

        #endregion
    }
}
