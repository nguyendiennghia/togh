using EventCloud.Repository.Entity;
using Microsoft.Extensions.Options;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
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
            // Tons of shit 'cause too lazy to register with Abp dependence
            IMongoClient client = new MongoClient(settings.Value.ConnectionString);
            if (client != null)
                _database = client.GetDatabase(settings.Value.Database);

            //var convention = new ConventionPack
            //{
            //    new MemberSerializationOptionsConvention(
            //        typeof (Guid),
            //        new RepresentationSerializationOptions(BsonType.String)
            //)
            //};

            //ConventionRegistry.Register("xxx", convention, t => true);

            //BsonClassMap.RegisterClassMap<Event>(cm =>
            //{
            //    cm.AutoMap();
            //    cm.MapMember(x => x.CreatedAt).SetDefaultValue(() => DateTime.Now);
            //    cm.MapMember(x => x.UpdatedAt).SetDefaultValue(() => DateTime.Now);
            ////    cm.ConventionPack.SetSerializationOptionsConvention
            //});
        }

        public IMongoCollection<Event> Events
            =>_database.GetCollection<Event>("event");
    }
}
