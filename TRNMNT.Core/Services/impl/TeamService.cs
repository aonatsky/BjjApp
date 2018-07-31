using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TRNMNT.Core.Const;
using TRNMNT.Core.Enum;
using TRNMNT.Core.Helpers.Exceptions;
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
        private readonly IOrderService _orderService;
        private readonly IFederationService _federationService;
        private readonly IPaymentService _paymentService;
        #endregion

        #region .ctor

        public TeamService(IRepository<Team> repository, IOrderService orderService, IFederationService federationService, IPaymentService paymentService)
        {
            _paymentService = paymentService;
            _federationService = federationService;
            _repository = repository;
            _orderService = orderService;
        }

        #endregion

        #region Public Methods

        public async Task ApproveEntityAsync(Guid entityId, Guid orderId)
        {
            var team = await _repository.GetByIDAsync(entityId);
            if (team != null)
            {
                team.ApprovalStatus = ApprovalStatus.Approved;
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

        public async Task<IEnumerable<TeamModelBase>> GetTeamsAsync(Guid federationId)
        {
            var teams = await _repository.GetAll().Where(t => t.FederationId == federationId).ToListAsync();
            await CheckTeamStatusAsync(teams);
            return teams.Where(t => t.ApprovalStatus == ApprovalStatus.Approved).Select(t => new TeamModelBase
            {
                TeamId = t.TeamId.ToString(),
                    Name = t.Name
            });
        }

        public async Task<PaymentDataModel> ProcessTeamRegistrationAsync(Guid federationId, TeamModelFull model, string callbackUrl, string redirectUrl, User user)
        {
            if (await IsTeamExistAsync(model.Name))
            {
                throw new BusinessException("TEAM_REGISTRATION_IS_ALREADY_REQUESTED");
            }
            var teamId = Guid.NewGuid();
            var priceModel = await _federationService.GetTeamRegistrationPriceAsync(federationId);
            var order = _orderService.AddNewOrder(OrderTypeEnum.TeamRegistration, priceModel.Amount, priceModel.Currency, teamId.ToString(), user.Id);
            AddTeam(model, teamId, order.OrderId, federationId, user.Id);
            return _paymentService.GetPaymentDataModel(order, callbackUrl, redirectUrl);
        }

        #endregion

        #region private methods

        private void AddTeam(TeamModelFull model, Guid teamId, Guid orderId, Guid federationId, string userId)
        {
            var team = new Team()
            {
                TeamId = teamId,
                ApprovalStatus = ApprovalStatus.Pending,
                OrderId = orderId,
                ContactEmail = model.ContactEmail,
                ContactName = model.ContactName,
                ContactPhone = model.ContactPhone,
                CreateBy = userId,
                CreateTs = DateTime.UtcNow,
                Description = model.Description,
                FederationId = federationId,
                Name = model.Name,
            };
            _repository.Add(team);
        }
        private async Task<bool> IsTeamExistAsync(string name)
        {
            return await _repository.GetAll(t => t.Name.Equals(name, StringComparison.OrdinalIgnoreCase)).AnyAsync();
        }

        private async Task CheckTeamStatusAsync(List<Team> teams)
        {
            foreach (var team in teams)
            {
                if (team.ApprovalStatus == ApprovalStatus.Pending && team.OrderId.HasValue)
                {
                    team.ApprovalStatus = await _orderService.GetApprovalStatus(team.OrderId.Value);
                    _repository.Update(team);
                }
            }
        }

        #endregion

    }
}