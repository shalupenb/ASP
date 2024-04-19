using ASP.Data.DAL;
using ASP.Models.Content.location;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ASP.Controllers
{
    [Route("api/room")]
    [ApiController]
    public class RoomController(DataAccessor dataAccessor) : ControllerBase
    {
        private readonly DataAccessor _dataAccessor = dataAccessor;

        [HttpPost]
        public String DoPost(RoomFormModel model)
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
            if(fileName==null)
                {
                    Response.StatusCode = StatusCodes.Status400BadRequest;
                    return "File image requred";
                }
            
                _dataAccessor.ContentDao.AddRoom(
                    name: model.Name,
                    description: model.Description,
                    photoUrl: "",
                    slug: model.Slug,
                    LocationId: model.LocationId,
                    stars: model.Stars
                    );
                Response.StatusCode = StatusCodes.Status201Created;
                return "Added";
            }
            catch (Exception ex)
            {
                Response.StatusCode = StatusCodes.Status500InternalServerError;
                return ex.Message;
            }

        }
    }
}
