namespace ASP.Services.Kdf
{
	public interface IKdfService
	{
		String DerivedKey(String salt, String password);
	}
}
