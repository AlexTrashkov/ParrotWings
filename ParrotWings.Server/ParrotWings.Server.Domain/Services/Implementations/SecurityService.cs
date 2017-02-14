using System;
using System.Security.Cryptography;
using System.Text;

namespace ParrotWings.Server.Domain.Services.Implementations
{
	internal sealed class SecurityService : ISecurityService
	{
		public string CreateSalt()
		{
			var rng = new RNGCryptoServiceProvider();
			byte[] buffer = new byte[16];

			rng.GetBytes(buffer);
			var salt = BitConverter.ToString(buffer);

			return salt;
		}

		public string CalculateHash(string password, string salt)
		{
			using (var sha1 = new SHA1Managed())
			{
				var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(password + salt));
				var sb = new StringBuilder(hash.Length*2);

				foreach (byte b in hash)
				{
					// can be "x2" if you want lowercase
					sb.Append(b.ToString("X2"));
				}

				return sb.ToString();
			}
		}
	}
}