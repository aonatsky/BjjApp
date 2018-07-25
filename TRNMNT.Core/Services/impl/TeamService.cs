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

        public async Task<IEnumerable<TeamModelBase>> GetTeamsAsync(Guid eventId)
        {
            return await _repository.GetAll().Where(t => t.Participants.Any(p => p.EventId == eventId)).Select(t => new TeamModelBase
            {
                TeamId = t.TeamId.ToString(),
                    Name = t.Name
            }).ToListAsync();
        }

        public async Task<PaymentDataModel> ProcessTeamRegistrationAsync(Guid federationId, TeamModelFull model, string callbackUrl, string redirectUrl, User user)
        {
            if (await GetTeamByNameAsync(model.Name) != null)
            {
                throw new BusinessException("REGISTRATION_TO_EVENT.PARTICIPANT_IS_ALREADY_REGISTERED");
            }
            var teamId = Guid.NewGuid();
            var priceModel = await _federationService.GetTeamRegistrationPriceAsync(federationId);
            var order = _orderService.AddNewOrder(OrderTypeEnum.TeamRegistration, priceModel.Amount, priceModel.Currency, teamId.ToString(), user.Id);
            AddTeam(model, teamId, order.OrderId, federationId, user.Id);
            return _paymentService.GetPaymentDataModel(order, callbackUrl, redirectUrl);
        }

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

        #endregion
    }
}