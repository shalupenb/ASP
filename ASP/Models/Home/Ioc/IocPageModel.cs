namespace ASP.Models.Home.Ioc
{
	public class IocPageModel
	{
		public String SingleHash { get; set; } = null!;
		public String Title { get; set; } = null!;
		public Dictionary<String, String> Hashes { get; set; } = new();
	}
}
