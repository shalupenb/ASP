namespace ASP.Data.Entities
{
    public class Reservation
    {
        public Guid Id { get; set; }
        public Guid RoomId { get; set; }
        public Guid UserId { get; set; }
        public DateTime Date {  get; set; }

        // NAVIGATION PROPS
        public User User { get; set; }
        public Room Room { get; set; }

    }
}
