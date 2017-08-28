using System.Collections.Generic;
using System.Threading.Tasks;
using TRNMNT.Core.Model;
using TRNMNT.Data.Entities;

namespace TRNMNT.Core.Services
{
    public interface ITeamService
    {
        Task<Team> GetTeamByNameAsync(string name);
        Task<IEnumerable<TeamModel>> GetTeams();
    }
}
