using ASP.Data.Entities;
using ASP.Services.Kdf;
namespace ASP.Data.DAL
{
	public class UserDao
	{
		private readonly DataContext _dataContext;
		private readonly IKdfService _kdfService;
		private readonly Object _dbLocker;
		public UserDao(DataContext dataContext, IKdfService kdfService, object dbLocker)
		{
			_dataContext = dataContext;
			_kdfService = kdfService;
			_dbLocker = dbLocker;
		}


		public User? GetUserById(String id)
		{
			User? user;
			try
			{
				lock (_dbLocker)
				{
					user = _dataContext.Users.Find(Guid.Parse(id));
				}
			}
			catch { return null; }
			return user;
		}
        public User? Authorize(String email, String password) 
		{ 
			var user = _dataContext.Users
				.FirstOrDefault(x => x.Email == email);
			if (user == null || user.Derivedkey != _kdfService.DerivedKey(user.Salt, password)) 
			{
				return null;
			}
			return user;
		}
		public void Signup( User user)
		{
			if (user.Id == default) 
			{
				user.Id = Guid.NewGuid();
			}
			_dataContext.Users.Add(user);
			_dataContext.SaveChanges();
		}
	}
}
