using System.Threading.Tasks;

namespace TRNMNT.Web.Core.Services.Authentication
{
    public interface IAuthenticationService
    {
        Task<string> GetToken();
        Task<string> GetToken(string login, string password);
    }
}
