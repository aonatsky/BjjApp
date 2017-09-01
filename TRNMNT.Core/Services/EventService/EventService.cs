using log4net.Core;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TRNMNT.Data.Entities;
using TRNMNT.Data.Repositories;
using System.Linq;
using System.IO;
using TRNMNT.Web.Core.Const;
using TRNMNT.Web.Core.Enum;
using TRNMNT.Core.Model.Event;
using TRNMNT.Core.Model.Category;
using TRNMNT.Core.Model;
using TRNMNT.Core.Model.WeightDivision;

namespace TRNMNT.Core.Services
{
    public class EventService : IEventService
    {
        private IRepository<Event> eventRepository;
        private IRepository<Category> categoryRepository;
        private IRepository<WeightDivision> weightDivisionRepository;
        private IFileService fileService;
        private IRepository<FederationMembership> federationMembershipRepository;

        public EventService(
            IRepository<Event> eventRepository,
            IRepository<Category> categoryRepository,
            IRepository<WeightDivision> weightDivisionRepository,
            IRepository<FederationMembership> federationMembershipRepository,
            IFileService fileservice
            )
        {
            this.eventRepository = eventRepository;
            this.categoryRepository = categoryRepository;
            this.weightDivisionRepository = weightDivisionRepository;
            this.federationMembershipRepository = federationMembershipRepository;
            this.fileService = fileservice;
        }

        public async Task UpdateEventAsync(EventModel eventModel)
        {

            var _event = await eventRepository.GetByIDAsync<Guid>(eventModel.EventId);
            _event = UpdateFromModel(_event, eventModel);
            _event.UpdateTS = DateTime.UtcNow;
            _event.IsActive = true;
            eventRepository.Update(_event);

            var categoriesToDelete = await categoryRepository.GetAll().Where(c => c.EventId == _event.EventId && !eventModel.CategoryModels.Select(cm => Guid.Parse(cm.CategoryId)).Contains(c.CategoryId)).ToListAsync();
            categoryRepository.DeleteRange(categoriesToDelete);

            foreach (var categoryModel in eventModel.CategoryModels)
            {
                var category = categoryRepository.GetByID<Guid>(Guid.Parse(categoryModel.CategoryId));
                if (category != null)
                {
                    category.Name = categoryModel.Name;
                    categoryRepository.Update(category);
                    var wdToDelete = await weightDivisionRepository.GetAll().Where(wd => wd.CategoryId == category.CategoryId && !categoryModel.WeightDivisionModels.Select(wdm => Guid.Parse(wdm.WeightDivisionId)).Contains(wd.WeightDivisionId)).ToListAsync();
                    weightDivisionRepository.DeleteRange(wdToDelete);
                }
                else
                {
                    category = new Category() { Name = categoryModel.Name, EventId = categoryModel.EventId };
                    categoryRepository.Add(category);
                }


                foreach (var wdModel in categoryModel.WeightDivisionModels)
                {
                    var wd = weightDivisionRepository.GetByID<Guid>(Guid.Parse(wdModel.WeightDivisionId));
                    if (wd != null)
                    {
                        wd.Name = wdModel.Name;
                        weightDivisionRepository.Update(wd);
                    }
                    else
                    {
                        wd = new WeightDivision() { Name = wdModel.Name, CategoryId = category.CategoryId };
                        weightDivisionRepository.Add(wd);
                    }
                }


            }

            await eventRepository.SaveAsync();
        }

        public async Task<EventModel> GetNewEventAsync(string userId)
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
            await eventRepository.SaveAsync();
            return GetEventModel(eventToAdd);
        }

        public async Task<EventModel> GetFullEventAsync(Guid id)
        {
            var _event = await eventRepository.GetAll().Include(e => e.Categories).ThenInclude(c => c.WeightDivisions).FirstOrDefaultAsync(e => e.EventId == id);
            return GetEventModel(_event);
        }



        public async Task<List<EventModelBase>> GetEventsForOwnerAsync(string userId)
        {
            var models = await eventRepository.GetAll().
                Where(e => e.OwnerId == userId).
                Select(e => new EventModelBase { EventId = e.EventId, EventDate = e.EventDate, RegistrationEndTS = e.RegistrationEndTS, RegistrationStartTS = e.RegistrationStartTS, Title = e.Title })
                .ToListAsync();

            return models;
        }

        public async Task<EventModel> GetEventByPrefixAsync(string prefix)
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
            await eventRepository.SaveAsync();
        }

        public async Task SaveEventTncAsync(Stream stream, string eventId, string fileName)
        {
            var path = Path.Combine(FilePath.EVENT_DATA_FOLDER, eventId, FilePath.EVENT_TNC_FOLDER, fileName);
            await fileService.SaveFileAsync(path, stream);
            var _event = await eventRepository.GetByIDAsync(new Guid(eventId));
            _event.TNCFilePath = path;
            eventRepository.Update(_event);
            await eventRepository.SaveAsync();
        }

        public async Task<string> GetEventIdAsync(string url)
        {
            return (await eventRepository.GetAll().Where(e => e.UrlPrefix == url).Select(e => e.EventId).FirstOrDefaultAsync()).ToString();
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

        #region Helpers

        private EventModel GetEventModel(Event _event)
        {
            return new EventModel()
            {
                EventId = _event.EventId,
                EventDate = _event.EventDate,
                AdditionalData = _event.AdditionalData,
                Address = _event.Address,
                CardNumber = _event.CardNumber,
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
                CategoryModels = GetCategoryModels(_event.Categories)
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
                        WeightDivisionModels = GetWeightDeivisionsModels(category.WeightDivisions)
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
                        Name = weightDivision.Descritpion
                    });
                }
            }
            return weightDivisionModels;
        }

        private Event GetEventFromModel(EventModel _eventModel)
        {
            return new Event()
            {
                EventId = _eventModel.EventId,
                EventDate = _eventModel.EventDate,
                AdditionalData = _eventModel.AdditionalData,
                Address = _eventModel.Address,
                CardNumber = _eventModel.CardNumber,
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
                    Name = model.Descritpion
                });
            }
            return weightDivisions;
        }

        private Event UpdateFromModel(Event _event, EventModel eventModel)
        {
            _event.EventDate = eventModel.EventDate;
            _event.AdditionalData = eventModel.AdditionalData;
            _event.Address = eventModel.Address;
            _event.CardNumber = eventModel.CardNumber;
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
            return _event;


        }


        private void DeleteCategories(List<Category> categories)
        {
            categoryRepository.DeleteRange(categories);
        }

        #endregion


    }

}

