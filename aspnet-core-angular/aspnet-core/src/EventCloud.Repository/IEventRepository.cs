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
        Task<IList<Event>> GetByAsync(IList<Guid> eventIds);
        Task<Event> GetByAsync(string id);
        Task<Event> GetByAsync(Guid eventId);

        Task<bool> SaveAsync(Event @event);

        Task<bool> DeleteAsync(Event @event);
    }
}
