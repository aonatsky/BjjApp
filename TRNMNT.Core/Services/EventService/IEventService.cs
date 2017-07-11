using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TRNMNT.Data.Entities;

namespace TRNMNT.Core.Services
{
    public interface IEventService
    {
        Task SaveEventAsync(Event eventToAdd, string userId);
        Task<Event> GetEventAsync(Guid id);
        Task<Event> GetEventByPrefixAsync(string prefix);
        Task<List<Event>> GetEventsForOwnerAsync(string userId);
        Task<bool> IsPrefixExistAsync(string prefix);
    }
}
