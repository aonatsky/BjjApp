using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TRNMNT.Data.Entities;
using TRNMNT.Data.Repositories;

namespace TRNMNT.Core.Services
{
    public class TeamService : ITeamService
    {
        private IRepository<Team> repository;

        public TeamService(IRepository<Team> repository)
        {
            this.repository = repository;
        }

        public async Task<Team> GetTeamByNameAsync(string name)
        {
            return await repository.GetAll().Where(t => t.Name == name).FirstOrDefaultAsync();
        }
    }
}
