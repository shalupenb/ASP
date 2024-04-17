using ASP.Data.DAL;
using ASP.Models.Content.Index;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace ASP.Controllers
{
	public class ContentController(DataAccessor dataAccessor) : Controller
	{
		private readonly DataAccessor _dataAccessor = dataAccessor;
        public IActionResult Index()
        {
            ContentIndexPageModel model = new()
            {
                Categories = _dataAccessor.ContentDao.GetCategories()
            };

            return View(model);
        }
        public IActionResult Category([FromRoute] Guid id)
        {
            return View();
        }
    }
}
