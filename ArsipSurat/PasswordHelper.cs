using System;
using System.Security.Cryptography;

namespace ArsipSurat
{
    /// <summary>
    /// Password hashing using PBKDF2-SHA256 (Rfc2898DeriveBytes).
    /// Format: {iterations}.{base64-salt}.{base64-hash}
    /// </summary>
    public static class PasswordHelper
    {
        private const int SaltSize = 16;        // 128 bits
        private const int HashSize = 32;        // 256 bits
        private const int Iterations = 100000;  // OWASP recommended minimum

        public static string HashPassword(string password)
        {
            byte[] salt = new byte[SaltSize];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(salt);
            }

            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations))
            {
                byte[] hash = pbkdf2.GetBytes(HashSize);
                return string.Format("{0}.{1}.{2}",
                    Iterations,
                    Convert.ToBase64String(salt),
                    Convert.ToBase64String(hash));
            }
        }

        public static bool VerifyPassword(string password, string storedHash)
        {
            try
            {
                string[] parts = storedHash.Split('.');
                if (parts.Length != 3) return false;

                int iterations = int.Parse(parts[0]);
                byte[] salt = Convert.FromBase64String(parts[1]);
                byte[] expectedHash = Convert.FromBase64String(parts[2]);

                using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations))
                {
                    byte[] actualHash = pbkdf2.GetBytes(expectedHash.Length);
                    return SlowEquals(expectedHash, actualHash);
                }
            }
            catch
            {
                return false;
            }
        }

        // Constant-time comparison to prevent timing attacks
        private static bool SlowEquals(byte[] a, byte[] b)
        {
            uint diff = (uint)a.Length ^ (uint)b.Length;
            for (int i = 0; i < a.Length && i < b.Length; i++)
                diff |= (uint)(a[i] ^ b[i]);
            return diff == 0;
        }
    }
}
