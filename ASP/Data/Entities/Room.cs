namespace ASP.Data.Entities
{
    public class Room
    {
        public Guid Id { get; set; }
        public Guid LocationId { get; set; }
        public int? Stars { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateTime? DeletedDt { get; set; }
    }
}
