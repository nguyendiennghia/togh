using System;
using System.ComponentModel.DataAnnotations;

namespace EventCloud.Events.Dtos
{
    public class CreateEventInput
    {
        [Required]
        [StringLength(Event.MaxTitleLength)]
        public string Title { get; set; }

        [StringLength(Event.MaxDescriptionLength)]
        public string Description { get; set; }

        public DateTime Date { get; set; }

        [Range(0, int.MaxValue)]
        public int MaxRegistrationCount { get; set; }

        public CreateEventLocation Location { get; set; }
    }

    public class CreateEventLocation
    {
        public double Longitude { get; set; }

        public double Latitude { get; set; }

        public string PostCode { get; set; }

        public string Address { get; set; }
    }
}
