﻿namespace ASP.Data.Entities
{
    public class Location
    {
        public Guid Id { get; set; }
        public Guid CategoryId { get; set; }
        public Guid? CountryId { get; set; }
        public Guid? CityId { get; set; }
        public string? Address { get; set; } = null!;
        public int? Stars { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateTime? DeletedDt { get; set; }
        public String? PhotoUrl { get; set; }
    }
}
