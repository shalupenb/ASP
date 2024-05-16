using ASP.Data.Entities;
using ASP.Services.Kdf;
using Microsoft.EntityFrameworkCore;

namespace ASP.Data.DAL
{
	public class UserDao
	{
		private readonly Object _dblocker;
		private readonly DataContext _dataContext;
		private readonly IKdfService _kdfService;

		public UserDao(DataContext dataContext, IKdfService kdfService, object dblocker)
		{
			_dataContext = dataContext;
			_kdfService = kdfService;
			_dblocker = dblocker;
		}

		public User? GetUserById(String id)
		{
			User? user;
			try
			{
				lock (_dblocker)
				{
					user = _dataContext.Users.Find(Guid.Parse(id));
				}
			}
			catch { return null; }
			return user;
		}
		public User? GetUserByToken(Guid token)
		{
			User? user;
			lock (_dblocker)
			{
				user = _dataContext.Tokens.Include(t => t.User).FirstOrDefault(t => t.Id == token)?.User;
			}
			return user;
		}
		public Token? CreateTokenForUser(User user)
		{
			return CreateTokenForUser(user.Id);
		}
		public Token? CreateTokenForUser(Guid userId)
		{
            Token? existingToken = _dataContext.Tokens
                .FirstOrDefault(t => t.UserId == userId && t.ExpiresDt > DateTime.Now);
            if (existingToken != null)
            {
                return existingToken; // Если активный токен уже существует, возвращаю его
            }
            Token token = new() { 
				Id = Guid.NewGuid(),
				UserId= userId,
				SubmitDt = DateTime.Now,
				ExpiresDt = DateTime.Now.AddDays(1),
			};
			_dataContext.Tokens.Add(token);
			try
			{
				lock(_dblocker)
				{
					_dataContext.SaveChanges();
				}
				return token;
			}
			catch
			{
				_dataContext.Tokens.Remove(token);
				return null;
			}
		}

		public User? Authorize(String email, String password)
		{
			var user = _dataContext
				.Users
				.FirstOrDefault(x => x.Email == email);

			if (user == null ||
				user.Derivedkey != _kdfService.DerivedKey(user.Salt, password))
			{
				return null;
			}
			return user;
		}

		public void Signup(User user)
		{
			if (user.Id == default)
			{
				user.Id = Guid.NewGuid();
			}
			_dataContext.Users.Add(user);
			_dataContext.SaveChanges();
		}

		public Boolean ConfirmEmail(String email, String code)
		{
			//Find User by email
			User? user;
			lock (_dblocker)
			{
				user = _dataContext.Users.FirstOrDefault(u => u.Email == email);				
			}
			if (user == null || user.EmailConfirmCode != code)
			{
				return false;
			}
			user.EmailConfirmCode = null;
			lock (_dblocker)
			{
				_dataContext.SaveChanges();
			}
			return true;
		}
	}
}

/* DAL - Data Access Layer - сукупність ycіх DAO
 * DAO - Data Access Object - набір методів для роботи з сутністю
 */
