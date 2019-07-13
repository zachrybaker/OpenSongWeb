using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Text;

namespace OpenSongWeb.Data.Helpers
{
    public class RandomGenerator
    {
        private const string AllowableCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";

        public static string GenerateString(int length)
        {
            var data = new byte[length];

            using (RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider())
            {
                crypto.GetBytes(data);
            }
            StringBuilder result = new StringBuilder(length);
            foreach (byte b in data)
            {
                result.Append(AllowableCharacters[b % (AllowableCharacters.Length)]);
            }
            return result.ToString();
        }

        public static string GenerateHMACSHA256SecretKey()
        {
            using (HMACSHA256 hmac = new HMACSHA256())
            {
                return Convert.ToBase64String(hmac.Key);
            }
        }
    }
}
