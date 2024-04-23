using Microsoft.AspNetCore.Mvc;

namespace ASP.Models.Content.Location
{
    public class RoomFormModel
    {
        [FromForm(Name = "location-id")]
        public Guid LocationId { get; set; }

        [FromForm(Name = "room-name")]
        public String Name { get; set; } = null!;

        [FromForm(Name = "room-description")]
        public String Description { get; set; } = null!;

        [FromForm(Name = "room-slug")]
        public String Slug { get; set; } = null!;

        [FromForm(Name = "room-stars")]
        public int Stars { get; set; }

        [FromForm(Name = "room-price")]
        public Double DailyPrice { get; set; }

        [FromForm(Name = "room-photo")]
        public IFormFile Photo { get; set; } = null!;
    }
}
