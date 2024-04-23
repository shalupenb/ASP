namespace ASP.Models.Home.Signup
{
	public class SingupPageModel
	{
		public SingupFormModel? FormModel { get; set; }
		public Dictionary<String, String>? ValidationErrors { get; set; }
	}
}
