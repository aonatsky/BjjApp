using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using TRNMNT.Core.Model.Event;

namespace TRNMNT.Core.Services
{
    public interface IEventService
    {
        Task UpdateEventAsync(EventModel eventModel);
        Task <EventModel>GetNewEventAsync(string userId);
        Task<EventModel> GetFullEventAsync(Guid id);
        Task<EventModel> GetEventByPrefixAsync(string prefix);
        Task<List<EventModelBase>> GetEventsForOwnerAsync(string userId);
        Task<EventModelBase> GetEventBaseInfoAsync(Guid id);
        Task<bool> IsPrefixExistAsync(string prefix);
        Task AddEventImageAsync(Stream stream, string eventId);
        Task SaveEventTncAsync(Stream stream, string eventId, string fileName);
        Task SavePromoCodeListAsync(Stream stream, string eventId);
        Task <string>GetEventIdAsync(string url);
        Task<string> GetEventOwnerIdAsync(Guid eventId);
        Task<int> GetPrice(Guid EventId, string userId);
    }
}
