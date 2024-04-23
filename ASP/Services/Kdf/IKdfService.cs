namespace ASP.Services.Kdf
{
	public interface IKdfService
	{
		string DerivedKey(String salt, String password);
	}
}

/* Key Derivation Service by RFC 2898
 * https://datatracker.ietf.org/doc/html/rfc2898#section-5.1
 */