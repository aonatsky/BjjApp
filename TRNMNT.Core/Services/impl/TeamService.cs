using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TRNMNT.Core.Model;
using TRNMNT.Core.Model.Team;
using TRNMNT.Core.Services.Interface;
using TRNMNT.Data.Entities;
using TRNMNT.Data.Repositories;

namespace TRNMNT.Core.Services.Impl
{
    public class TeamService : ITeamService
    {
        #region Dependencies

        private readonly IRepository<Team> _repository;

        #endregion

        #region .ctor

        public TeamService(IRepository<Team> repository)
        {
            _repository = repository;
        }

        #endregion

        #region Public Methods

        public async Task ApproveEntityAsync(Guid entityId, Guid orderId)
        {
            var team = await _repository.GetByIDAsync(entityId);
            if (team != null)
            {
                team.IsApproved = true;
                team.OrderId = orderId;
            }
        }

        public async Task<Team> GetTeamByNameAsync(string name)
        {
            return await _repository.GetAll().Where(t => t.Name == name).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<TeamModel>> GetTeamsAsync()
        {
            return await _repository.GetAll().Select(t => new TeamModel
            {
                TeamId = t.TeamId.ToString(),
                Name = t.Name,
                Description = t.Description
            }).ToListAsync();
        }

        public async Task<IEnumerable<TeamModelBase>> GetTeamsAsync(Guid eventId)
        {
            return await _repository.GetAll().Where(t => t.Participants.Any(p => p.EventId == eventId)).Select(t => new TeamModelBase
            {
                TeamId = t.TeamId.ToString(),
                Name = t.Name
            }).ToListAsync();
        }

        #endregion
    }
}
