namespace ASP.Services.Random
{
	public interface IRandomService
	{
		string GenerateOTP(int length);
		string GenerateFilename(int length);
		string GenerateSalt(int length);
	}
}
