using System;
using System.Security.Cryptography;

namespace ASP.Models
{
	public class RandomStringService
	{
		private static readonly Random random = new Random();

		public static string GenerateOTP(int length)
		{
			const string chars = "0123456789";
			return GenerateRandomString(chars, length);
		}

		public static string GenerateFilename(int length)
		{
			const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
			return GenerateRandomString(chars, length);
		}

		public static string GenerateSalt(int length)
		{
			const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%^&*()_+";
			return GenerateRandomString(chars, length);
		}

		private static string GenerateRandomString(string chars, int length)
		{
			char[] buffer = new char[length];
			for (int i = 0; i < length; i++)
				buffer[i] = chars[random.Next(chars.Length)];
			return new string(buffer);
		}
	}
}
