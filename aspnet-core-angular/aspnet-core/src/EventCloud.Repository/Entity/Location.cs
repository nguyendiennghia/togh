using Abp.Domain.Entities;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace EventCloud.Repository.Entity
{
    public class Location// : Entity<Guid>
    {
        /// <summary>
        /// Could be any: post code, address, city name, stadium name etc. input initially and reshown up as mask later on
        /// </summary>
        [BsonElement("add")]
        public virtual string RawAddress { get; set; }

        [BsonElement("lng")]
        public double Longitude { get; set; }

        [BsonElement("lat")]
        public double Latitude { get; set; }
    }
}