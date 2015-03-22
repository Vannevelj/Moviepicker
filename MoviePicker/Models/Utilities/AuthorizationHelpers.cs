using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Models.Utilities
{
    public static class AuthorizationHelpers
    {
        public static string GetHash(string input)
        {
            var algorithm = new SHA256CryptoServiceProvider();
            var byteValue = Encoding.UTF8.GetBytes(input);
            var byteHash = algorithm.ComputeHash(byteValue);
            return Convert.ToBase64String(byteHash);
        }
    }
}
