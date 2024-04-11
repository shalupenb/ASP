namespace ASP.Services.Hash
{
    public class ShaHashService : IHashService
    {
        public string Digest(string input) => Convert.ToHexString(
            System.Security.Cryptography.SHA1.HashData(
                System.Text.Encoding.UTF8.GetBytes(input)));
    }
}
