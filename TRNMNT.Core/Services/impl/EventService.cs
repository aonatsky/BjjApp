using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TRNMNT.Core.Const;
using TRNMNT.Core.Model.Category;
using TRNMNT.Core.Model.Event;
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
        private readonly IRepository<FederationMembership> _federationMembershipRepository;
        private readonly IAppDbContext _unitOfWork;
        private readonly IPromoCodeService _promoCodeService;

        #endregion

        #region .ctor

        public EventService(
            IRepository<Event> eventRepository,
            IRepository<Category> categoryRepository,
            IRepository<WeightDivision> weightDivisionRepository,
            IRepository<FederationMembership> federationMembershipRepository,
            IFileService fileService,
            IAppDbContext unitOfWork,
            IPromoCodeService promoCodeService
        )
        {
            _eventRepository = eventRepository;
            _categoryRepository = categoryRepository;
            _weightDivisionRepository = weightDivisionRepository;
            _federationMembershipRepository = federationMembershipRepository;
            _fileService = fileService;
            _unitOfWork = unitOfWork;
            _promoCodeService = promoCodeService;
        }

        #endregion

        #region Public Methods

        public async Task UpdateEventAsync(EventModelFull eventModel)
        {
            var existingEvent = await _eventRepository.GetAll(e => e.EventId == eventModel.EventId).Include(e => e.Categories).ThenInclude(c => c.WeightDivisions).FirstOrDefaultAsync();// GetByIDAsync<Guid>(eventModel.EventId);
            var updatedEvent = GetEventFromModel(eventModel);
            updatedEvent.UpdateTS = DateTime.UtcNow;
            updatedEvent.IsActive = true;
            updatedEvent.FederationId = existingEvent.FederationId;
            updatedEvent.OwnerId = existingEvent.OwnerId;
            _eventRepository.UpdateValues(existingEvent, updatedEvent);
            UpdateCategories(existingEvent.Categories, updatedEvent.Categories);
            await _unitOfWork.SaveAsync();
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
                var exsistingDivision = existingDivisions.FirstOrDefault(c => c.WeightDivisionId == updatedDivision.WeightDivisionId);
                if (exsistingDivision != null)
                {
                    _weightDivisionRepository.UpdateValues(exsistingDivision, updatedDivision);
                }
                else
                {
                    _weightDivisionRepository.Add(updatedDivision);
                }
            }

        }
        public async Task<EventModelFull> GetFullEventAsync(Guid id)
        {
            var @event = await _eventRepository.GetAll().Include(e => e.Categories).ThenInclude(c => c.WeightDivisions).FirstOrDefaultAsync(e => e.EventId == id);
            return @event != null ? GetEventModel(@event) : null;
        }

        public async Task<List<EventModelBase>> GetEventsForOwnerAsync(string userId)
        {
            var models = await _eventRepository.GetAll()
                //.Where(e => e.OwnerId == userId)
                .Select(e => new EventModelBase
                {
                    EventId = e.EventId,
                    EventDate = e.EventDate,
                    RegistrationEndTS = e.RegistrationEndTS,
                    EarlyRegistrationEndTS = e.EarlyRegistrationEndTS,
                    RegistrationStartTS = e.RegistrationStartTS,
                    Title = e.Title
                }).ToListAsync();

            return models;
        }

        public async Task<bool> IsEventUrlPrefixExistAsync(string prefix)
        {
            return await _eventRepository.GetAll().AnyAsync();
        }

        public async Task SaveEventImageAsync(Stream stream, string eventId)
        {
            var fileName = FilePath.EventImageFile;
            var path = Path.Combine(FilePath.EventDataFolder, eventId, FilePath.EventImageFolder, FilePath.EventImageFile);
            var _event = await _eventRepository.GetByIDAsync(new Guid(eventId));
            await _fileService.SaveImageAsync(path, stream, fileName);
            _event.ImgPath = path;
            _eventRepository.Update(_event);
            await _unitOfWork.SaveAsync();
        }

        public async Task SaveEventTncAsync(Stream stream, string eventId, string fileName)
        {
            var path = Path.Combine(FilePath.EventDataFolder, eventId, FilePath.EventTncFolder, fileName);
            await _fileService.SaveFileAsync(path, stream);
            var _event = await _eventRepository.GetByIDAsync(new Guid(eventId));
            _event.TNCFilePath = path;
            _eventRepository.Update(_event);
            await _unitOfWork.SaveAsync();
        }

        public async Task SavePromoCodeListAsync(Stream stream, string eventId)
        {
            var path = Path.Combine(FilePath.EventDataFolder, eventId, FilePath.EventTncFolder, FilePath.EventPromocodeListFile);
            await _fileService.SaveFileAsync(path, stream);
            var _event = await _eventRepository.GetByIDAsync(new Guid(eventId));
            _event.PromoCodeListPath = path;
            _eventRepository.Update(_event);
            await _unitOfWork.SaveAsync();
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

        public async Task<int> GetPriceAsync(Guid eventId, string userId)
        {
            var _event = await _eventRepository.GetByIDAsync(eventId);
            if (_event != null)
            {
                var isMember = await _federationMembershipRepository.GetAll().Where(fm => fm.UserId == userId && fm.FederationId == _event.FederationId).AnyAsync();
                var dateNow = DateTime.UtcNow;
                if (dateNow <= _event.EarlyRegistrationEndTS)
                {
                    return isMember ? _event.EarlyRegistrationPriceForMembers : _event.EarlyRegistrationPrice;
                }
                return isMember ? _event.LateRegistrationPriceForMembers : _event.LateRegistrationPrice;
            }
            return 0;
        }

        public async Task<int> GetPriceAsync(Guid eventId, string userId, string promoCode = "")
        {
            var _event = await _eventRepository.GetByIDAsync(eventId);
            if (_event != null)
            {
                var isPromoCodeUsed = false;
                if (string.IsNullOrEmpty(promoCode))
                {
                    isPromoCodeUsed = await _promoCodeService.ValidateCodeAsync(eventId, promoCode, userId);
                }
                var dateNow = DateTime.UtcNow;
                if (dateNow <= _event.EarlyRegistrationEndTS)
                {
                    return isPromoCodeUsed ? _event.EarlyRegistrationPriceForMembers : _event.EarlyRegistrationPrice;
                }
                return isPromoCodeUsed ? _event.LateRegistrationPriceForMembers : _event.LateRegistrationPrice;
            }
            return 0;
        }


        public async Task<int> GetPriceAsync(Guid eventId, bool specialPrice)
        {
            var _event = await _eventRepository.GetByIDAsync(eventId);
            if (_event != null)
            {
                var dateNow = DateTime.UtcNow;
                if (dateNow <= _event.EarlyRegistrationEndTS)
                {
                    return specialPrice ? _event.EarlyRegistrationPriceForMembers : _event.EarlyRegistrationPrice;
                }
                return specialPrice ? _event.LateRegistrationPriceForMembers : _event.LateRegistrationPrice;
            }
            return 0;
        }

        public async Task<EventModelBase> GetEventBaseInfoAsync(Guid id)
        {
            var model = await _eventRepository.GetAll().Where(e => e.EventId == id)
                .Select(e => new EventModelBase { EventId = e.EventId, EventDate = e.EventDate, RegistrationEndTS = e.RegistrationEndTS, EarlyRegistrationEndTS = e.EarlyRegistrationEndTS, RegistrationStartTS = e.RegistrationStartTS, Title = e.Title })
                .FirstOrDefaultAsync();
            return model;
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
                RegistrationEndTS  = DateTime.UtcNow.AddDays(21).Date,
                IsActive = false
            };
            _eventRepository.Add(_event);

            return _event.EventId;
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
                    RoundTime = category.MatchTime,
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
                    Descritpion = weightDivision.Descritpion,
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
                    MatchTime = model.RoundTime,
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
                Descritpion = model.Descritpion,
                Name = model.Name,
                CategoryId = categoryId
            }).ToList();
        }
        
        #endregion
    }

}

