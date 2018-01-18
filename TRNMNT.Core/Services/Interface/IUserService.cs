using System.Security.Claims;
using System.Threading.Tasks;
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
    }
}
