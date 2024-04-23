namespace ASP.Data.Entities
{
    public class Room
    {
        public Guid Id { get; set; }
        public Guid LocationId { get; set; }
        public int? Stars { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateTime? DeleteDt { get; set; }  // ознака видалення/доступностi
        public String? PhotoUrl { get; set; }
        public Double DailyPrice { get; set; }

        public String? Slug { get; set; }

        public List<Reservation> Reservations { get; set; }
    }
}
