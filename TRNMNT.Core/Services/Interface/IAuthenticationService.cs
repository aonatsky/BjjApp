using System.Threading.Tasks;
using TRNMNT.Core.Model;
using TRNMNT.Core.Model.Result;

namespace TRNMNT.Core.Services.Interface
{
    public interface IAuthenticationService
    {
        /// <summary>
        /// Gets the token asynchronous.
        /// </summary>
        /// <returns></returns>
        Task<AuthTokenResult> GetTokenAsync();

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
        Task<UserRegistrationResult> CreateParticipantUserAsync(string login, string password);

        /// <summary>
        /// Creates user asynchronous.
        /// </summary>
        /// <param name="model">Credentials model.</param>
        /// <returns></returns>
        Task<UserRegistrationResult> CreateUserAsync(UserCredentialsModel model);

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
    }
}
