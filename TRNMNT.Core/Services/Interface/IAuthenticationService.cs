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
        /// Creates the participant user asynchronous.
        /// </summary>
        /// <param name="login">The login.</param>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        Task<UserRegistrationResult> CreateParticipantUserAsync(UserRegistrationModel model);

        /// <summary>
        /// Change users password asynchronously 
        /// </summary>
        /// <param name="oldPassword">Old password.</param>
        /// <param name="newPassword">New password.</param>
        /// <param name="userId">User id.</param>
        /// <returns></returns>
        Task ChangesPasswordAsync(string oldPassword, string newPassword, string userId);

        /// <summary>
        /// Sets user password async.
        /// </summary>
        /// <param name="password"> Password.</param>
        /// <param name="userId">User id.</param>
        /// <returns></returns>
        Task SetPasswordAsync(string password, string userId);

        /// <summary>
        /// Creates user asynchronous.
        /// </summary>
        /// <param name="model">Credentials model.</param>
        /// <param name="role">Role.</param>
        /// <returns></returns>
        Task<UserRegistrationResult> CreateUserAsync(UserRegistrationModel model, string role);

        /// <summary>
        /// Updates user asynchronous.
        /// </summary>
        /// <param name="model">User model.</param>
        /// <returns></returns>
        Task UpdateUserAsync(UserModel model);

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