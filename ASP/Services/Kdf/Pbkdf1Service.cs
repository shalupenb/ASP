using ASP.Services.Hash;

namespace ASP.Services.Kdf
{
	public class Pbkdf1Service :IKdfService
	{
		private readonly IHashService _hashService;
		public Pbkdf1Service(IHashService hashService)
		{
			_hashService = hashService;
		}
		public string DerivedKey(string salt, string password)
		{
			String t1 = _hashService.Digest(password + salt);
			String t2 = _hashService.Digest(t1);
			String t3 = _hashService.Digest(t2);
			return t3;
		}
	}
}
/* згідно з п. 5.1. PBKDF1
	https://datatracker.ietf.org/doc/html/rfc2898#section-5.1
 */