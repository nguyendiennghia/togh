using Abp.Domain.Entities.Auditing;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EventCloud.Repository.Entity
{
    /// <summary>
    /// Extension of Event in EventCloud.Core. This will be used as terminal for Event linked to MongoDB besides the core original EF
    /// </summary>
    public class Event// : FullAuditedEntity<Guid>
    {
        /// <summary>
        /// Internal purpose only
        /// </summary>
        [BsonId]
        [BsonElement("id")]
        public ObjectId InternalId { get; set; }

        [BsonElement("cat")]
        public virtual Category Category { get; set; }

        [BsonElement("loc")]
        [BsonRequired]
        public virtual Location Location { get; set; }

        [BsonElement("avai")]
        [BsonRequired]
        public virtual IList<Availability> Availabilities { get; set; }

        [BsonElement("created")]
        public DateTime CreatedAt { get; set; }

        [BsonElement("updated")]
        public DateTime UpdatedAt { get; set; }

        /// <summary>
        /// External ID from the origin likewise SQL
        /// </summary>
        [BsonElement("uid")]
        [BsonRepresentation(BsonType.String)]
        public Guid ExternalId { get; set; }
    }

    public class SingleEvent : Event
    {
        [MaxLength(1)]
        [MinLength(1)]
        public override IList<Availability> Availabilities { get; set; }
    }

    public class ReoccurenceEvent : Event
    {
        [MaxLength(int.MaxValue)]
        [MinLength(1)]
        public override IList<Availability> Availabilities { get; set; }
    }

    public class GuidSerializer : IBsonSerializer
    {
        public Type ValueType => typeof(Guid);

        public object Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            throw new NotImplementedException();
        }

        public void Serialize(BsonSerializationContext context, BsonSerializationArgs args, object value)
        {
            throw new NotImplementedException();
        }
    }
}
