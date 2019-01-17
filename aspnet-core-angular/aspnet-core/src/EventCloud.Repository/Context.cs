using EventCloud.Repository.Entity;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventCloud.Repository
{
    public class Settings
    {
        public string ConnectionString;
        public string Database;
    }

    public class Context
    {
        private readonly IMongoDatabase _database = null;

        public Context(IOptions<Settings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            if (client != null)
                _database = client.GetDatabase(settings.Value.Database);
        }

        public IMongoCollection<Event> Events
        {
            get
            {
                return _database.GetCollection<Event>("event");
            }
        }
    }
}
