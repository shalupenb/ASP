using ASP.Data.Entities;

namespace ASP.Data.DAL
{
    public class ContentDao
    {
        private readonly DataContext _context;
		private readonly Object _dbLocker;
		public ContentDao(DataContext context, object dbLocker)
		{
			_context = context;
			_dbLocker = dbLocker;
		}
		public void AddCategory(String name, String description, String? photoUrl)
        {
            _context.Categories.Add(new()
            {
                Id = Guid.NewGuid(),
                Name = name,
                Description = description,
                DeletedDt = null,
                PhotoUrl = photoUrl
            });
            _context.SaveChanges();
        }
        public List<Category> GetCategories()
        {
            List<Category> list;
            lock (_dbLocker)
            {
				list = _context.Categories.Where(c => c.DeletedDt == null).ToList();
			}
            return list;
        }
        public void UpdateCategory(Category category)
        {
            var ctg = _context.Categories.Find(category.Id);
            if(ctg != null)
            {
                ctg.Name = category.Name;
                ctg.Description = category.Description;
                ctg.DeletedDt = category.DeletedDt;
                _context.SaveChanges();
            }
        }
        public void DeleteCategory(Guid id)
        {
            var ctg = _context.Categories.Find(id);
            if (ctg != null)
            {
                ctg.DeletedDt = DateTime.Now;
                _context.SaveChanges();
            }
        }
        public void DeleteCategory(Category category)
        { 
            DeleteCategory(category.Id);
        }




        public void AddLocation(String name, String description, Guid CategoryId,
	        int? Stars = null, Guid? CountryId = null,
	        Guid? CityId = null, string? Address = null, String? PhotoUrl = null)
		{
            lock(_dbLocker)
            {
				_context.Locations.Add(new()
				{
					Id = Guid.NewGuid(),
					Name = name,
					Description = description,
					CategoryId = CategoryId,
					Stars = Stars,
					CountryId = CountryId,
					CityId = CityId,
					Address = Address,
					DeletedDt = null,
					PhotoUrl = PhotoUrl
				});
				_context.SaveChanges();
			}
		}
		public List<Location> GetLocations(Guid? categoryId = null)
		{
            var query = _context.Locations.Where(loc => loc.DeletedDt == null);
            if(categoryId != null)
            {
                query = query.Where(loc => loc.CategoryId == categoryId);
            }
			return query.ToList();
		}
	}
   
}
