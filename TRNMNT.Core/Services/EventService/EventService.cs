using log4net.Core;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TRNMNT.Data.Entities;
using TRNMNT.Data.Repositories;
using System.Linq;

namespace TRNMNT.Core.Services
{
    public class EventService : IEventService
    {
        private IRepository<Event> eventRepository;

        public EventService(IRepository<Event> eventRepository)
        {
            this.eventRepository = eventRepository;
        }

        public async Task SaveEventAsync(Event eventToSave, string userId)
        {
            eventToSave.OwnerId = userId;
            eventToSave.UpdateTS = DateTime.UtcNow;
            eventToSave.IsActive = true;
            if (await eventRepository.GetByIDAsync<Guid>(eventToSave.EventId) != null)
            {
                eventRepository.Update(eventToSave);
            }
            else
            {
                eventRepository.Add(eventToSave);
            }
            await eventRepository.SaveAsync();
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
    }
}
