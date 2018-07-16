using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TRNMNT.Core.Const;
using TRNMNT.Core.Enum;
using TRNMNT.Core.Helpers;
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

        #endregion

        #region .ctor

        public ParticipantService(IRepository<Participant> repository)
        {
            _repository = repository;
        }

        #endregion

        #region Public Methods

        public async Task<bool> IsParticipantExistsAsync(ParticipantModelBase model, Guid eventId)
        {
            return await _repository.GetAll().AnyAsync(p =>
                p.EventId == eventId &&
                p.FirstName == model.FirstName &&
                p.LastName == model.LastName &&
                p.DateOfBirth == model.DateOfBirth);
        }

        public void AddParticipant(Participant participant)
        {
            _repository.Add(participant);
        }

        public Participant CreatePaticipant(ParticipantRegistrationModel model, Guid eventId)
        {
            var id = Guid.NewGuid();
            return new Participant()
            {
                ParticipantId = id,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    TeamId = Guid.Parse(model.TeamId),
                    DateOfBirth = model.DateOfBirth,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    CategoryId = Guid.Parse(model.CategoryId),
                    WeightDivisionId = Guid.Parse(model.WeightDivisionId),
                    EventId = eventId,
                    UserId = model.UserId,
                    IsDisqualified = true,
                    IsApproved = false,
                    UpdateTS = DateTime.UtcNow,
            };
        }

        public async Task ApproveEntityAsync(Guid entityId, Guid orderId)
        {
            var participant = await _repository.GetByIDAsync(entityId);
            if (participant != null)
            {
                participant.IsApproved = true;
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
                throw new Exception($"Participant with id {participantModel.ParticipantId} not found");
            }
            participant.FirstName = participantModel.FirstName;
            participant.LastName = participantModel.LastName;
            participant.TeamId = participantModel.TeamId;
            participant.DateOfBirth = participantModel.DateOfBirth;
            participant.CategoryId = participantModel.CategoryId;
            participant.WeightDivisionId = participantModel.WeightDivisionId;
            participant.IsMember = participantModel.IsMember;
            participant.IsDisqualified = true;
            participant.UpdateTS = DateTime.UtcNow;

            _repository.Update(participant);
            return participant;
        }

        public async Task<IEnumerable<Participant>> GetParticipantsByWeightDivisionAsync(Guid weightDivisionId, bool includeTeam = false, bool showInactive = true)
        {
            var query = _repository.GetAll(p =>
                p.WeightDivisionId == weightDivisionId || p.AbsoluteWeightDivisionId == weightDivisionId);
            if (!showInactive)
            {
                query = query.Where(p => p.IsActive);
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
            var allParticipants = _repository.GetAll().Where(p => p.EventId == filter.EventId);
            if (filter.CategoryId != null)
            {
                allParticipants = allParticipants.Where(p => p.CategoryId == filter.CategoryId);
            }
            if (filter.WeightDivisionId != null)
            {
                allParticipants = allParticipants.Where(p => p.WeightDivisionId == filter.WeightDivisionId);
            }
            if (filter.IsMembersOnly)
            {
                allParticipants = allParticipants.Where(p => p.IsMember);
            }
            var totalCount = await allParticipants.CountAsync();

            allParticipants = SortParticipants(allParticipants, filter.SortField, filter.SortDirection);

            allParticipants = allParticipants.Skip(size * filter.PageIndex).Take(size);

            var anonimList = await allParticipants.Select(p => new ParticipantTableModel
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
                    IsMember = p.IsMember
            }).ToListAsync();

            return new PagedList<ParticipantTableModel>(anonimList, filter.PageIndex, size, totalCount);
        }

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

        public async Task<IEnumerable<Participant>> GetParticipantsInAbsoluteDivisionByCategoryAsync(Guid categoryId)
        {
            return await _repository.GetAll(p => p.AbsoluteWeightDivisionId != null).ToListAsync();
        }

        #endregion

        #region Private Methods

        #endregion
    }
}