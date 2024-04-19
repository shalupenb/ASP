namespace ASP.Models.Content.location
{
	public class ContentLocationPageModel
	{
		public Data.Entities.Location Location { get; set; } = null!;
		public List<Data.Entities.Room> Rooms { get; set; } = [];
	}
}
