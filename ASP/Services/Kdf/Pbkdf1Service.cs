using ASP.Services.Hash;

namespace ASP.Services.Kdf
{
	public class Pbkdf1Service : IKdfService
	{
		private readonly IHashService _hashService;

		public Pbkdf1Service(IHashService hashService)
		{
			_hashService = hashService;
		}
		public string DerivedKey(string salt, string password)
		{
			string t1 = _hashService.Digest(password + salt);
			string t2 = _hashService.Digest(t1);
			string t3 = _hashService.Digest(t2);
			return t3;
		}
	}
}
