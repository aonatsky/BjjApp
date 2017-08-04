using System.Threading.Tasks;

namespace TRNMNT.Web.Core.Services.Authentication
{
    public interface IAuthenticationService
    {
        Task<string> GetTokenAsync();
        Task<string> GetTokenAsync(string login, string password);
        Task CreateAccountAsync(string login, string password);
    }
}
