using System.Collections.Generic;
using System.Threading.Tasks;
using TRNMNT.Core.Model;
using TRNMNT.Data.Entities;

namespace TRNMNT.Core.Services.Interface
{
    public interface ITeamService : IPaidEntityService
    {
        Task<Team> GetTeamByNameAsync(string name);
        Task<IEnumerable<TeamModel>> GetTeams();
    }
}
