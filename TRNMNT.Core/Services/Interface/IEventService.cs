using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using TRNMNT.Core.Model;
using TRNMNT.Core.Model.Event;

namespace TRNMNT.Core.Services.Interface
{
    public interface IEventService
    {
        /// <summary>
        /// Updates event
        /// </summary>
        /// <param name="eventModel">Event model</param>
        /// <returns></returns>
        Task UpdateEventAsync(EventModelFull eventModel);

        /// <summary>
        /// Gets the full event asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        Task<EventModelFull> GetFullEventModelAsync(Guid id);

        /// <summary>
        /// Gets the event information asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        Task<EventModelFull> GetEventInfoAsync(Guid? id);

        /// <summary>
        /// Gets the events for owner asynchronous.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="role">Role.</param>
        /// <returns></returns>
        Task<List<EventModelBase>> GetEventsForOwnerAsync(string userId, string role);

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
        Task<bool> IsEventUrlPrefixExistAsync(string prefix);

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
        Task<Guid?> GetEventIdAsync(string url);

        /// <summary>
        /// Get owners user id
        /// </summary>
        /// <param name="eventId">Event id</param>
        /// <returns></returns>
        Task<string> GetEventOwnerIdAsync(Guid eventId);

        /// <summary>
        /// Returns price model for an event based on user.
        /// </summary>
        /// <param name="EventId"></param>
        /// <param name="userId"></param>
        /// <returns>Price</returns>
        Task<PriceModel> GetPriceAsync(Guid EventId, string userId);

        /// <summary>
        /// Creates new event to edit.
        /// </summary>
        /// <param name="userId">Owner Id</param>
        /// <param name="federationId">FederationId Id</param>
        /// <returns>Event Id</returns>
        Guid CreateEvent(string userId, Guid federationId);

        /// <summary>
        /// Sets event inactive.
        /// </summary>
        /// <param name="eventId">Event id.</param>
        /// <returns></returns>
        Task DeleteEventAsync(string eventId);
    }
}