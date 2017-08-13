using System.Threading.Tasks;
using TRNMNT.Core.Model.Result;

namespace TRNMNT.Web.Core.Services.Authentication
{
    public interface IAuthenticationService
    {
        Task<string> GetTokenAsync();
        Task<string> GetTokenAsync(string login, string password);
        Task<UserRegistrationResult> CreateParticipantUserAsync(string login, string password);
        Task <UserRegistrationResult> CreateOwnerUserAsync(string login, string password);
    }
}
