using ASP.Data.Entities;
using ASP.Migrations;

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

        public void AddRoom(String name, String description, 
            String? photoUrl, String? slug, Guid LocationId, int stars)
        {

            lock (_dbLocker)
            {
                _context.Rooms.Add(new()
                {
                    id = Guid.NewGuid(),
                    Name = name,
                    Description = description,
                    DeletedDt = null,
                    PhotoUrl = photoUrl,
                    Slug = slug,
                    LocationId = LocationId,
                    Stars = stars
                });
                _context.SaveChanges();
            }
        }

        public void AddCategory(String name,String description, String? photoUrl, String? slug = null)
        {

            slug??= name;
          
            lock (_dbLocker)
            {
                _context.Categories.Add(new()
                {
                    id = Guid.NewGuid(),
                    Name = name,
                    Description = description,
                    DeletedDt = null,
                    PhotoUrl = photoUrl
                });
                _context.SaveChanges();
            }
        }
        public List<Category> GetCategories()
        {
            List<Category> list;
            lock (_dbLocker)
            {
                list = _context
                    .Categories
                    .Where(c =>c.DeletedDt==null)
                    .ToList();
            }
            return _context
                .Categories
                .Where(c => c.DeletedDt ==null)
                .ToList();
        }

        public Category? GetCategoryBySlug(String slug)
        {
            Category? ctg;
            lock(_dbLocker)
            {
                ctg=_context.Categories.FirstOrDefault(c => c.Slug == slug);
            }
            return ctg;
        }

        public void UpdateCategory(Category category)
        {
            var ctg = _context
                .Categories
                .Find(category.id);
            if (ctg != null)
            {
                ctg.Name = category.Name;
                ctg.Description = category.Description;
                ctg.DeletedDt = category.DeletedDt;
                _context.SaveChanges();
            }
        }
        public void DeleteCategory(Guid id)
        {
            var ctg = _context
                .Categories
                .Find(id);
            if (ctg != null)
            {
                ctg.DeletedDt = DateTime.Now;
                _context.SaveChanges();

            }
        }
        public void DeleteCategory(Category category)
        {
            DeleteCategory(category.id);
        }

		public void AddLocation(String name, String description, Guid CategoryId,
	int? Stars = null, Guid? CountryId = null,
	Guid? CityId = null, string? Address = null, String? PhotoUrl=null, String? slug = null)
		{
			

			lock (_dbLocker)
            {
                _context.Locations.Add(new()
                {
                    id = Guid.NewGuid(),
                    Name = name,
                    Description = description,
                    CategoryId = CategoryId,
                    Stars = Stars,
                    CountryId = CountryId,
                    CityId = CityId,
                    adress = Address,
                    DeletedDt = null,
                    PhotoUrl = PhotoUrl,

					Slug = slug ?? name
			});
                _context.SaveChanges();
            }
		}
		public List<Location> GetLocation(String categorySlug)
		{
            var ctg = GetCategoryBySlug(categorySlug);
            if (ctg == null)
            {
                return new List<Location>();
            }
            var query = _context
                .Locations
                .Where(loc => loc.DeletedDt == null &&
                loc.CategoryId == ctg.id);
            return query.ToList();
		}
		public Location? GetLocationBySlug(String slug)
		{
			Location? loc;
			lock (_dbLocker)
			{
				loc = _context.Locations.FirstOrDefault(c => c.Slug == slug);
			}
			return loc;
		}
	}

}
