using System.Security.Claims;
using System.Threading.Tasks;
using TRNMNT.Core.Model.Result;
using TRNMNT.Core.Model.User;
using TRNMNT.Data.Entities;

namespace TRNMNT.Core.Services.Interface
{
    public interface IUserService
    {
        /// <summary>
        /// Gets the user asynchronous.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        Task<User> GetUserAsync(string userId);

        /// <summary>
        /// Returns user role.
        /// </summary>
        /// <param name="userId">User id.</param>
        /// <returns></returns>
        Task<string> GetUserRoleAsync(User user);

        /// <summary>
        /// Gets the user asynchronous.
        /// </summary>
        /// <param name="claims">The claims.</param>
        /// <returns></returns>
        Task<User> GetUserAsync(ClaimsPrincipal claims);

        /// <summary>
        /// Backdoors the user creation.
        /// </summary>
        /// <param name="secretUserCreationModel">The secret user creation model.</param>
        /// <returns></returns>
        Task BackdoorUserCreation(SecretUserCreationModel secretUserCreationModel);

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
        /// Adds user with role
        /// </summary>
        /// <param name="user">User object.</param>
        /// <param name="role">Role.</param>
        /// <param name="password">Password.</param>
        /// <returns></returns>
        Task<UserRegistrationResult> CreateUserWithRoleAsync(User user, string role, string password);
    }
}