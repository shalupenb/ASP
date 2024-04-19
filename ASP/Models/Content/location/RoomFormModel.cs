using Microsoft.AspNetCore.Mvc;

namespace ASP.Models.Content.location
{
    public class RoomFormModel
    {
        [FromForm(Name = "location-id")]
        public Guid LocationId { get; set; }
        [FromForm(Name = "room-name")]
        public string Name { get; set; } = null!;
        [FromForm(Name = "room-description")]
        public string Description { get; set; } = null!;
        [FromForm(Name = "room-slug")]
        public string Slug { get; set; } = null!;

        [FromForm(Name = "room-stars")]
        public int Stars { get; set; }

        [FromForm(Name = "room-photo")]
        public IFormFile Photo { get; set; } = null!;
    }
}
