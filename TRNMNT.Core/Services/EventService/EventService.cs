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
            return await eventRepository.GetAll().Include(e => e.Categories).ThenInclude(c => c.WeightDivisions).FirstOrDefaultAsync();
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

        public async Task SaveEventImageAsync(Stream stream, string eventId)
        {
            var path = Path.Combine(FilePath.EVENT_DATA_FOLDER, eventId, FilePath.EVENT_IMAGE_FOLDER, FilePath.EVENT_IMAGE_FILE);
            await fileService.SaveImageAsync(path,stream);
        }

        public async Task SaveEventTncAsync(Stream stream, string eventId, string fileName)
        {
            var path = Path.Combine(FilePath.EVENT_DATA_FOLDER, eventId, FilePath.EVENT_IMAGE_FOLDER, fileName);
            await fileService.SaveFileAsync(path, stream);
        }
    }
}
