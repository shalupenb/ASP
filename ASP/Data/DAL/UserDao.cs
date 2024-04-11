using ASP.Data.Entities;
using ASP.Services.Kdf;
namespace ASP.Data.DAL
{
	public class UserDao
	{
		private readonly DataContext _dataContext;
		private readonly IKdfService _kdfService;
        public UserDao(DataContext dataContext, IKdfService kdfService)
        {
            _dataContext = dataContext;
            _kdfService = kdfService;
        }
		public User? GetUserById(String id)
		{
			try
			{
				return _dataContext.Users.Find(Guid.Parse(id));
			}
			catch { return null; }
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
