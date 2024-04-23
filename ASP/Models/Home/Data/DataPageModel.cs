namespace ASP.Models.Home.Data
{
	public class DataPageModel
	{
		public String TabHeader { get; set; } = null!;
		public String PageTitle { get; set; } = null!;
		public List<String> NuGetPackages { get; set; } = null!;
		public List<String> DataStruct { get; set; } = null!;

	}
}
