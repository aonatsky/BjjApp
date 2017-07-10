using log4net.Core;
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
        private IRepository<Event> _eventRepository;

        public EventService(IRepository<Event> eventRepository) 
        {
            _eventRepository = eventRepository;
        }


        public async Task AddEventAsync(Event eventToAdd)
        {
            _eventRepository.Add(eventToAdd);
            await _eventRepository.SaveAsync(); 
        }

        public async Task<Event> GetEventAsync(Guid id)
        {
            return await _eventRepository.GetByIDAsync(id);
        }
    }
}
