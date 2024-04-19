namespace ASP.Models.Content.Category
{
	public class ContentCategoryPageModel
	{
		public Data.Entities.Category Category { get; set; }
		public List<Data.Entities.Location> locations { get; set; } = [];
	}
}
