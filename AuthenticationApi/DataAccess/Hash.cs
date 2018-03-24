using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace DataAccess
{
    public static class Hash
    {

        public static string ToSHA256Hash(this string source)
        {
            var bytes = Encoding.UTF8.GetBytes(source);
            using (var hash = SHA256Managed.Create())
            {
                var hashBytes = hash.ComputeHash(bytes);
                var sb = new StringBuilder();
                foreach (var @byte in hashBytes) sb.Append(@byte.ToString("x2"));
                return sb.ToString();
            }

        }
    }
}
