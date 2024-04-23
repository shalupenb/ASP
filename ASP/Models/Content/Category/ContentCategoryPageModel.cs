namespace ASP.Models.Content.Category
{
    public class ContentCategoryPageModel
    {
        public Data.Entities.Category Category { get; set; } = null!;
        public List<Data.Entities.Location> Locations { get; set; } = [];
    }
}
