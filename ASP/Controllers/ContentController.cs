using ASP.Data.DAL;
using ASP.Models.Content.Category;
using ASP.Models.Content.Index;
using ASP.Models.Content.location;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;

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
			public IActionResult Category([FromRoute]String id)
			{
				var ctg = _dataAccessor.ContentDao.GetCategoryBySlug(id);
			return ctg == null
			? View("NotFound")
			: View(new ContentCategoryPageModel
			{
				Category = ctg,
				locations = _dataAccessor.ContentDao.GetLocation(ctg.Slug!)
			}); ;
			}


		public IActionResult Location([FromRoute] String id)
		{
			var ctg = _dataAccessor.ContentDao.GetLocationBySlug(id);
			return ctg == null
			? View("NotFound")
			: View(new ContentLocationPageModel()
			{
				Location = ctg,
			});

		}
	}
	}

