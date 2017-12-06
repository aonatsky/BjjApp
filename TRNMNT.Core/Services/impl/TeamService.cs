﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TRNMNT.Core.Model;
using TRNMNT.Core.Services.Interface;
using TRNMNT.Data.Entities;
using TRNMNT.Data.Repositories;

namespace TRNMNT.Core.Services.Impl
{
    public class TeamService : ITeamService
    {
        private IRepository<Team> repository;

        public TeamService(IRepository<Team> repository)
        {
            this.repository = repository;
        }

        public async Task ApproveEntityAsync(Guid entityId, Guid orderId)
        {
            var team = await repository.GetByIDAsync(entityId);
            if (team != null)
            {
                team.IsApproved = true;
                team.OrderId = orderId;
            }
        }

        public async Task<Team> GetTeamByNameAsync(string name)
        {
            return await repository.GetAll().Where(t => t.Name == name).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<TeamModel>> GetTeams()
        {
            return (await repository.GetAll().ToListAsync()).Select(t => new TeamModel { TeamId = t.TeamId.ToString(), Name = t.Name, Description = t.Description });
        }
    }
}
