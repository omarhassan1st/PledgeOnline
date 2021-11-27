using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Web_FirstApplication.Const
{
    public static class Encryption
    {
		public static class Sro
		{
			public static string ComputeMD5Hash(string password)
			{
				byte[] bytes = new UTF8Encoding().GetBytes(password);
				return BitConverter.ToString(((HashAlgorithm)CryptoConfig.CreateFromName("MD5"))
					.ComputeHash(bytes)).Replace("-", string.Empty).ToLower();
			}
		}
	}
}
