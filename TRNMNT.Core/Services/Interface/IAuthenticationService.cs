using System.Threading.Tasks;
using TRNMNT.Core.Model;
using TRNMNT.Core.Model.Result;
using TRNMNT.Core.Model.User;
using TRNMNT.Data.Entities;

namespace TRNMNT.Core.Services.Interface
{
    public interface IAuthenticationService
    {
        /// <summary>
        /// Gets the token asynchronous.
        /// </summary>
        /// <param name="login">The login.</param>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        Task<AuthTokenResult> GetTokenAsync(string login, string password);

        /// <summary>
        /// Updates the token asynchronous.
        /// </summary>
        /// <param name="refreshToken">The refresh token.</param>
        /// <returns></returns>
        Task<AuthTokenResult> UpdateTokenAsync(string refreshToken);

        /// <summary>
        /// Returns token for social logged in user.
        /// </summary>
        /// <param name="fbToken"></param>
        /// <returns></returns>
        Task<AuthTokenResult> FacebookLogin(string fbToken);

        /// <summary>
        /// Returns user by userId
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<User> GetUser(string userId);
    }
}