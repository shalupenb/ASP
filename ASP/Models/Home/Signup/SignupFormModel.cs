using Microsoft.AspNetCore.Mvc;

namespace ASP.Models.Home.Signup
{
	public class SignupFormModel
	{
		[FromForm(Name = "user-name")]
		public String UserName { get; set; } = null!;


		[FromForm(Name = "user-email")]
		public String UserEmail { get; set; } = null!;

		[FromForm(Name = "user-password")]
		public String Password { get; set; } = null!;


		[FromForm(Name = "user-repeat")]
		public String Repeat { get; set; } = null!;

		[FromForm(Name = "user-birthdate")]
		public DateTime UserBirthdate { get; set; }


		[FromForm(Name = "user-avatar")]
		public IFormFile UserAvatar { get; set; } = null!;

		[FromForm(Name = "user-agreement")]
		public bool agreement { get; set; } = false!;

		[FromForm(Name = "signup-button")]
		public bool HasData { get; set; } = false!;

		public String? SavedAvataFilename { get; set; }
	}
}
