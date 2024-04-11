namespace ASP.Models.Home.Model
{
	// Приклад моделі представлення - набору даних для сторінки Home/Model
	public class PageModel
	{
		public String TabHeader { get; set; } = null!;
		public String PageTitle { get; set; } = null!;

		// Якщо на сторінці є форми, то модель представлення має їх поля
		public FormModel? FormModel { get; set; }
	}
}
