using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using EventCloud.Repository.Entity;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace EventCloud.Repository
{
    /// <summary>
    /// TODO: Generic CRUD MongoRepository for input POCO <see cref="Event"/>
    /// </summary>
    public class MongoEventRepository : IEventRepository
    {
        private readonly Context _context = null;

        public MongoEventRepository(IOptions<Settings> settings)
        {
            _context = new Context(settings);
        }

        /// <summary>
        /// TODO: In some Exception handler instead
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        private static async Task<T> Throws<T>(Exception exception) => await Task.Run(new Func<Task<T>>(() => throw exception));

        public async Task<bool> DeleteAsync(Event @event)
        {
            return await Throws<bool>(new NotImplementedException());
        }

        public async Task<IList<Event>> GetByAsync()
        {
            //return await Throws<IList<Event>>(new NotImplementedException());
            return await _context.Events.Find(_ => true).ToListAsync();
        }

        public async Task<Event> GetByAsync(string id)
        {
            //return await Throws<Event>(new NotImplementedException());
            var internalId = GetInternalId(id);
            return await _context.Events
                            .Find(e => e.Id == id || e.InternalId == internalId)
                            .FirstOrDefaultAsync();
        }

        public async Task<bool> SaveAsync(Event @event)
        {
            //return await Throws<bool>(new NotImplementedException());

            if (@event.InternalId == ObjectId.Empty)
            {
                @event.CreatedAt = @event.UpdatedAt = DateTime.Now;
                await _context.Events.InsertOneAsync(@event);
                return true;
            }

            var filter = Builders<Event>.Filter.Eq(s => s.Id, @event.Id);
            var update = Builders<Event>.Update
                            .Set(e => e.Availabilities, @event.Availabilities)
                            .Set(e => e.Category, @event.Category)
                            .Set(e => e.Location, @event.Location)
                            .CurrentDate(e => e.UpdatedAt);
            var result = await _context.Events.UpdateOneAsync(filter, update);
            return result.IsAcknowledged && result.ModifiedCount > 0; 
        }

        private ObjectId GetInternalId(string id)
        {
            if (!ObjectId.TryParse(id, out ObjectId internalId))
                internalId = ObjectId.Empty;

            return internalId;
        }
    }
}
