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

namespace TRNMNT.Core.Services
{
    public class EventService : IEventService
    {
        private IRepository<Event> eventRepository;
        private IFileService fileService;

        public EventService(IRepository<Event> eventRepository, IFileService fileservice)
        {
            this.eventRepository = eventRepository;
            this.fileService = fileservice;
        }

        public async Task SaveEventAsync(Event eventToSave, string userId)
        {
            eventToSave.OwnerId = userId;
            eventToSave.UpdateTS = DateTime.UtcNow;
            eventToSave.IsActive = true;
            if (await eventRepository.GetAll().AnyAsync(e => e.EventId == eventToSave.EventId))
            {
                eventRepository.Update(eventToSave);
            }
            else
            {
                eventRepository.Add(eventToSave);
            }
            await eventRepository.SaveAsync();
        }

        public async Task<Event> GetFullEventAsync(Guid id)
        {
            return await eventRepository.GetAll().Include(e => e.Categories).ThenInclude(c => c.WeightDivisions).FirstOrDefaultAsync(e => e.EventId == id);
        }
        public async Task<Event> GetEventAsync(Guid id)
        {
            return await eventRepository.GetByIDAsync(id);
        }

        public async Task<List<Event>> GetEventsForOwnerAsync(string userId)
        {
            return await eventRepository.GetAll().Where(e => e.OwnerId == userId).ToListAsync();
        }

        public async Task<Event> GetEventByPrefixAsync(string prefix)
        {
            return await eventRepository.GetAll().FirstOrDefaultAsync(e => e.UrlPrefix == prefix);
        }

        public async Task<bool> IsPrefixExistAsync(string prefix)
        {
            return await eventRepository.GetAll().AnyAsync();
        }

        public async Task AddEventImageAsync(Stream stream, string eventId)
        {
            var fileName = FilePath.EVENT_IMAGE_FILE;
            var path = Path.Combine(FilePath.EVENT_DATA_FOLDER, eventId, FilePath.EVENT_IMAGE_FOLDER, FilePath.EVENT_IMAGE_FILE);
            var _event =  await GetEventAsync(new Guid(eventId));
            await fileService.SaveImageAsync(path, stream, fileName);
            _event.ImgPath = path;
            eventRepository.Update(_event);
            await eventRepository.SaveAsync();
        }

        public async Task SaveEventTncAsync(Stream stream, string eventId, string fileName)
        {
            var path = Path.Combine(FilePath.EVENT_DATA_FOLDER, eventId, FilePath.EVENT_IMAGE_FOLDER, fileName);
            await fileService.SaveFileAsync(path, stream);
            var _event = await GetEventAsync(new Guid(eventId));
            _event.TNCFilePath = path;
            eventRepository.Update(_event);
            await eventRepository.SaveAsync();
        }

        public async Task<string> GetEventIdAsync(string url)
        {
            return (await eventRepository.GetAll().Where(e => e.UrlPrefix == url).Select(e => e.EventId).FirstOrDefaultAsync()).ToString();
        }

        public async Task<Event> CreateEventAsync()
        {
            var _event = new Event() { StatusId = (int)EventStatusEnum.Init, IsActive = false };
            eventRepository.Add(_event);
            await eventRepository.SaveAsync();
            return _event;
        }
    }
}
