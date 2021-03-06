﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TRNMNT.Core.Const;
using TRNMNT.Core.Helpers.Exceptions;
using TRNMNT.Core.Model;
using TRNMNT.Core.Model.Category;
using TRNMNT.Core.Model.Event;
using TRNMNT.Core.Model.Participant;
using TRNMNT.Core.Model.WeightDivision;
using TRNMNT.Core.Services.Interface;
using TRNMNT.Data.Context;
using TRNMNT.Data.Entities;
using TRNMNT.Data.Repositories;

namespace TRNMNT.Core.Services.Impl
{
    public class EventService : IEventService
    {
        #region Dependencies

        private readonly IRepository<Event> _eventRepository;
        private readonly IRepository<Category> _categoryRepository;
        private readonly IRepository<WeightDivision> _weightDivisionRepository;
        private readonly IFileService _fileService;
        private readonly IFederationMembershipService _federationMembershipService;
        private readonly IPromoCodeService _promoCodeService;
        private readonly IFederationService _federationService;
        private readonly IRepository<Participant> _participantRepository;
        private IRepository<Match> _matchRepository;

        #endregion

        #region .ctor

        public EventService(
            IRepository<Event> eventRepository,
            IRepository<Category> categoryRepository,
            IRepository<WeightDivision> weightDivisionRepository,
            IRepository<Participant> participantRepository,
            IFederationMembershipService federationMembershipService,
            IFileService fileService,
            IPromoCodeService promoCodeService,
            IFederationService federationService,
            IRepository<Match> matchRepository
        )
        {
            _eventRepository = eventRepository;
            _categoryRepository = categoryRepository;
            _weightDivisionRepository = weightDivisionRepository;
            _federationMembershipService = federationMembershipService;
            _fileService = fileService;
            _promoCodeService = promoCodeService;
            _federationService = federationService;
            _matchRepository = matchRepository;
            _participantRepository = participantRepository;
        }

        #endregion

        #region Public Methods

        public async Task UpdateEventAsync(EventModelFull eventModel)
        {
            var existingEvent = await _eventRepository.GetAll(e => e.EventId == eventModel.EventId).Include(e => e.Categories).ThenInclude(c => c.WeightDivisions).FirstOrDefaultAsync(); // GetByIDAsync<Guid>(eventModel.EventId);
            var updatedEvent = GetEventFromModel(eventModel);
            updatedEvent.UpdateTS = DateTime.UtcNow;
            updatedEvent.IsActive = true;
            updatedEvent.FederationId = existingEvent.FederationId;
            updatedEvent.OwnerId = existingEvent.OwnerId;
            _eventRepository.UpdateValues(existingEvent, updatedEvent);
            UpdateCategories(existingEvent.Categories, updatedEvent.Categories);
        }

        public void UpdateCategories(IEnumerable<Category> existingCategories, IEnumerable<Category> updatedCategories)
        {
            foreach (var category in existingCategories)
            {
                if (updatedCategories.All(c => c.CategoryId != category.CategoryId))
                {
                    _weightDivisionRepository.DeleteRange(category.WeightDivisions);
                    _categoryRepository.Delete(category);
                }
            }
            foreach (var updatedCategory in updatedCategories)
            {
                var existingCategory = existingCategories.FirstOrDefault(c => c.CategoryId == updatedCategory.CategoryId);
                if (existingCategory != null)
                {
                    _categoryRepository.UpdateValues(existingCategory, updatedCategory);
                    UpdateWeightDivisions(existingCategory.WeightDivisions, updatedCategory.WeightDivisions);
                }
                else
                {
                    _categoryRepository.Add(updatedCategory);
                    _weightDivisionRepository.AddRange(updatedCategory.WeightDivisions);
                }

            }

        }

        public async Task<Event> GetEventAsync(Guid? id)
        {
            return await _eventRepository.GetByIDAsync(id);
        }

        public bool IsCorrectionAllowed(Event _event)
        {
            return DateTime.UtcNow < _event.RegistrationEndTS.AddHours(18);
        }

        public void UpdateWeightDivisions(IEnumerable<WeightDivision> existingDivisions,
            IEnumerable<WeightDivision> updatedDivisions)
        {
            //deleting
            foreach (var existingDivision in existingDivisions)
            {
                if (updatedDivisions.All(c => c.WeightDivisionId != existingDivision.WeightDivisionId))
                {
                    _weightDivisionRepository.Delete(existingDivision);
                }
            }

            //updating
            foreach (var updatedDivision in updatedDivisions)
            {
                var existingDivision = existingDivisions.FirstOrDefault(c => c.WeightDivisionId == updatedDivision.WeightDivisionId);
                if (existingDivision != null)
                {
                    _weightDivisionRepository.UpdateValues(existingDivision, updatedDivision);
                }
                else
                {
                    _weightDivisionRepository.Add(updatedDivision);
                }
            }

        }
        public async Task<EventModelFull> GetFullEventModelAsync(Guid id)
        {
            var @event = await _eventRepository.GetAll().Include(e => e.Categories).ThenInclude(c => c.WeightDivisions).FirstOrDefaultAsync(e => e.EventId == id);
            return @event != null ? GetEventModel(@event) : null;
        }

        public async Task<List<EventModelBase>> GetEventsForOwnerAsync(string userId, string role)
        {
            var modelsQuery = _eventRepository.GetAll();
            if (role == Roles.Admin)
            {
                return await GetModels(_eventRepository.GetAll(e => e.IsActive));
            }
            if (role == Roles.FederationOwner)
            {
                return await GetModels(_eventRepository.GetAll(e => e.IsActive));
            }
            if (role == Roles.Owner)
            {
                return await GetModels(_eventRepository.GetAll(e => e.IsActive && e.OwnerId == userId));
            }
            return new List<EventModelBase>();

            async Task<List<EventModelBase>> GetModels(IQueryable<Event> query)
            {
                return await query.Select(e => new EventModelBase
                {
                    EventId = e.EventId,
                        EventDate = e.EventDate,
                        RegistrationEndTS = e.RegistrationEndTS,
                        EarlyRegistrationEndTS = e.EarlyRegistrationEndTS,
                        RegistrationStartTS = e.RegistrationStartTS,
                        Title = e.Title
                }).ToListAsync();
            }
        }

        public async Task<bool> IsEventUrlPrefixExistAsync(Guid eventId, string prefix)
        {
            var restrictedUrls = new List<string>() { "admin", "uabjj", "bjj" };

            return await _eventRepository.GetAll(e => e.UrlPrefix == prefix && e.EventId != eventId).AnyAsync() ||
                restrictedUrls.Contains(prefix);
        }

        public async Task SetCorrectionsEnabledAsync(Guid eventId, bool value)
        {
            var _event = await _eventRepository.GetByIDAsync(eventId);
            _event.CorrectionsEnabled = value;
            _eventRepository.Update(_event);
        }

        public async Task SetBracketsPublishAsync(Guid eventId, bool value)
        {
            var _event = await _eventRepository.GetByIDAsync(eventId);
            if (!_event.CorrectionsEnabled && await AreMatchesCreatedForEvent(eventId))
            {
                _event.BracketsPublished = value;
                _eventRepository.Update(_event);
            }
        }

        public async Task SetParticipantListsPublishAsync(Guid eventId, bool value)
        {
            var _event = await _eventRepository.GetByIDAsync(eventId);
            _event.ParticipantListsPublished = value;
            _eventRepository.Update(_event);
        }

        public async Task SaveEventImageAsync(Stream stream, string eventId)
        {
            var path = Path.Combine(FilePath.EventDataFolder, eventId, FilePath.EventImageFolder, FilePath.EventImageFile);
            var _event = await _eventRepository.GetByIDAsync(new Guid(eventId));
            await _fileService.SaveFileAsync(path, stream);
            _event.ImgPath = path;
            _eventRepository.Update(_event);
        }

        public async Task SaveEventTncAsync(Stream stream, string eventId, string fileName)
        {
            var path = Path.Combine(FilePath.EventDataFolder, eventId, FilePath.EventTncFolder, fileName);
            await _fileService.SaveFileAsync(path, stream);
            var _event = await _eventRepository.GetByIDAsync(new Guid(eventId));
            _event.TNCFilePath = path;
            _eventRepository.Update(_event);
        }

        public async Task SavePromoCodeListAsync(Stream stream, string eventId)
        {
            var path = Path.Combine(FilePath.EventDataFolder, eventId, FilePath.EventTncFolder, FilePath.EventPromocodeListFile);
            await _fileService.SaveFileAsync(path, stream);
            var _event = await _eventRepository.GetByIDAsync(new Guid(eventId));
            _event.PromoCodeListPath = path;
            _eventRepository.Update(_event);
        }

        public async Task<Guid?> GetEventIdAsync(string url)
        {
            var ids = await _eventRepository.GetAll().Where(e => e.UrlPrefix == url && e.IsActive).Select(e => e.EventId).ToListAsync();
            if (ids.Any())
            {
                return ids.FirstOrDefault();
            }
            return null;
        }

        public async Task<string> GetEventOwnerIdAsync(Guid eventId)
        {
            var existingEvent = await _eventRepository.GetByIDAsync(eventId);
            if (existingEvent != null)
            {
                return existingEvent.OwnerId;
            }
            return string.Empty;
        }

        public async Task<PriceModel> GetPriceAsync(Guid eventId, string userId, bool includeMembership)
        {
            var _event = await _eventRepository.GetByIDAsync(eventId);
            if (_event == null)
            {
                throw new BusinessException("ERROR.EVENT_IS_NOT_FOUND");
            }

            var isMember = await _federationMembershipService.IsFederationMemberAsync(_event.FederationId, userId);
            var priceModel = new PriceModel()
            {
                Currency = await _federationService.GetFederationCurrencyAsync(_event.FederationId)
            };

            if (DateTime.UtcNow <= _event.EarlyRegistrationEndTS)
            {
                priceModel.Amount = isMember || includeMembership ? _event.EarlyRegistrationPriceForMembers : _event.EarlyRegistrationPrice;
            }
            else
            {
                priceModel.Amount = isMember || includeMembership ? _event.LateRegistrationPrice : _event.LateRegistrationPrice;
            }

            if (!isMember && includeMembership)
            {
                priceModel.Amount += (await _federationService.GetMembershipPriceAsync(_event.FederationId)).Amount;
            }

            return priceModel;
        }

        public async Task<PriceModel> GetTeamPriceAsync(Guid eventId, List<ParticipantRegistrationModel> participants)
        {
            var _event = await _eventRepository.GetByIDAsync(eventId);
            if (_event == null)
            {
                throw new BusinessException("ERROR.EVENT_IS_NOT_FOUND");
            }

            var memberships = await _federationMembershipService.GetFederationMembershipsForUsersAsync(_event.FederationId, participants.Select(p => p.UserId).ToList());
            var priceModel = new PriceModel()
            {
                Currency = await _federationService.GetFederationCurrencyAsync(_event.FederationId)
            };
            var membershipPrice = (await _federationService.GetMembershipPriceAsync(_event.FederationId)).Amount;

            foreach (var participant in participants)
            {
                var isMember = memberships.Any(m => m.UserId == participant.UserId);
                if (DateTime.UtcNow <= _event.EarlyRegistrationEndTS)
                {
                    priceModel.Amount += isMember ||
                        participant.IncludeMembership ? _event.EarlyRegistrationPriceForMembers : _event.EarlyRegistrationPrice;
                }
                else
                {
                    priceModel.Amount += isMember ||
                        participant.IncludeMembership ? _event.LateRegistrationPrice : _event.LateRegistrationPrice;
                }

                if (!isMember && participant.IncludeMembership)
                {
                    priceModel.Amount += membershipPrice;
                }
            }
            return priceModel;
        }

        // public async Task<int> GetPriceAsync(Guid eventId, string userId, string promoCode = "")
        // {
        //     var _event = await _eventRepository.GetByIDAsync(eventId);
        //     if (_event != null)
        //     {
        //         var isPromoCodeUsed = false;
        //         if (string.IsNullOrEmpty(promoCode))
        //         {
        //             isPromoCodeUsed = await _promoCodeService.ValidateCodeAsync(eventId, promoCode, userId);
        //         }
        //         var dateNow = DateTime.UtcNow;
        //         if (dateNow <= _event.EarlyRegistrationEndTS)
        //         {
        //             return isPromoCodeUsed ? _event.EarlyRegistrationPriceForMembers : _event.EarlyRegistrationPrice;
        //         }
        //         return isPromoCodeUsed ? _event.LateRegistrationPriceForMembers : _event.LateRegistrationPrice;
        //     }
        //     throw new BusinessException("ERROR.EVENT_IS_NOT_FOUND");
        // }

        public async Task<EventModelBase> GetEventBaseInfoAsync(Guid id)
        {
            var model = await _eventRepository.GetAll().Where(e => e.EventId == id)
                .Select(e => new EventModelBase { EventId = e.EventId, EventDate = e.EventDate, RegistrationEndTS = e.RegistrationEndTS, EarlyRegistrationEndTS = e.EarlyRegistrationEndTS, RegistrationStartTS = e.RegistrationStartTS, Title = e.Title })
                .FirstOrDefaultAsync();
            return model;
        }

        public async Task<EventDashboardModel> GetEventDashboardDataAsync(Guid id)
        {
            var _event = await GetEventAsync(id);
            if (_event == null)
            {
                throw new BusinessException("ERROR.EVENT_NOT_FOUND");
            }
            var participantGroups = await _participantRepository.GetAll(p => p.EventId == id && p.IsActive)
                .Include(p => p.Category).Include(p => p.WeightDivision)
                .GroupBy(x => new { CName = x.Category.Name, WDName = x.WeightDivision.Name })
                .Select(g => new CategoryWeightDivisionParticipants { CategoryName = g.Key.CName, WeightDivisionName = g.Key.WDName, ParticipantsCount = g.Count() }).ToListAsync();
            return new EventDashboardModel()
            {
                EventId = _event.EventId,
                    EventDate = _event.EventDate,
                    Title = _event.Title,
                    BracketsCreated = await AreMatchesCreatedForEvent(id),
                    BracketsPublished = _event.BracketsPublished,
                    ParticipantListsPublished = _event.ParticipantListsPublished,
                    EventStatus = GetEventStatus(_event),
                    CorrectionsEnabled = _event.CorrectionsEnabled,
                    ParticipantGroups = participantGroups
            };
        }

        private async Task<bool> AreMatchesCreatedForEvent(Guid eventId)
        {
            return await _matchRepository.GetAll(m => m.Category.EventId == eventId).AnyAsync();
        }

        private string GetEventStatus(Event _event)
        {
            var now = DateTime.UtcNow;
            if (now < _event.RegistrationStartTS)
            {
                return EventStatus.GetTranslationKey(EventStatus.RegistrationNotStarted);
            }
            if (now >= _event.RegistrationStartTS && now <= _event.EarlyRegistrationEndTS)
            {
                return EventStatus.GetTranslationKey(EventStatus.EarlyRegistartion);
            }
            if (now > _event.EarlyRegistrationEndTS && now <= _event.RegistrationEndTS)
            {
                return EventStatus.GetTranslationKey(EventStatus.LateRegistration);
            }
            if (now > _event.RegistrationEndTS)
            {
                return EventStatus.GetTranslationKey(EventStatus.RegistrationEnded);
            }
            return EventStatus.GetTranslationKey(EventStatus.Draft);
        }

        public async Task<EventModelFull> GetEventInfoAsync(Guid? id)
        {
            if (!id.HasValue)
            {
                return null;
            }
            var _event = await _eventRepository.GetAll().Include(e => e.Categories).ThenInclude(c => c.WeightDivisions).FirstOrDefaultAsync(e => e.EventId == id);
            if (_event != null)
            {
                return GetEventModel(_event);
            }
            return null;
        }

        public Guid CreateEvent(string userId, Guid federationId)
        {
            var _event = new Event
            {
                EventId = Guid.NewGuid(),
                OwnerId = userId,
                FederationId = federationId,
                EventDate = DateTime.UtcNow.AddMonths(1).Date,
                RegistrationStartTS = DateTime.UtcNow.AddDays(7).Date,
                EarlyRegistrationEndTS = DateTime.UtcNow.AddDays(14).Date,
                RegistrationEndTS = DateTime.UtcNow.AddDays(21).Date,
                IsActive = false
            };
            _eventRepository.Add(_event);

            return _event.EventId;
        }

        public async Task DeleteEventAsync(Guid eventId)
        {
            var _event = await _eventRepository.GetByIDAsync(eventId);
            if (_event == null)
            {
                throw new BusinessException("ERROR.EVENT_NOT_FOUND");
            }
            _event.IsActive = false;
            _eventRepository.Update(_event);
        }

        #endregion

        #region Helpers

        private static EventModelFull GetEventModel(Event @event)
        {
            return new EventModelFull
            {
                EventId = @event.EventId,
                    EventDate = @event.EventDate,
                    AdditionalData = @event.AdditionalData,
                    Address = @event.Address,
                    ContactEmail = @event.ContactEmail,
                    Description = @event.Description,
                    ImgPath = @event.ImgPath,
                    TNCFilePath = @event.TNCFilePath,
                    ContactPhone = @event.ContactPhone,
                    Title = @event.Title,
                    FBLink = @event.FBLink,
                    RegistrationEndTS = @event.RegistrationEndTS,
                    RegistrationStartTS = @event.RegistrationStartTS,
                    UrlPrefix = @event.UrlPrefix,
                    VKLink = @event.VKLink,
                    EarlyRegistrationEndTS = @event.EarlyRegistrationEndTS,
                    EarlyRegistrationPrice = @event.EarlyRegistrationPrice,
                    EarlyRegistrationPriceForMembers = @event.EarlyRegistrationPriceForMembers,
                    LateRegistrationPrice = @event.LateRegistrationPrice,
                    LateRegistrationPriceForMembers = @event.LateRegistrationPriceForMembers,
                    PromoCodeEnabled = @event.PromoCodeEnabled,
                    PromoCodeListPath = @event.PromoCodeListPath,
                    ParticipantListsPublished = @event.ParticipantListsPublished,
                    CorrectionsEnabled = @event.CorrectionsEnabled,
                    BracketsPublished = @event.BracketsPublished,
                    CategoryModels = GetCategoryModels(@event.Categories)
            };
        }

        private static ICollection<CategoryModel> GetCategoryModels(ICollection<Category> categories)
        {
            var categoryModels = new List<CategoryModel>();
            if (categories != null)
            {
                categoryModels.AddRange(categories.Select(category => new CategoryModel
                {
                    CategoryId = category.CategoryId,
                        Name = category.Name,
                        MatchTime = category.MatchTime,
                        WeightDivisionModels = GetWeightDeivisionsModels(category.WeightDivisions),
                        EventId = category.EventId
                }));
            }
            return categoryModels;
        }

        private static ICollection<WeightDivisionModel> GetWeightDeivisionsModels(ICollection<WeightDivision> weightDivisions)
        {
            var weightDivisionModels = new List<WeightDivisionModel>();
            if (weightDivisions != null)
            {
                weightDivisionModels.AddRange(weightDivisions.Where(w => !w.IsAbsolute).Select(weightDivision => new WeightDivisionModel
                {
                    WeightDivisionId = weightDivision.WeightDivisionId,
                        Weight = weightDivision.Weight,
                        Description = weightDivision.Description,
                        Name = weightDivision.Name,
                        CategoryId = weightDivision.CategoryId
                }));
            }
            return weightDivisionModels;
        }

        private Event GetEventFromModel(EventModelFull eventModel)
        {
            return new Event
            {
                EventId = eventModel.EventId,
                    EventDate = eventModel.EventDate,
                    AdditionalData = eventModel.AdditionalData,
                    Address = eventModel.Address,
                    ContactEmail = eventModel.ContactEmail,
                    Description = eventModel.Description,
                    ImgPath = eventModel.ImgPath,
                    TNCFilePath = eventModel.TNCFilePath,
                    ContactPhone = eventModel.ContactPhone,
                    Title = eventModel.Title,
                    FBLink = eventModel.FBLink,
                    RegistrationEndTS = eventModel.RegistrationEndTS,
                    RegistrationStartTS = eventModel.RegistrationStartTS,
                    UrlPrefix = eventModel.UrlPrefix,
                    VKLink = eventModel.VKLink,
                    PromoCodeEnabled = eventModel.PromoCodeEnabled,
                    LateRegistrationPrice = eventModel.LateRegistrationPrice,
                    LateRegistrationPriceForMembers = eventModel.LateRegistrationPriceForMembers,
                    EarlyRegistrationPriceForMembers = eventModel.EarlyRegistrationPriceForMembers,
                    EarlyRegistrationPrice = eventModel.EarlyRegistrationPrice,
                    EarlyRegistrationEndTS = eventModel.EarlyRegistrationEndTS,
                    Categories = GetCategoriesFromModels(eventModel.CategoryModels)
            };
        }

        private static ICollection<Category> GetCategoriesFromModels(ICollection<CategoryModel> models)
        {
            return models.Select(model =>
            {
                var category = new Category
                {
                EventId = model.EventId,
                CategoryId = model.CategoryId == Guid.Empty ? Guid.NewGuid() : model.CategoryId,
                MatchTime = model.MatchTime,
                Name = model.Name
                };
                category.WeightDivisions = GetWeightDeivisionsFromModels(model.WeightDivisionModels, category.CategoryId);
                return category;
            }).ToList();
        }

        private static ICollection<WeightDivision> GetWeightDeivisionsFromModels(ICollection<WeightDivisionModel> models, Guid categoryId)
        {
            return models.Select(model => new WeightDivision
            {
                WeightDivisionId = model.WeightDivisionId == Guid.Empty ? Guid.NewGuid() : model.WeightDivisionId,
                    Weight = model.Weight,
                    Description = model.Description,
                    Name = model.Name,
                    CategoryId = categoryId
            }).ToList();
        }

        #endregion
    }

}