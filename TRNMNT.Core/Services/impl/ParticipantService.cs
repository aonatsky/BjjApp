﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TRNMNT.Core.Model.Participant;
using TRNMNT.Core.Services.Interface;
using TRNMNT.Data.Entities;
using TRNMNT.Data.Repositories;
using TRNMNT.Core.Helpers;
using TRNMNT.Core.Const;
using TRNMNT.Core.Enum;
using TRNMNT.Core.Model.Interface;
using TRNMNT.Core.Model;

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
             p.EventId == eventId
             && p.FirstName == model.FirstName
             && p.LastName == model.LastName
             && p.DateOfBirth == model.DateOfBirth);
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

            allParticipants = SortParticipants(allParticipants, filter.SortField, filter.SortDirection, federationId);

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

        private IQueryable<Participant> SortParticipants(IQueryable<Participant> allParticipants, ParticpantSortField filterSortField, SortDirectionEnum sortDirection, Guid federationId)
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
                    allParticipants = allParticipants.OrderByDirection(sortDirection, p => p.Team.FederationId == federationId);
                    break;
                default:
                    allParticipants = allParticipants.OrderByDirection(sortDirection, p => p.FirstName);
                    break;
            }
            return allParticipants;
        }

        #endregion
    }
}