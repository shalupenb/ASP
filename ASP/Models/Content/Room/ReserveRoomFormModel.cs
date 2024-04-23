namespace ASP.Models.Content.Room
{
    public class ReserveRoomFormModel
    {
        public Guid RoomId { get; set; }
        public Guid UserId { get; set; }
        public DateTime Date {  get; set; }
    }
}
