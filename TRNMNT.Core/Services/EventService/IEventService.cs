using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using TRNMNT.Core.Model.Event;

namespace TRNMNT.Core.Services
{
    public interface IEventService
    {
        Task UpdateEventAsync(EventModelFull eventModel);
        Task <EventModelFull>GetNewEventAsync(string userId);
        Task<EventModelFull> GetFullEventAsync(Guid id);
        Task<EventModelFull> GetEventByPrefixAsync(string prefix);
        Task<EventModelFull> GetEventInfoAsync(Guid id);
        Task<List<EventModelBase>> GetEventsForOwnerAsync(string userId);
        Task<EventModelBase> GetEventBaseInfoAsync(Guid id);

        Task<bool> IsPrefixExistAsync(string prefix);

        Task AddEventImageAsync(Stream stream, string eventId);
        Task SaveEventTncAsync(Stream stream, string eventId, string fileName);
        Task SavePromoCodeListAsync(Stream stream, string eventId);
        Task <Guid?>GetEventIdAsync(string url);
        Task<string> GetEventOwnerIdAsync(Guid eventId);
        Task<int> GetPrice(Guid EventId, string userId);
    }
}
