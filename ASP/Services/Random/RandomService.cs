using System;
using System.Security.Cryptography;

namespace ASP.Services.Random
{
	public class RandomService : IRandomService
	{
		private static readonly System.Random _random = new System.Random();

		public string GenerateOTP(int length)
		{
			const string chars = "0123456789";
			return GenerateRandomString(chars, length);
		}

		public string GenerateFilename(int length)
		{
			const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
			return GenerateRandomString(chars, length);
		}

		public string GenerateSalt(int length)
		{
			const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%^&*()_+";
			return GenerateRandomString(chars, length);
		}

		private string GenerateRandomString(string chars, int length)
		{
			char[] buffer = new char[length];
			for (int i = 0; i < length; i++)
				buffer[i] = chars[_random.Next(chars.Length)];
			return new string(buffer);
		}
	}
}
