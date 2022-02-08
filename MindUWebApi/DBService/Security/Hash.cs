using System;
using System.Security.Cryptography;

namespace DBService.Security
{
    public static class Hash
    {
        public const int Length = 20;
        public static string HashPasword(string password, string saltString)
        {
            try
            {
                var salt = Convert.FromBase64String(saltString);

                var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100000);
                byte[] hash = pbkdf2.GetBytes(20);

                return Convert.ToBase64String(hash);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }

        public static string GetNewSalt()
        {
            var salt = new byte[20];
            using (var random = new RNGCryptoServiceProvider())
            {
                random.GetNonZeroBytes(salt);
            }

            return Convert.ToBase64String(salt);
        }

    }
}

