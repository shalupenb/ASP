﻿using ASP.Data.DAL;
using ASP.Data.Entities;
using ASP.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ASP.Controllers
{
	[Route("api/location")]
	[ApiController]
	public class LocationController : ControllerBase
	{
		private readonly DataAccessor _dataAccessor;
		public LocationController(DataAccessor dataAccessor)
		{
			_dataAccessor = dataAccessor;
		}
		[HttpGet]
		public List<Location> DoGet(Guid? categoryId)
		{
			return _dataAccessor.ContentDao.GetLocations(categoryId);
		}
		[HttpPost]
		public String DoPost([FromForm] LocationPostModel model)
		{
			try
			{
				String? fileName = null;
				if (model.Photo != null)
				{
					String ext = Path.GetExtension(model.Photo.FileName);
					String path = Directory.GetCurrentDirectory() + "/wwwroot/img/content/";
					String pathName;
					string randomFileName = RandomStringService.GenerateFilename(10);
					do
					{
						fileName = randomFileName + ext;
						pathName = path + fileName;

					}
					while (System.IO.File.Exists(pathName));
					using var stream = System.IO.File.OpenWrite(pathName);
					model.Photo.CopyTo(stream);
				}
				_dataAccessor.ContentDao.AddLocation(
					name: model.Name,
					description: model.Description,
					CategoryId: model.CategoryId,
					Stars: model.Stars,
					PhotoUrl: fileName);
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
	public class LocationPostModel
	{
		public String Name { get; set; }
		public String Description { get; set; }
		public Guid CategoryId { get; set; }
		public int Stars { get; set; }
		public IFormFile Photo { get; set; }
	}
}