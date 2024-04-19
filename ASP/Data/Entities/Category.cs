namespace ASP.Data.Entities
{
    public class Category
    {
        public Guid id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateTime? DeletedDt { get; set; }
        public String? PhotoUrl { get; set; }
        public String? Slug { get; set; }
    }
}
