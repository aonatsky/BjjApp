using log4net.Core;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TRNMNT.Data.Entities;
using TRNMNT.Data.Repositories;

namespace TRNMNT.Core.Services
{
    public class EventService : IEventService
    {
        private IRepository<Event> eventRepository;

        public EventService(IRepository<Event> eventRepository)
        {
            this.eventRepository = eventRepository;
        }


        public async Task AddEventAsync(Event eventToAdd)
        {
            eventRepository.Add(eventToAdd);
            await eventRepository.SaveAsync();
        }

        public async Task<Event> GetEventAsync(Guid id)
        {
            return await eventRepository.GetByIDAsync(id);
        }

        public async Task<bool> IsPrefixExistAsync(string prefix)
        {
            return await eventRepository.GetAll().AnyAsync();
        }
    }
}
