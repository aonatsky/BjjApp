using System.Threading.Tasks;
using TRNMNT.Core.Model.Result;

namespace TRNMNT.Core.Services.Interface
{
    public interface IAuthenticationService
    {
        /// <summary>
        /// Gets the token asynchronous.
        /// </summary>
        /// <returns></returns>
        Task<string> GetTokenAsync();

        /// <summary>
        /// Gets the token asynchronous.
        /// </summary>
        /// <param name="login">The login.</param>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        Task<string> GetTokenAsync(string login, string password);

        /// <summary>
        /// Creates the participant user asynchronous.
        /// </summary>
        /// <param name="login">The login.</param>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        Task<UserRegistrationResult> CreateParticipantUserAsync(string login, string password);

        /// <summary>
        /// Creates the owner user asynchronous.
        /// </summary>
        /// <param name="login">The login.</param>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        Task<UserRegistrationResult> CreateOwnerUserAsync(string login, string password);
    }
}
