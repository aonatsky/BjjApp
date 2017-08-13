using System.Threading.Tasks;
using TRNMNT.Data.Entities;

namespace TRNMNT.Core.Services
{
    public interface ITeamService
    {
        Task<Team> GetTeamByNameAsync(string name);
    }
}
