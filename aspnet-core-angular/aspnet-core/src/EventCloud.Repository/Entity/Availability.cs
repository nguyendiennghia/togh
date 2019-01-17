using Abp.Domain.Entities.Auditing;
using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Text;
using Abp.Domain.Entities;

namespace EventCloud.Repository.Entity
{
    public class Availability// : FullAuditedEntity<Guid>
    {
        [Range(1, int.MaxValue)]
        public virtual int Size { get; set; }

        public DateTime At { get; set; }
    }

    //public class OneTimeAvailability : Availability
    //{
    //    public DateTime At { get; set; }
    //}

    //public class WeeklyAvailability : Availability
    //{
    //    public DateTime At { get; set; }
    //}

    //public class ReoccurenceAvailability : Availability
    //{ }
}
