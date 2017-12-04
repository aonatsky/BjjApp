using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using TRNMNT.Core.Model.Event;

namespace TRNMNT.Core.Services
{
    public interface IEventService
    {
        /// <summary>
        /// Updates event
        /// </summary>
        /// <param name="eventModel">Event model</param>
        /// <returns></returns>
        Task UpdateEventAsync(EventModelFull eventModel);
        Task<EventModelFull> GetFullEventAsync(Guid id);
        Task<EventModelFull> GetEventInfoAsync(Guid id);
        Task<List<EventModelBase>> GetEventsForOwnerAsync(string userId);
        /// <summary>
        /// Get base event information
        /// </summary>
        /// <param name="id">Event Id</param>
        /// <returns></returns>
        Task<EventModelBase> GetEventBaseInfoAsync(Guid id);

        /// <summary>
        /// Checks is event url prefix is already used
        /// </summary>
        /// <param name="prefix">URL prefix</param>
        /// <returns>True if prefix is already used</returns>
        Task<bool> IsEventUrlPrefixExist(string prefix);

        /// <summary>
        /// Saves event image
        /// </summary>
        /// <param name="stream">Stream with image</param>
        /// <param name="eventId">Event Id</param>
        /// <returns></returns>
        Task SaveEventImageAsync(Stream stream, string eventId);
        /// <summary>
        /// Saves Event Tnc
        /// </summary>
        /// <param name="stream">Stream with tnc file</param>
        /// <param name="eventId">Event Id</param>
        /// <param name="fileName">File name</param>
        /// <returns></returns>
        Task SaveEventTncAsync(Stream stream, string eventId, string fileName);
        /// <summary>
        /// Saves promocode list for an event
        /// </summary>
        /// <param name="stream">Stream with code list file</param>
        /// <param name="eventId">Event Id</param>
        /// <returns></returns>
        Task SavePromoCodeListAsync(Stream stream, string eventId);
        /// <summary>
        /// Gets event id by URL prefix
        /// </summary>
        /// <param name="url">URL prefix</param>
        /// <returns>Event id or NULL if not exist</returns>
        Task <Guid?>GetEventIdAsync(string url);
        /// <summary>
        /// Get owners user id
        /// </summary>
        /// <param name="eventId">Event id</param>
        /// <returns></returns>
        Task<string> GetEventOwnerIdAsync(Guid eventId);
        /// <summary>
        /// Get prive for an event based on user
        /// </summary>
        /// <param name="EventId"></param>
        /// <param name="userId"></param>
        /// <returns>Price</returns>
        Task<int> GetPrice(Guid EventId, string userId);
        /// <summary>
        /// Returns price based on registration dates. And promocode (optional)
        /// </summary>
        /// <param name="EventId"></param>
        /// <returns></returns>
        Task<int> GetPrice(Guid eventId, string userId, string promocode = "");
        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventId">Event Id</param>
        /// <param name="isMember">Is special price</param>
        /// <returns></returns>
        Task<int> GetPrice(Guid eventId, bool specialPrice);
        /// <summary>
        /// Creates new event to edit.
        /// </summary>
        /// <param name="userId">Owner Id</param>
        /// <returns>Event Id</returns>
        Guid CreateEvent(string userId);
    }
}
