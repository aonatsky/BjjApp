using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using TRNMNT.Data.Entities;

namespace TRNMNT.Core.Services
{
    public interface IEventService
    {
        Task SaveEventAsync(Event eventToAdd, string userId);
        Task<Event> GetFullEventAsync(Guid id);
        Task<Event> GetEventAsync(Guid id);
        Task<Event> GetEventByPrefixAsync(string prefix);
        Task<List<Event>> GetEventsForOwnerAsync(string userId);
        Task<bool> IsPrefixExistAsync(string prefix);
        Task SaveEventImageAsync(Stream stream, string eventId);
        Task SaveEventTncAsync(Stream stream, string eventId, string fileName);
    }
}
