namespace ASP.Models.Home.Ioc
{
	public class IocPageModel
	{
		public String TabHeader { get; set; } = null!;
		public String PageTitle { get; set; } = null!;
		public String IoCIs { get; set; } = null!;
		public List<String> IoCOptions { get; set; } = null!;
		public List<String> HashExm { get; set; } = null!;
		public String SingleHash { get; set; } = null!;
		public String Title { get; set; } = null!;
		public String Url { get; set; } = null!;
		public Dictionary<String, String> Hashes { get; set; } = new();
	}
}
