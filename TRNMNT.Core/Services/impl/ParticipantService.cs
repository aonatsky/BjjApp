using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TRNMNT.Core.Model.Participant;
using TRNMNT.Data.Entities;
using TRNMNT.Data.Repositories;
using TRNMNT.Data.Context;
using TRNMNT.Core.Const;
using TRNMNT.Core.Model.Interface;
using TRNMNT.Core.Model;

namespace TRNMNT.Core.Services
{
    public class ParticipantService : IParticipantService
    {
        private readonly IRepository<Participant> _repository;

        public ParticipantService(IRepository<Participant> repository)
        {
            _repository = repository;
        }

        public async Task<bool> IsParticipantExistsAsync(ParticipantModelBase model, Guid eventId)
        {
            return await _repository.GetAll().AnyAsync(p =>
             p.EventId == eventId
             && p.FirstName == model.FirstName
             && p.LastName == model.LastName
             && p.DateOfBirth == model.DateOfBirth
             );
        }

        public void AddParticipant(Participant participant)
        {
            _repository.Add(participant);
        }

        public Participant CreatePaticipant(ParticipantRegistrationModel model, Guid eventId)
        {
            return new Participant()
            {
                ParticipantId = Guid.NewGuid(),
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
                IsActive = true,
                IsApproved = false,
                UpdateTS = DateTime.UtcNow,
            };
        }

        public async  Task ApproveEntityAsync(Guid entityId, Guid orderId)
        {
            var participant = await _repository.GetByIDAsync(entityId);
            if (participant != null)
            {
                participant.IsApproved = true;
                participant.OrderId = orderId;
                _repository.Update(participant);
            }
        }

        public async Task<IPagedList<ParticipantTableModel>> GetFilteredParticipantsAsync(Guid federationId, ParticipantFilterModel filter)
        {
            var size = DefaultValues.DefaultPageSize;
            var allParticipants = _repository.GetAll(p => p.EventId == filter.EventId);
            if (filter.CategoryId != null)
            {
                allParticipants = allParticipants.Where(p => p.CategoryId == filter.CategoryId);
            }
            if (filter.WeightDivisionId != null)
            {
                allParticipants = allParticipants.Where(p => p.WeightDivisionId == filter.WeightDivisionId);
            }
            var totalCount = await allParticipants.CountAsync();
            allParticipants = allParticipants.Skip(size * filter.PageIndex).Take(size);
            var list = await allParticipants.Select(p => new ParticipantTableModel
            {
                FirstName = p.FirstName,
                LastName = p.LastName,
                DateOfBirth = p.DateOfBirth,
                UserId = p.UserId,
                TeamName = p.Team.Name,
                CategoryName = p.Category.Name,
                WeightDivisionName = p.WeightDivision.Name,
                IsMember = p.Team.FederationId == federationId
            }).ToListAsync();

            return new PagedList<ParticipantTableModel>(list, filter.PageIndex, size, totalCount);
        }
    }


}







