namespace ASP.Models.Home.Signup
{
	public class SignupPageModel
	{
		public SignupFormModel? FormModel { get; set; }
		public Dictionary<string, string> ValidationsErrors { get; set; }
	}
}
