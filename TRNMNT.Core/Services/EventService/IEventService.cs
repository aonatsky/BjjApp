using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using TRNMNT.Core.Model;
using TRNMNT.Data.Entities;

namespace TRNMNT.Core.Services
{
    public interface IEventService
    {
        Task UpdateEventAsync(EventModel eventModel);
        Task <Event>GetNewEventAsync(string userId);
        Task<Event> GetFullEventAsync(Guid id);
        Task<Event> GetEventAsync(Guid id);
        Task<Event> GetEventByPrefixAsync(string prefix);
        Task<List<Event>> GetEventsForOwnerAsync(string userId);
        Task<bool> IsPrefixExistAsync(string prefix);
        Task AddEventImageAsync(Stream stream, string eventId);
        Task SaveEventTncAsync(Stream stream, string eventId, string fileName);
        Task <string>GetEventIdAsync(string url);
        Task <Event>CreateEventAsync();
        Task<string> GetEventOwnerIdAsync(Guid eventId);
    }
}
