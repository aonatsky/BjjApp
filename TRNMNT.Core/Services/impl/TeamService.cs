using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TRNMNT.Core.Const;
using TRNMNT.Core.Enum;
using TRNMNT.Core.Helpers.Exceptions;
using TRNMNT.Core.Model;
using TRNMNT.Core.Model.Team;
using TRNMNT.Core.Model.User;
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
        private readonly IFederationMembershipService _federationMembershipService;
        private readonly IParticipantService _participantService;
        private readonly IPaymentService _paymentService;
        private readonly IUserService _userService;
        #endregion

        #region .ctor

        public TeamService(IRepository<Team> repository,
            IOrderService orderService,
            IFederationService federationService,
            IPaymentService paymentService,
            IUserService userService,
            IFederationMembershipService federationMembershipService,
            IParticipantService participantService
        )
        {
            _paymentService = paymentService;
            _userService = userService;
            _federationMembershipService = federationMembershipService;
            _participantService = participantService;
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

        public async Task<IEnumerable<TeamModelBase>> GetTeamsForEventAsync(Guid federationId)
        {
            var teams = await _repository.GetAll().Where(t => t.FederationId == federationId && t.FederationApprovalStatus != ApprovalStatus.Declined).ToListAsync();
            await CheckTeamStatusAsync(teams);
            return teams.Select(t => new TeamModelBase
            {
                TeamId = t.TeamId.ToString(),
                    Name = t.Name,
                    ApprovalStatus = t.ApprovalStatus,
                    FederationApprovalStatus = t.FederationApprovalStatus
            });
        }

        public async Task DeclineTeam(Guid teamId)
        {
            var team = await _repository.GetByIDAsync(teamId);
            if (team != null)
            {
                team.FederationApprovalStatus = ApprovalStatus.Declined;
                _repository.Update(team);
            }
        }

        public async Task ApproveTeam(Guid teamId)
        {
            var team = await _repository.GetByIDAsync(teamId);
            if (team != null)
            {
                team.FederationApprovalStatus = ApprovalStatus.Approved;
                _repository.Update(team);
            }
        }

        public async Task<IEnumerable<TeamModelFull>> GetTeamsForAdminAsync(Guid federationId)
        {
            var teams = await _repository.GetAll().Where(t => t.FederationId == federationId).ToListAsync();
            await CheckTeamStatusAsync(teams);
            return teams.Select(t => new TeamModelFull
            {
                TeamId = t.TeamId.ToString(),
                    Name = t.Name,
                    ContactEmail = t.ContactEmail,
                    ContactPhone = t.ContactPhone,
                    Description = t.Description,
                    ContactName = t.ContactName,
                    ApprovalStatus = ApprovalStatus.GetTranslationKey(t.ApprovalStatus),
                    FederationApprovalStatus = t.FederationApprovalStatus
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
            await _userService.SetTeamForUserAsync(teamId, user.Id, true);
            return _paymentService.GetPaymentDataModel(order, callbackUrl, redirectUrl);
        }

        public async Task<IEnumerable<UserModelAthlete>> GetAthletesAsync(Guid teamId)
        {
            var result = new List<UserModelAthlete>();
            var users = await GetTeamUsersAsync(teamId);
            users.ToList().ForEach(async u =>
            {
                result.Append(new UserModelAthlete()
                {
                    FirstName = u.FirstName,
                        LastName = u.LastName,
                        DateOfBirth = u.DateOfBirth,
                        Email = u.Email,
                        UserId = u.Id,
                        TeamMembershipApprovalStatus = u.TeamMembershipApprovalStatus,
                });
            });
            return result;
        }

        public async Task<IEnumerable<UserModelAthlete>> GetAthletesForParticipationAsync(Guid teamId, Guid eventId, Guid federationId)
        {
            var result = new List<UserModelAthlete>();
            var users = await GetTeamUsersAsync(teamId);
            users.ToList().ForEach(async u =>
            {
                result.Append(new UserModelAthlete()
                {
                    FirstName = u.FirstName,
                        LastName = u.LastName,
                        DateOfBirth = u.DateOfBirth,
                        Email = u.Email,
                        UserId = u.Id,
                        TeamMembershipApprovalStatus = u.TeamMembershipApprovalStatus,
                        IsFederationMember = await _federationMembershipService.IsFederationMemberAsync(federationId, u.Id),
                        IsParticipant = await _participantService.IsParticipantExistsAsync(u.Id, eventId)
                });
            });
            return result;
        }

        public async Task<UserModelAthlete> GetAthlete(User user)
        {
            var model = new UserModelAthlete()
            {
                UserId = user.Id,
                TeamId = user.TeamId,
                TeamMembershipApprovalStatus = user.TeamMembershipApprovalStatus,
                FirstName = user.FirstName,
                LastName = user.LastName,
                DateOfBirth = user.DateOfBirth,
                Email = user.Email

            };
            if (user.TeamId.HasValue)
            {
                var team = await _repository.GetByIDAsync(user.TeamId.Value);
                model.TeamName = team != null ? team.Name : null;
            }
            return model;

        }

        public async Task<Team> GetTeamForOwnerAsync(string ownerId)
        {
            return await _repository.FirstOrDefaultAsync(t => t.OwnerId == ownerId);
        }

        #endregion

        #region private methods

        private void AddTeam(TeamModelFull model, Guid teamId, Guid orderId, Guid federationId, string userId)
        {
            var team = new Team()
            {
                TeamId = teamId,
                ApprovalStatus = ApprovalStatus.Pending,
                FederationApprovalStatus = ApprovalStatus.Pending,
                OrderId = orderId,
                ContactEmail = model.ContactEmail,
                ContactName = model.ContactName,
                ContactPhone = model.ContactPhone,
                OwnerId = userId,
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

        private async Task<IEnumerable<User>> GetTeamUsersAsync(Guid teamId)
        {
            var team = await _repository.GetAll().FirstOrDefaultAsync(t => t.TeamId == teamId);
            if (team != null)
            {
                return await _userService.GetUsersAsync(u => u.TeamId.HasValue && (u.TeamId == team.TeamId));
            }
            return new List<User>();
        }

        #endregion

    }
}