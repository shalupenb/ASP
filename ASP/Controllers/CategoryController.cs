using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ASP.Data.Entities;
using ASP.Data.DAL;
using ASP.Models;
using System.Security.Claims;
using Microsoft.Extensions.Primitives;
using ASP.Middleware;

namespace ASP.Controllers
{
	[Route("api/category")]
	[ApiController]
	public class CategoryController : ControllerBase
	{
		private readonly DataAccessor _dataAccessor;
		public CategoryController(DataAccessor dataAccessor)
		{
			_dataAccessor = dataAccessor;
		}

		[HttpGet]
		public List<Category> DoGet()
		{
			String? userRole = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
			bool isAdmin = "Admin".Equals(userRole);
			return _dataAccessor.ContentDao.GetCategories(includeDeleted: isAdmin);
		}

		[HttpPost]
		public String DoPost([FromForm] CategoryPostModel model)
		{
			var identity = User.Identities.FirstOrDefault(i => i.AuthenticationType == nameof(AuthTokenMiddleware));
			if (identity == null)
			{
				Response.StatusCode = StatusCodes.Status401Unauthorized;
				return HttpContext.Items[nameof(AuthTokenMiddleware)]?.ToString() ?? "";
			}
			if(identity.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value != "Admin")
			{
				Response.StatusCode = StatusCodes.Status403Forbidden;
				return "Access to API forbidden";
			}
			try
			{
				String? fileName = null;
				if (model.Photo != null)
				{
					string ext = Path.GetExtension(model.Photo.FileName);
					String path = Directory.GetCurrentDirectory() + "/wwwroot/img/content/";
					String pathName;
					do
					{
						fileName = Guid.NewGuid() + ext;
						pathName = path + fileName;
					}
					while (System.IO.File.Exists(pathName));
					using var steam = System.IO.File.OpenWrite(pathName);
					model.Photo.CopyTo(steam);
				}
				_dataAccessor.ContentDao.AddCategory(model.Name, model.Description, fileName, model.Slug);
				Response.StatusCode = StatusCodes.Status201Created;
				return "OK";
			}
			catch (Exception ex)
			{
				Response.StatusCode = StatusCodes.Status400BadRequest;
				return "ERROR";
			}

		}

		[HttpDelete("{id}")]
		public String DoDelete(Guid id)
		{
			_dataAccessor.ContentDao.DeleteCategory(id);
			Response.StatusCode = StatusCodes.Status202Accepted;
			return "Ok";
		}
		
		//Метод НЕ позначений атрибутом, буде викликано якщо не знайдеться необзідній з позначених. Це дозволяє прийняти нестандартні запити
		public Object DoOther()
		{ 
			if(Request.Method == "RESTORE")
			{
				return DoRestore();
			}
			Response.StatusCode = StatusCodes.Status405MethodNotAllowed;
			return "Method Not Allowed";
		}
		//Другий не позначений метод має бути private щоб не було конфлікту
		private String DoRestore()
		{
			String? id = Request.Query["id"].FirstOrDefault();
			try
			{
				_dataAccessor.ContentDao.RestoreCategory(Guid.Parse(id!));
			}
			catch
			{
				Response.StatusCode = StatusCodes.Status400BadRequest;
				return "Empty or invalid id";
			}
			Response.StatusCode = StatusCodes.Status202Accepted;
			return "RESTORE Ok for id = " + id;
		}
	}
	public class CategoryPostModel
	{
		[FromForm(Name = "category-name")]
		public String Name { get; set; }
		[FromForm(Name = "category-description")]
		public String Description { get; set; }
		[FromForm(Name = "category-slug")]
		public String Slug { get; set; }
		[FromForm(Name = "category-photo")]
		public IFormFile? Photo { get; set; } 
	}
}
