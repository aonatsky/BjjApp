using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TRNMNT.Data.Entities;

namespace TRNMNT.Core.Services
{
    public interface IEventService
    {
        Task AddEventAsync(Event eventToAdd);
        Task<Event> GetEventAsync(Guid id);
        Task<bool> IsPrefixExistAsync(string prefix);
    }
}
