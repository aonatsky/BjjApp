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
            var _event = await _eventRepository.GetAll().Include(e => e.Categories).ThenInclude(c => c.WeightDivisions).FirstOrDefaultAsync();// GetByIDAsync<Guid>(eventModel.EventId);

            _event = UpdateFromModel(_event, eventModel);
            _event.UpdateTS = DateTime.UtcNow;
            _event.IsActive = true;
            _eventRepository.Update(_event);

            await DeleteCategoriesAsync(_event.EventId, eventModel.CategoryModels.Where(cm => !String.IsNullOrEmpty(cm.CategoryId)).Select(cm => Guid.Parse(cm.CategoryId)));

            foreach (var categoryModel in eventModel.CategoryModels)
            {

                var category = Guid.TryParse(categoryModel.CategoryId, out var categoryId) ? _categoryRepository.GetByID(categoryId) : null;
                if (category != null)
                {
                    category.Name = categoryModel.Name;
                    _categoryRepository.Update(category);
                }
                else
                {
                    category = new Category
                    {
                        Name = categoryModel.Name,
                        EventId = _event.EventId,
                        RoundTime = categoryModel.RoundTime,
                        CategoryId = Guid.NewGuid()
                    };
                    _categoryRepository.Add(category);
                }

                var actualWeightDivisionIds = categoryModel.WeightDivisionModels.Where(wdm => !String.IsNullOrEmpty(wdm.WeightDivisionId)).Select(wdm => new Guid(wdm.WeightDivisionId));
                var wdToDelete = _weightDivisionRepository.GetAll().Where(wd => wd.CategoryId == category.CategoryId && !actualWeightDivisionIds.Contains(wd.WeightDivisionId));
                _weightDivisionRepository.DeleteRange(wdToDelete);

                foreach (var wdModel in categoryModel.WeightDivisionModels)
                {
                    var wd = Guid.TryParse(wdModel.WeightDivisionId, out var weightDivisionId) ? _weightDivisionRepository.GetByID(weightDivisionId) : null;
                    if (wd != null)
                    {
                        wd.Name = wdModel.Name;
                        _weightDivisionRepository.Update(wd);
                    }
                    else
                    {
                        wd = new WeightDivision
                        {
                            WeightDivisionId = Guid.NewGuid(),
                            Name = wdModel.Name,
                            CategoryId = category.CategoryId
                        };
                        _weightDivisionRepository.Add(wd);
                    }
                }
            }

            await _unitOfWork.SaveAsync();
        }

        public async Task<EventModelFull> GetFullEventAsync(Guid id)
        {
            var @event = await _eventRepository.GetAll().Include(e => e.Categories).ThenInclude(c => c.WeightDivisions).FirstOrDefaultAsync(e => e.EventId == id);
            return @event != null ? GetEventModel(@event) : null;
        }

        public async Task<List<EventModelBase>> GetEventsForOwnerAsync(string userId)
        {
            var models = await _eventRepository.GetAll()
                .Where(e => e.OwnerId == userId)
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
                EventDate = DateTime.UtcNow.AddMonths(1).Date
            };
            _eventRepository.Add(_event);

            return _event.EventId;
        }

        #endregion

        #region Helpers

        private EventModelFull GetEventModel(Event @event)
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
                LateRegistrationPrice = @event.EarlyRegistrationPriceForMembers,
                LateRegistrationPriceForMembers = @event.LateRegistrationPriceForMembers,
                PromoCodeEnabled = @event.PromoCodeEnabled,
                PromoCodeListPath = @event.PromoCodeListPath,
                CategoryModels = GetCategoryModels(@event.Categories)
            };
        }

        private EventModelInfo GetEventModelInfo(Event _event)
        {
            return new EventModelInfo
            {
                EventId = _event.EventId,
                EventDate = _event.EventDate,
                AdditionalData = _event.AdditionalData,
                Address = _event.Address,
                ContactEmail = _event.ContactEmail,
                Description = _event.Description,
                ImgPath = _event.ImgPath,
                TNCFilePath = _event.TNCFilePath,
                ContactPhone = _event.ContactPhone,
                Title = _event.Title,
                FBLink = _event.FBLink,
                RegistrationEndTS = _event.RegistrationEndTS,
                RegistrationStartTS = _event.RegistrationStartTS,
                VKLink = _event.VKLink,
                EarlyRegistrationEndTS = _event.EarlyRegistrationEndTS,
                EarlyRegistrationPrice = _event.EarlyRegistrationPrice,
                EarlyRegistrationPriceForMembers = _event.EarlyRegistrationPriceForMembers,
                LateRegistrationPrice = _event.EarlyRegistrationPriceForMembers,
                LateRegistrationPriceForMembers = _event.LateRegistrationPriceForMembers,
            };
        }

        private ICollection<CategoryModel> GetCategoryModels(ICollection<Category> categories)
        {
            var categoryModels = new List<CategoryModel>();
            if (categories != null)
            {
                categoryModels.AddRange(categories.Select(category => new CategoryModel
                {
                    CategoryId = category.CategoryId.ToString(),
                    Name = category.Name,
                    RoundTime = category.RoundTime,
                    WeightDivisionModels = GetWeightDeivisionsModels(category.WeightDivisions),
                    EventId = category.EventId
                }));
            }
            return categoryModels;
        }

        private ICollection<WeightDivisionModel> GetWeightDeivisionsModels(ICollection<WeightDivision> weightDivisions)
        {
            var weightDivisionModels = new List<WeightDivisionModel>();
            if (weightDivisions != null)
            {
                weightDivisionModels.AddRange(weightDivisions.Select(weightDivision => new WeightDivisionModel
                {
                    WeightDivisionId = weightDivision.WeightDivisionId.ToString(),
                    Weight = weightDivision.Weight,
                    Descritpion = weightDivision.Descritpion,
                    Name = weightDivision.Name,
                    CategoryId = weightDivision.CategoryId.ToString()
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
                EarlyRegistrationPriceForMembers = eventModel.EarlyRegistrationPriceForMembers,
                EarlyRegistrationPrice = eventModel.EarlyRegistrationPriceForMembers,
                EarlyRegistrationEndTS = eventModel.EarlyRegistrationEndTS,
                Categories = GetCategoriesFromModels(eventModel.CategoryModels)
            };
        }

        private ICollection<Category> GetCategoriesFromModels(ICollection<CategoryModel> models)
        {
            return models.Select(model => new Category
            {
                EventId = model.EventId,
                CategoryId = Guid.Parse(model.CategoryId),
                Name = model.Name,
                WeightDivisions = GetWeightDeivisionsFromModels(model.WeightDivisionModels)
            }).ToList();
        }

        private ICollection<WeightDivision> GetWeightDeivisionsFromModels(ICollection<WeightDivisionModel> models)
        {
            return models.Select(model => new WeightDivision
            {
                WeightDivisionId = Guid.Parse(model.WeightDivisionId),
                Weight = model.Weight,
                Descritpion = model.Descritpion,
                Name = model.Name,
                CategoryId = Guid.Parse(model.CategoryId)
            }).ToList();
        }

        private Event UpdateFromModel(Event @event, EventModelFull eventModel)
        {
            @event.EventDate = eventModel.EventDate;
            @event.AdditionalData = eventModel.AdditionalData;
            @event.Address = eventModel.Address;
            @event.ContactEmail = eventModel.ContactEmail;
            @event.Description = eventModel.Description;
            @event.ImgPath = eventModel.ImgPath;
            @event.TNCFilePath = eventModel.TNCFilePath;
            @event.ContactPhone = eventModel.ContactPhone;
            @event.Title = eventModel.Title;
            @event.FBLink = eventModel.FBLink;
            @event.RegistrationEndTS = eventModel.RegistrationEndTS;
            @event.RegistrationStartTS = eventModel.RegistrationStartTS;
            @event.UrlPrefix = eventModel.UrlPrefix;
            @event.VKLink = eventModel.VKLink;
            @event.EarlyRegistrationEndTS = eventModel.EarlyRegistrationEndTS;
            @event.EarlyRegistrationPrice = eventModel.EarlyRegistrationPrice;
            @event.EarlyRegistrationPriceForMembers = eventModel.EarlyRegistrationPriceForMembers;
            @event.LateRegistrationPrice = eventModel.EarlyRegistrationPriceForMembers;
            @event.LateRegistrationPriceForMembers = eventModel.LateRegistrationPriceForMembers;
            @event.PromoCodeEnabled = eventModel.PromoCodeEnabled;
            @event.PromoCodeListPath = eventModel.PromoCodeListPath;
            return @event;
        }


        private async Task DeleteCategoriesAsync(Guid eventId, IEnumerable<Guid> eventCategoryIds)
        {
            var categoriesToDelete = await _categoryRepository.GetAll().Where(c => c.EventId == eventId && !eventCategoryIds.Contains(c.CategoryId)).ToListAsync();
            var weightDivisionsToDelete = await _weightDivisionRepository.GetAll().Where(wd => categoriesToDelete.Select(c => c.CategoryId).Contains(wd.CategoryId)).ToListAsync();
            _weightDivisionRepository.DeleteRange(weightDivisionsToDelete);
            _categoryRepository.DeleteRange(categoriesToDelete);
        }

        #endregion
    }

}

