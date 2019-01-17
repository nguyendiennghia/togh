using Abp.Domain.Entities;
using System;

namespace EventCloud.Repository.Entity
{
    public class Location// : Entity<Guid>
    {
        /// <summary>
        /// Could be any: post code, address, city name, stadium name etc. input initially and reshown up as mask later on
        /// </summary>
        public virtual string RawAddress { get; set; }

        public double Longtitude { get; set; }

        public double Latitude { get; set; }
    }
}