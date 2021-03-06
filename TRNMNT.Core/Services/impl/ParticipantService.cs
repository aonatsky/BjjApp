﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TRNMNT.Core.Const;
using TRNMNT.Core.Enum;
using TRNMNT.Core.Helpers;
using TRNMNT.Core.Helpers.Exceptions;
using TRNMNT.Core.Model;
using TRNMNT.Core.Model.Interface;
using TRNMNT.Core.Model.Participant;
using TRNMNT.Core.Services.Interface;
using TRNMNT.Data.Entities;
using TRNMNT.Data.Repositories;

namespace TRNMNT.Core.Services.Impl
{
    public class ParticipantService : IParticipantService
    {
        #region Dependencies

        private readonly IRepository<Participant> _repository;
        private readonly IOrderService _orderService;
        private readonly IPaymentService _paymentService;
        private readonly IEventService _eventService;
        private readonly IFederationMembershipService _federationMembershipService;

        #endregion

        #region .ctor
        private readonly IPaymentService paymentService;

        public ParticipantService(IRepository<Participant> repository, IOrderService orderService, IPaymentService paymentService, IEventService eventService, IFederationMembershipService federationMembershipService)
        {
            _paymentService = paymentService;
            _repository = repository;
            _orderService = orderService;
            _eventService = eventService;
            this._federationMembershipService = federationMembershipService;
        }

        #endregion

        #region Public Methods

        public async Task<List<ParticipantEventModel>> GetUserParticipantsAsync(User user, bool isTeamOwner)
        {
            List<Participant> participants;
            List<ParticipantEventModel> participantModels = new List<ParticipantEventModel>();

            if (isTeamOwner)
            {
                participants = await _repository.GetAll(p => p.TeamId == user.TeamId && p.IsActive).ToListAsync();
            }
            else
            {
                participants = await _repository.GetAll(p => p.UserId == user.Id).ToListAsync();
            }

            if (participants.Any())
            {
                var groupsByEvent = participants.GroupBy(p => p.EventId).Select(g => new { EventId = g.Key, Participants = g.Select(p => p) });
                foreach (var group in groupsByEvent)
                {
                    var _event = await _eventService.GetEventAsync(group.EventId);
                    participantModels.AddRange(group.Participants.Select(p => new ParticipantEventModel()
                    {
                        CategoryId = p.CategoryId,
                            WeightDivisionId = p.WeightDivisionId,
                            DateOfBirth = p.DateOfBirth,
                            EventDate = _event.EventDate,
                            Email = p.Email,
                            EventName = _event.Title,
                            FirstName = p.FirstName,
                            LastName = p.LastName,
                            EventId = p.EventId,
                            TeamId = p.TeamId,
                            PhoneNumber = p.PhoneNumber,
                            ParticipantId = p.ParticipantId,
                            CorrectionsAllowed = _eventService.IsCorrectionAllowed(_event),
                            UserId = p.UserId
                    }));
                }
            }
            return participantModels;

        }

        public async Task<bool> IsParticipantExistsAsync(ParticipantModelBase model, Guid eventId)
        {
            return await _repository.GetAll(p =>
                p.IsActive &&
                p.EventId == eventId &&
                p.FirstName == model.FirstName &&
                p.LastName == model.LastName &&
                p.DateOfBirth == model.DateOfBirth).AnyAsync();
        }

        public async Task<bool> IsParticipantExistsAsync(string userId, Guid eventId)
        {
            return await _repository.GetAll(p =>
                p.IsActive &&
                p.EventId == eventId &&
                p.UserId == userId).AnyAsync();
        }

        public void AddParticipant(ParticipantRegistrationModel model, Guid eventId, Guid orderId, Guid participantId, bool isFederationMember)
        {
            var participant = new Participant()
            {
                ParticipantId = participantId,
                FirstName = model.FirstName,
                LastName = model.LastName,
                TeamId = model.TeamId,
                DateOfBirth = model.DateOfBirth,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                CategoryId = model.CategoryId,
                WeightDivisionId = model.WeightDivisionId,
                EventId = eventId,
                UserId = model.UserId,
                WeightInStatus = ApprovalStatus.Pending,
                ApprovalStatus = ApprovalStatus.Pending,
                IsActive = true,
                UpdateTS = DateTime.UtcNow,
                OrderId = orderId
            };
            _repository.Add(participant);
        }

        public async Task ApproveEntityAsync(Guid entityId, Guid orderId)
        {
            var participant = await _repository.GetByIDAsync(entityId);
            if (participant != null)
            {
                participant.ApprovalStatus = ApprovalStatus.Approved;
                participant.OrderId = orderId;
                _repository.Update(participant);
            }
        }

        public async Task<Participant> UpdateParticipantAsync(ParticipantTableModel participantModel)
        {
            if (participantModel == null)
            {
                throw new ArgumentNullException(nameof(participantModel));
            }
            var participant = await _repository.GetAll()
                .FirstOrDefaultAsync(p => p.ParticipantId == participantModel.ParticipantId);
            if (participant == null)
            {
                // todo change to custom exception
                throw new BusinessException($"Participant with id {participantModel.ParticipantId} not found");
            }
            participant.FirstName = participantModel.FirstName;
            participant.LastName = participantModel.LastName;
            participant.TeamId = participantModel.TeamId;
            participant.DateOfBirth = participantModel.DateOfBirth;
            participant.CategoryId = participantModel.CategoryId;
            participant.WeightDivisionId = participantModel.WeightDivisionId;
            participant.IsMember = participantModel.IsMember;
            participant.UpdateTS = DateTime.UtcNow;

            _repository.Update(participant);
            return participant;
        }

        public async Task<IEnumerable<Participant>> GetParticipantsByWeightDivisionAsync(Guid weightDivisionId, bool includeTeam = false, bool showInactive = true, bool onlyPayed = false)
        {
            var query = _repository.GetAll(p =>
                p.WeightDivisionId == weightDivisionId || p.AbsoluteWeightDivisionId == weightDivisionId);
            if (!showInactive)
            {
                query = query.Where(p => p.IsActive);
            }
            if (onlyPayed)
            {
                query = query.Where(p => p.ApprovalStatus == ApprovalStatus.Approved);
            }
            if (includeTeam)
            {
                query = query.Include(p => p.Team);
            }
            return await query.ToListAsync();
        }

        public async Task AddParticipantsToAbsoluteWeightDivisionAsync(Guid[] participantsIds, Guid categoryId,
            Guid absoluteWeightDivisionId)
        {
            var previousParticipants = await _repository.GetAll(p =>
                p.CategoryId == categoryId && !participantsIds.Contains(p.ParticipantId) &&
                p.AbsoluteWeightDivisionId == absoluteWeightDivisionId).ToListAsync();
            foreach (var previousParticipant in previousParticipants)
            {
                previousParticipant.AbsoluteWeightDivisionId = null;
                _repository.Update(previousParticipant);
            }
            var participants = await _repository.GetAll(p => participantsIds.Contains(p.ParticipantId)).ToListAsync();
            foreach (var participant in participants)
            {
                participant.AbsoluteWeightDivisionId = absoluteWeightDivisionId;
                _repository.Update(participant);
            }
        }

        public async Task DeleteParticipantAsync(Guid participantId)
        {
            var participant = await _repository.GetByIDAsync(participantId);
            if (participant == null)
            {
                // todo change to custom exception
                throw new Exception($"Participant with id {participantId} not found");
            }
            _repository.Delete(participant);
        }

        public async Task<IPagedList<ParticipantTableModel>> GetFilteredParticipantsAsync(ParticipantFilterModel filter)
        {
            var size = DefaultValues.DefaultPageSize;
            var participantsQuery = _repository.GetAll().Where(p => p.EventId == filter.EventId);
            if (filter.CategoryId != null)
            {
                participantsQuery = participantsQuery.Where(p => p.CategoryId == filter.CategoryId);
            }
            if (filter.WeightDivisionId != null)
            {
                participantsQuery = participantsQuery.Where(p => p.WeightDivisionId == filter.WeightDivisionId);
            }
            if (filter.IsMembersOnly)
            {
                participantsQuery = participantsQuery.Where(p => p.IsMember);
            }
            var totalCount = await participantsQuery.CountAsync();

            participantsQuery = SortParticipants(participantsQuery, filter.SortField, filter.SortDirection);

            participantsQuery = participantsQuery.Skip(size * filter.PageIndex).Take(size);
            var participants = await participantsQuery.ToListAsync();
            await CheckParticipantStatus(participants);

            var anonymList = await participantsQuery.Select(p => new ParticipantTableModel
            {
                ParticipantId = p.ParticipantId,
                    FirstName = p.FirstName,
                    LastName = p.LastName,
                    DateOfBirth = p.DateOfBirth,
                    UserId = p.UserId,
                    TeamName = p.Team.Name,
                    TeamId = p.TeamId,
                    CategoryName = p.Category.Name,
                    CategoryId = p.CategoryId,
                    WeightDivisionName = p.WeightDivision.Name,
                    WeightDivisionId = p.WeightDivisionId,
                    IsMember = p.IsMember,
                    WeightInStatus = p.WeightInStatus,
                    ApprovalStatus = p.ApprovalStatus
            }).ToListAsync();

            return new PagedList<ParticipantTableModel>(anonymList, filter.PageIndex, size, totalCount);
        }

        public async Task<IEnumerable<Participant>> GetParticipantsInAbsoluteDivisionByCategoryAsync(Guid categoryId)
        {
            return await _repository.GetAll(p => p.AbsoluteWeightDivisionId != null).ToListAsync();
        }

        public async Task<PaymentDataModel> ProcessParticipantRegistrationAsync(Guid eventId, Guid federationId, ParticipantRegistrationModel model, string callbackUrl, string redirectUrl, User user)
        {
            if (await IsParticipantExistsAsync(model, eventId))
            {
                throw new BusinessException("PARTICIPATION.PARTICIPANT_IS_ALREADY_REGISTERED");
            }

            var isFederationMember = await _federationMembershipService.IsFederationMemberAsync(federationId, user.Id);
            if (model.IncludeMembership && isFederationMember)
            {
                model.IncludeMembership = false;
            }

            var orderType = model.IncludeMembership? OrderTypeEnum.EventParticipationAndMembership : OrderTypeEnum.EventParticipation;

            var participantId = Guid.NewGuid();
            var priceModel = await _eventService.GetPriceAsync(eventId, user.Id, model.IncludeMembership);
            var order = _orderService.AddNewOrder(orderType, priceModel.Amount, priceModel.Currency, participantId.ToString(), user.Id);
            AddParticipant(model, eventId, order.OrderId, participantId, isFederationMember || model.IncludeMembership);

            if (model.IncludeMembership)
            {
                _federationMembershipService.AddFederationMembership(user.Id, federationId, order.OrderId, Guid.NewGuid());
            }

            return _paymentService.GetPaymentDataModel(order, callbackUrl, redirectUrl);
        }

        public async Task<PaymentDataModel> ProcessParticipantTeamRegistrationAsync(Guid eventId, Guid federationId, List<ParticipantRegistrationModel> models, string callbackUrl, string redirectUrl, User teamOwner)
        {
            if (!models.Any())
            {
                throw new BusinessException("ERROR.NO_PARTICIPANTS_SELECTED");
            }

            var existingParticipantsUserIds = await IsUsersParticipants(models.Select(m => m.UserId).ToList(), eventId);
            models.RemoveAll(m => existingParticipantsUserIds.Contains(m.UserId));

            var federationMemberships =
                await _federationMembershipService.GetFederationMembershipsForUsersAsync(federationId, models.Select(m => m.UserId).ToList());
            var priceModel = await _eventService.GetTeamPriceAsync(eventId, models);
            var orderType = OrderTypeEnum.TeamEventParticipation;
            var orderReference = $"{models.FirstOrDefault().TeamName}";
            var orderId = Guid.NewGuid();

            foreach (var model in models)
            {
                if (await IsParticipantExistsAsync(model, eventId))
                {
                    break;
                    //throw new BusinessException("PARTICIPATION.PARTICIPANT_IS_ALREADY_REGISTERED");
                }
                var isFederationMember = federationMemberships.Any(fm => fm.UserId == model.UserId);
                var participantId = Guid.NewGuid();
                orderReference += $"{participantId.ToString()}_";
                AddParticipant(model, eventId, orderId, participantId, isFederationMember || model.IncludeMembership);

                if (model.IncludeMembership)
                {
                    _federationMembershipService.AddFederationMembership(model.UserId, federationId, orderId, Guid.NewGuid());
                }
            }
            var order = _orderService.AddNewOrder(orderType, priceModel.Amount, priceModel.Currency, orderReference, teamOwner.Id, orderId);
            return _paymentService.GetPaymentDataModel(order, callbackUrl, redirectUrl);
        }

        public async Task SetWeightInStatus(Guid participantId, string status)
        {
            var participant = await _repository.GetByIDAsync(participantId);

            if (participant == null)
            {
                new BusinessException("ERROR.PARTICIPANT_IS_NOT_FOUND");
            }

            switch (status)
            {
                case ApprovalStatus.Approved:
                    participant.WeightInStatus = ApprovalStatus.Approved;
                    break;
                case ApprovalStatus.Pending:
                    participant.WeightInStatus = ApprovalStatus.Pending;
                    break;
                case ApprovalStatus.Declined:
                    participant.WeightInStatus = ApprovalStatus.Declined;
                    break;
                default:
                    break;
            }
            _repository.Update(participant);
        }

        #endregion

        #region Private Methods
        private IQueryable<Participant> SortParticipants(IQueryable<Participant> allParticipants, ParticpantSortField filterSortField, SortDirectionEnum sortDirection)
        {
            switch (filterSortField)
            {
                case ParticpantSortField.FirstName:
                    allParticipants = allParticipants.OrderByDirection(sortDirection, p => p.FirstName);
                    break;
                case ParticpantSortField.LastName:
                    allParticipants = allParticipants.OrderByDirection(sortDirection, p => p.LastName);
                    break;
                case ParticpantSortField.DateOfBirth:
                    allParticipants = allParticipants.OrderByDirection(sortDirection, p => p.DateOfBirth);
                    break;
                case ParticpantSortField.TeamName:
                    allParticipants = allParticipants.OrderByDirection(sortDirection, p => p.Team.Name);
                    break;
                case ParticpantSortField.CategoryName:
                    allParticipants = allParticipants.OrderByDirection(sortDirection, p => p.Category.Name);
                    break;
                case ParticpantSortField.WeightDivisionName:
                    allParticipants = allParticipants.OrderByDirection(sortDirection, p => p.WeightDivision.Name);
                    break;
                case ParticpantSortField.IsMember:
                    allParticipants = allParticipants.OrderByDirection(sortDirection, p => p.IsMember);
                    break;
                default:
                    allParticipants = allParticipants.OrderByDirection(sortDirection, p => p.FirstName);
                    break;
            }
            return allParticipants;
        }

        private async Task CheckParticipantStatus(List<Participant> participants)
        {
            foreach (var participant in participants)
            {
                if (participant.ApprovalStatus == ApprovalStatus.Pending && participant.OrderId.HasValue)
                {
                    participant.ApprovalStatus = await _orderService.GetApprovalStatus(participant.OrderId.Value);
                    if (participant.ApprovalStatus == ApprovalStatus.PaymentNotFound)
                    {
                        participant.IsActive = false;
                    }
                    _repository.Update(participant);
                }
            }
        }

        private async Task<List<string>> IsUsersParticipants(List<string> userIds, Guid eventId)
        {
            return await _repository.GetAll(p => p.EventId == eventId && !string.IsNullOrEmpty(p.UserId) && userIds.Contains(p.UserId)).Select(p => p.UserId).ToListAsync();
        }

        #endregion
    }
}