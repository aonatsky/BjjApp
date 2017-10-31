using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TRNMNT.Data.Entities;
using TRNMNT.Data.Repositories;
using System.Linq;
using System.IO;
using TRNMNT.Web.Core.Const;
using TRNMNT.Web.Core.Enum;
using TRNMNT.Core.Model.Event;
using TRNMNT.Core.Model.Category;
using TRNMNT.Core.Model.WeightDivision;
using TRNMNT.Data.Context;

namespace TRNMNT.Core.Services
{
    public class EventService : IEventService
    {
        private IRepository<Event> eventRepository;
        private IRepository<Category> categoryRepository;
        private IRepository<WeightDivision> weightDivisionRepository;
        private IFileService fileService;
        private IRepository<FederationMembership> federationMembershipRepository;
        private IAppDbContext unitOfWork;
        private IPromoCodeService promoCodeService;

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
            this.eventRepository = eventRepository;
            this.categoryRepository = categoryRepository;
            this.weightDivisionRepository = weightDivisionRepository;
            this.federationMembershipRepository = federationMembershipRepository;
            this.fileService = fileService;
            this.unitOfWork = unitOfWork;
            this.promoCodeService = promoCodeService;
        }

        public async Task UpdateEventAsync(EventModelFull eventModel)
        {
            var _event = await eventRepository.GetAll().Include(e => e.Categories).ThenInclude(c => c.WeightDivisions).FirstOrDefaultAsync();// GetByIDAsync<Guid>(eventModel.EventId);

            _event = UpdateFromModel(_event, eventModel);
            _event.UpdateTS = DateTime.UtcNow;
            _event.IsActive = true;
            eventRepository.Update(_event);

            await DeleteCategoriesAsync(_event.EventId, eventModel.CategoryModels.Where(cm => !String.IsNullOrEmpty(cm.CategoryId)).Select(cm => Guid.Parse(cm.CategoryId)));


            foreach (var categoryModel in eventModel.CategoryModels)
            {

                Category category = Guid.TryParse(categoryModel.CategoryId, out Guid categoryId) ? categoryRepository.GetByID(categoryId) : null;
                if (category != null)
                {
                    category.Name = categoryModel.Name;
                    categoryRepository.Update(category);
                }
                else
                {
                    category = new Category()
                    {
                        Name = categoryModel.Name,
                        EventId = categoryModel.EventId,
                        CategoryId = Guid.NewGuid()
                    };
                    categoryRepository.Add(category);
                }




                var actualWeightDivisionIds = categoryModel.WeightDivisionModels.Where(wdm => !String.IsNullOrEmpty(wdm.WeightDivisionId)).Select(wdm => new Guid(wdm.WeightDivisionId));
                var wdToDelete = weightDivisionRepository.GetAll().Where(wd => wd.CategoryId == category.CategoryId && !actualWeightDivisionIds.Contains(wd.WeightDivisionId));
                weightDivisionRepository.DeleteRange(wdToDelete);


                foreach (var wdModel in categoryModel.WeightDivisionModels)
                {
                    var wd = Guid.TryParse(wdModel.WeightDivisionId, out var weightDivisionId) ? weightDivisionRepository.GetByID(weightDivisionId) : null;
                    if (wd != null)
                    {
                        wd.Name = wdModel.Name;
                        weightDivisionRepository.Update(wd);
                    }
                    else
                    {
                        wd = new WeightDivision()
                        {
                            WeightDivisionId = Guid.NewGuid(),
                            Name = wdModel.Name,
                            CategoryId = category.CategoryId
                        };
                        weightDivisionRepository.Add(wd);
                    }
                }

            }

            await unitOfWork.SaveAsync();
        }

        public async Task<EventModelFull> GetNewEventAsync(string userId)
        {
            var eventToAdd = new Event()
            {
                EventId = Guid.NewGuid(),
                OwnerId = userId,
                UpdateTS = DateTime.UtcNow,
                IsActive = false,
                StatusId = (int)EventStatusEnum.Init
            };
            eventRepository.Add(eventToAdd);
            await unitOfWork.SaveAsync();
            return GetEventModel(eventToAdd);
        }

        public async Task<EventModelFull> GetFullEventAsync(Guid id)
        {
            var _event = await eventRepository.GetAll().Include(e => e.Categories).ThenInclude(c => c.WeightDivisions).FirstOrDefaultAsync(e => e.EventId == id);
            return _event != null ? GetEventModel(_event) : null;
        }



        public async Task<List<EventModelBase>> GetEventsForOwnerAsync(string userId)
        {
            var models = await eventRepository.GetAll().
                Where(e => e.OwnerId == userId).
                Select(e => new EventModelBase { EventId = e.EventId, EventDate = e.EventDate, RegistrationEndTS = e.RegistrationEndTS, EarlyRegistrationEndTS = e.EarlyRegistrationEndTS, RegistrationStartTS = e.RegistrationStartTS, Title = e.Title })
                .ToListAsync();

            return models;
        }

        public async Task<EventModelFull> GetEventByPrefixAsync(string prefix)
        {
            var _event = await eventRepository.GetAll().Include(e => e.Categories).ThenInclude(c => c.WeightDivisions).FirstOrDefaultAsync(e => e.UrlPrefix == prefix);
            if (_event != null)
            {
                return GetEventModel(_event);
            }
            else
            {
                return null;
            }
        }

        public async Task<bool> IsPrefixExistAsync(string prefix)
        {
            return await eventRepository.GetAll().AnyAsync();
        }

        public async Task AddEventImageAsync(Stream stream, string eventId)
        {
            var fileName = FilePath.EVENT_IMAGE_FILE;
            var path = Path.Combine(FilePath.EVENT_DATA_FOLDER, eventId, FilePath.EVENT_IMAGE_FOLDER, FilePath.EVENT_IMAGE_FILE);
            var _event = await this.eventRepository.GetByIDAsync(new Guid(eventId));
            await fileService.SaveImageAsync(path, stream, fileName);
            _event.ImgPath = path;
            eventRepository.Update(_event);
            await unitOfWork.SaveAsync();
        }

        public async Task SaveEventTncAsync(Stream stream, string eventId, string fileName)
        {
            var path = Path.Combine(FilePath.EVENT_DATA_FOLDER, eventId, FilePath.EVENT_TNC_FOLDER, fileName);
            await fileService.SaveFileAsync(path, stream);
            var _event = await eventRepository.GetByIDAsync(new Guid(eventId));
            _event.TNCFilePath = path;
            eventRepository.Update(_event);
            await unitOfWork.SaveAsync();
        }

        public async Task SavePromoCodeListAsync(Stream stream, string eventId)
        {
            var path = Path.Combine(FilePath.EVENT_DATA_FOLDER, eventId, FilePath.EVENT_TNC_FOLDER, FilePath.EVENT_PROMOCODE_LIST_FILE);
            await fileService.SaveFileAsync(path, stream);
            var _event = await eventRepository.GetByIDAsync(new Guid(eventId));
            _event.PromoCodeListPath = path;
            eventRepository.Update(_event);
            await unitOfWork.SaveAsync();
        }

        public async Task<Guid?> GetEventIdAsync(string url)
        {
            var ids = await eventRepository.GetAll().Where(e => e.UrlPrefix == url).Select(e => e.EventId).ToListAsync();
            if (ids.Any())
            {
                return ids.FirstOrDefault();
            }
            else
            {
                return null;
            }

        }

        public async Task<string> GetEventOwnerIdAsync(Guid eventId)
        {
            var existingEvent = await eventRepository.GetByIDAsync<Guid>(eventId);
            if (existingEvent != null)
            {
                return existingEvent.OwnerId;
            }
            else
            {
                return "";
            }
        }

        public async Task<int> GetPrice(Guid eventId, string userId)
        {
            var _event = await eventRepository.GetByIDAsync(eventId);
            if (_event != null)
            {
                var isMember = await federationMembershipRepository.GetAll().Where(fm => fm.UserId == userId && fm.FederationId == _event.FederationId).AnyAsync();
                var dateNow = DateTime.UtcNow;
                if (dateNow <= _event.EarlyRegistrationEndTS)
                {
                    return isMember ? _event.EarlyRegistrationPriceForMembers : _event.EarlyRegistrationPrice;
                }
                else
                {
                    return isMember ? _event.LateRegistrationPriceForMembers : _event.LateRegistrationPrice;
                }
            }
            return 0;
        }

        public async Task<int> GetPrice(Guid eventId, string userId, string promoCode = "")
        {
            var _event = await eventRepository.GetByIDAsync(eventId);
            if (_event != null)
            {
                var isPromoCodeUsed = false;
                if (string.IsNullOrEmpty(promoCode))
                {
                    isPromoCodeUsed = await promoCodeService.ValidateCodeAsync(eventId, promoCode, userId);
                }
                var dateNow = DateTime.UtcNow;
                if (dateNow <= _event.EarlyRegistrationEndTS)
                {
                    return isPromoCodeUsed ? _event.EarlyRegistrationPriceForMembers : _event.EarlyRegistrationPrice;
                }
                else
                {
                    return isPromoCodeUsed ? _event.LateRegistrationPriceForMembers : _event.LateRegistrationPrice;
                }
            }
            return 0;
        }


        public async Task<int> GetPrice(Guid eventId, bool specialPrice)
        {
            var _event = await eventRepository.GetByIDAsync(eventId);
            if (_event != null)
            {
                var dateNow = DateTime.UtcNow;
                if (dateNow <= _event.EarlyRegistrationEndTS)
                {
                    return specialPrice ? _event.EarlyRegistrationPriceForMembers : _event.EarlyRegistrationPrice;
                }
                else
                {
                    return specialPrice ? _event.LateRegistrationPriceForMembers : _event.LateRegistrationPrice;
                }
            }
            return 0;
        }


        public async Task<EventModelBase> GetEventBaseInfoAsync(Guid id)
        {
            var model = await eventRepository.GetAll().
            Where(e => e.EventId == id).
            Select(e => new EventModelBase { EventId = e.EventId, EventDate = e.EventDate, RegistrationEndTS = e.RegistrationEndTS, EarlyRegistrationEndTS = e.EarlyRegistrationEndTS, RegistrationStartTS = e.RegistrationStartTS, Title = e.Title })
            .FirstOrDefaultAsync();
            return model;
        }

        public async Task<EventModelFull> GetEventInfoAsync(Guid id)
        {
            var _event = await eventRepository.GetAll().Include(e => e.Categories).ThenInclude(c => c.WeightDivisions).FirstOrDefaultAsync(e => e.EventId == id);
            if (_event != null)
            {
                return GetEventModel(_event);
            }
            else
            {
                return null;
            }
        }



        #region Helpers

        private EventModelFull GetEventModel(Event _event)
        {
            return new EventModelFull()
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
                UrlPrefix = _event.UrlPrefix,
                VKLink = _event.VKLink,
                EarlyRegistrationEndTS = _event.EarlyRegistrationEndTS,
                EarlyRegistrationPrice = _event.EarlyRegistrationPrice,
                EarlyRegistrationPriceForMembers = _event.EarlyRegistrationPriceForMembers,
                LateRegistrationPrice = _event.EarlyRegistrationPriceForMembers,
                LateRegistrationPriceForMembers = _event.LateRegistrationPriceForMembers,
                PromoCodeEnabled = _event.PromoCodeEnabled,
                PromoCodeListPath = _event.PromoCodeListPath,
                CategoryModels = GetCategoryModels(_event.Categories)
            };
        }

        private EventModelInfo GetEventModelInfo(Event _event)
        {
            return new EventModelInfo()
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
                foreach (var category in categories)
                {
                    categoryModels.Add(new CategoryModel()
                    {
                        CategoryId = category.CategoryId.ToString(),
                        Name = category.Name,
                        WeightDivisionModels = GetWeightDeivisionsModels(category.WeightDivisions),
                        EventId = category.EventId
                    });
                }
            }
            return categoryModels;
        }

        private ICollection<WeightDivisionModel> GetWeightDeivisionsModels(ICollection<WeightDivision> weightDivisions)
        {
            var weightDivisionModels = new List<WeightDivisionModel>();
            if (weightDivisions != null)
            {
                foreach (var weightDivision in weightDivisions)
                {
                    weightDivisionModels.Add(new WeightDivisionModel()
                    {
                        WeightDivisionId = weightDivision.WeightDivisionId.ToString(),
                        Weight = weightDivision.Weight,
                        Descritpion = weightDivision.Descritpion,
                        Name = weightDivision.Name,
                        CategoryId = weightDivision.CategoryId.ToString()
                    });
                }
            }
            return weightDivisionModels;
        }

        private Event GetEventFromModel(EventModelFull _eventModel)
        {
            return new Event()
            {
                EventId = _eventModel.EventId,
                EventDate = _eventModel.EventDate,
                AdditionalData = _eventModel.AdditionalData,
                Address = _eventModel.Address,
                ContactEmail = _eventModel.ContactEmail,
                Description = _eventModel.Description,
                ImgPath = _eventModel.ImgPath,
                TNCFilePath = _eventModel.TNCFilePath,
                ContactPhone = _eventModel.ContactPhone,
                Title = _eventModel.Title,
                FBLink = _eventModel.FBLink,
                RegistrationEndTS = _eventModel.RegistrationEndTS,
                RegistrationStartTS = _eventModel.RegistrationStartTS,
                UrlPrefix = _eventModel.UrlPrefix,
                VKLink = _eventModel.VKLink,
                PromoCodeEnabled = _eventModel.PromoCodeEnabled,
                LateRegistrationPrice = _eventModel.LateRegistrationPrice,
                EarlyRegistrationPriceForMembers = _eventModel.EarlyRegistrationPriceForMembers,
                EarlyRegistrationPrice = _eventModel.EarlyRegistrationPriceForMembers,
                EarlyRegistrationEndTS = _eventModel.EarlyRegistrationEndTS,
                Categories = GetCategoriesFromModels(_eventModel.CategoryModels)
            };
        }

        private ICollection<Category> GetCategoriesFromModels(ICollection<CategoryModel> models)
        {
            var categories = new List<Category>();
            foreach (var model in models)
            {
                categories.Add(new Category()
                {
                    EventId = model.EventId,
                    CategoryId = Guid.Parse(model.CategoryId),
                    Name = model.Name,
                    WeightDivisions = GetWeightDeivisionsFromModels(model.WeightDivisionModels)
                });
            }
            return categories;
        }

        private ICollection<WeightDivision> GetWeightDeivisionsFromModels(ICollection<WeightDivisionModel> models)
        {
            var weightDivisions = new List<WeightDivision>();
            foreach (var model in models)
            {
                weightDivisions.Add(new WeightDivision()
                {
                    WeightDivisionId = Guid.Parse(model.WeightDivisionId),
                    Weight = model.Weight,
                    Descritpion = model.Descritpion,
                    Name = model.Name,
                    CategoryId = Guid.Parse(model.CategoryId)
                });
            }
            return weightDivisions;
        }

        private Event UpdateFromModel(Event _event, EventModelFull eventModel)
        {
            _event.EventDate = eventModel.EventDate;
            _event.AdditionalData = eventModel.AdditionalData;
            _event.Address = eventModel.Address;
            _event.ContactEmail = eventModel.ContactEmail;
            _event.Description = eventModel.Description;
            _event.ImgPath = eventModel.ImgPath;
            _event.TNCFilePath = eventModel.TNCFilePath;
            _event.ContactPhone = eventModel.ContactPhone;
            _event.Title = eventModel.Title;
            _event.FBLink = eventModel.FBLink;
            _event.RegistrationEndTS = eventModel.RegistrationEndTS;
            _event.RegistrationStartTS = eventModel.RegistrationStartTS;
            _event.UrlPrefix = eventModel.UrlPrefix;
            _event.VKLink = eventModel.VKLink;
            _event.EarlyRegistrationEndTS = eventModel.EarlyRegistrationEndTS;
            _event.EarlyRegistrationPrice = eventModel.EarlyRegistrationPrice;
            _event.EarlyRegistrationPriceForMembers = eventModel.EarlyRegistrationPriceForMembers;
            _event.LateRegistrationPrice = eventModel.EarlyRegistrationPriceForMembers;
            _event.LateRegistrationPriceForMembers = eventModel.LateRegistrationPriceForMembers;
            _event.PromoCodeEnabled = eventModel.PromoCodeEnabled;
            _event.PromoCodeListPath = eventModel.PromoCodeListPath;
            return _event;
        }


        private async Task DeleteCategoriesAsync(Guid eventId, IEnumerable<Guid> eventCategoryIds)
        {
            var categoriesToDelete = await categoryRepository.GetAll().Where(c => c.EventId == eventId && !eventCategoryIds.Contains(c.CategoryId)).ToListAsync();
            var weightDivisionsToDelete = await weightDivisionRepository.GetAll().Where(wd => categoriesToDelete.Select(c => c.CategoryId).Contains(wd.CategoryId)).ToListAsync();
            weightDivisionRepository.DeleteRange(weightDivisionsToDelete);
            categoryRepository.DeleteRange(categoriesToDelete);
        }

        #endregion


    }

}

