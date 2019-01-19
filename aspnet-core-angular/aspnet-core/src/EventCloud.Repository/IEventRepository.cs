using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EventCloud.Repository.Entity;

namespace EventCloud.Repository
{
    /// <summary>
    /// CRUD
    /// </summary>
    public interface IEventRepository
    {
        Task<IList<Event>> GetByAsync();
        Task<Event> GetByAsync(string id);
        Task<IList<Event>> GetByAsync(Guid eventId);

        Task<bool> SaveAsync(Event @event);

        Task<bool> DeleteAsync(Event @event);
    }
}
