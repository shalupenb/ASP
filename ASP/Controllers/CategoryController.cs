using ASP.Data.DAL;
using ASP.Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

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
            return _dataAccessor.ContentDao.GetCategories();
        }
		[HttpPost]
        public String DoPost([FromForm] CategoryPostModel model)
		{
			try
			{
				String? fileName = null;
				if (model.Photo != null)
				{

					String ext = Path.GetExtension(model.Photo.FileName);
					String path = Directory.GetCurrentDirectory() + "/wwwroot/img/content/";
					String pathName;
					do
					{
						fileName = Guid.NewGuid() + ext;
						pathName = path + fileName;

					}
					while (System.IO.File.Exists(pathName));
					using var stream = System.IO.File.OpenWrite(pathName);
					model.Photo.CopyTo(stream);
				}
				_dataAccessor.ContentDao.AddCategory(model.Name, model.Description, fileName);
                Response.StatusCode = StatusCodes.Status201Created;
                return "Ok";
            }
            catch (Exception ex)
            {
				Response.StatusCode = StatusCodes.Status400BadRequest;
				return "Error";
			}
        }
	}
    public class CategoryPostModel
    {
        public String Name { get; set; }
		public String Description { get; set; }
        public IFormFile? Photo { get; set; }
	}
}
