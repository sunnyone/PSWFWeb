using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace PSWFWeb
{
    public class PasswordUtil
    {
        public static string GeneratePasswordHash(string password)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(password);
            SHA256 crypto = new SHA256CryptoServiceProvider();
            byte[] hashed = crypto.ComputeHash(bytes);

            StringBuilder sb = new StringBuilder();
            foreach (var b in hashed)
            {
                sb.AppendFormat("{0:X2}", b);
            }

            return sb.ToString();
        }
    }
}
