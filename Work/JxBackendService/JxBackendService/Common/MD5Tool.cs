using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace JxBackendService.Common
{
	public class MD5Tool
	{
		/// <summary>
		/// 第三方All Bet使用
		/// </summary>
		public static string Base64edMd5(string data)
		{
			return Convert.ToBase64String(Md5(Encoding.UTF8.GetBytes(data)));
		}

		public static byte[] Md5(byte[] data)
		{
			MD5CryptoServiceProvider md5Crp = new MD5CryptoServiceProvider();
			return md5Crp.ComputeHash(data);
		}

		
		/// <summary>
		/// 第三方OB使用
		/// </summary>
		public static string MD5EncodingForOBGameProvider(string rawPass)
		{
			MD5 mD = MD5.Create();
			byte[] bytes = Encoding.UTF8.GetBytes(rawPass);
			byte[] hashBytes = mD.ComputeHash(bytes);
			var stringBuilder = new StringBuilder();
			
			foreach (byte hashByte in hashBytes)
			{
				stringBuilder.Append(hashByte.ToString("x2"));
			}

			return stringBuilder.ToString();
		}

	}
}
