namespace ASP.Models.FrontendForm
{
	public class FrontendFormInput
	{
		// для вхідних даних збіг імен (у класі та JSON) вимагається
		public String UserName { get; set; } = null!;
		public String UserEmail { get; set; } = null!;
		public bool UserTerm {  get; set; } = false;
		public String UserGen { get; set; } = null!;
		public DateTime UserDate { get; set; }
	}
}
