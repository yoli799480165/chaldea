using System;
using System.Security.Cryptography;
using System.Text;

namespace Chaldea.IdentityServer.Utilities
{
    public static class Md5Helper
    {
        public static string Md5(string str)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(str));
                var hash = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
                return hash;
            }
        }
    }
}