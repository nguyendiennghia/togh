using Abp.Domain.Entities.Auditing;
using MongoDB.Bson;
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
        [BsonId]
        public ObjectId InternalId { get; set; }

        public string Id { get; set; }

        public virtual Category Category { get; set; }

        public virtual Location Location { get; set; }

        public virtual IList<Availability> Availabilities { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public ObjectId ExternalId { get; set; }
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
}
