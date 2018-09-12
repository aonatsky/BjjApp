using System;
using System.Collections.Generic;
using System.Linq.Expressions;
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
        /// Returns users by query
        /// </summary>
        /// <param name="predicate">Query</param>
        /// <returns></returns>
        Task<IEnumerable<User>> GetUsersAsync(Expression<Func<User, bool>> predicate);

        /// <summary>
        /// Declines team membership by user Id.
        /// </summary>
        /// <param name="teamId">Team Id.</param>
        /// <param name="userId">User Id.</param>
        /// <returns></returns>
        Task DeclineTeamMembershipAsync(Guid teamId, string userId);

        /// <summary>
        /// Approves team membership by user Id.
        /// </summary>
        /// <param name="teamId">Team Id.</param>
        /// <param name="userId">User Id.</param>
        /// <returns></returns>
        Task ApproveTeamMembershipAsync(Guid teamId, string userId);

        /// <summary>
        /// Returns user role.
        /// </summary>
        /// <param name="userId">User id.</param>
        /// <returns></returns>
        Task<List<string>> GetUserRolesAsync(User user);

        /// <summary>
        /// Returns is user admin async.
        /// </summary>
        /// <param name="userId">User id.</param>
        /// <returns></returns>
        Task<bool> IsAdminAsync(User user);

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
        Task CreateParticipantUserAsync(UserModelRegistration model);

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
        /// <param name="roles">Roles.</param>
        /// <returns></returns>
        Task CreateUserAsync(UserModelRegistration model, List<string> roles);

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
        /// <param name="roles">Roles.</param>
        /// <param name="password">Password.</param>
        /// <returns></returns>
        Task CreateUserWithRoleAsync(User user, List<string> roles, string password);
        
        /// <summary>
        /// Sets team for user
        /// </summary>
        /// <param name="teamId">Team Id.</param>
        /// <param name="userId">User Id.</param>
        Task SetTeamForUserAsync(Guid teamId, string userId, bool isOwner = false);
    }
}