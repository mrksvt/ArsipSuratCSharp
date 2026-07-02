using System;
using System.Security.Cryptography;
using System.Text;

namespace ArsipSurat
{
    public static class TotpHelper
    {
        private const int SecretLength = 20;
        private const int Digits = 6;
        private const int Period = 30;

        public static string GenerateSecret()
        {
            byte[] bytes = new byte[SecretLength];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(bytes);
            }
            return ToBase32(bytes);
        }

        public static string GetOtpAuthUri(string username, string secret, string issuer = "ArsipSurat")
        {
            return string.Format("otpauth://totp/{0}:{1}?secret={2}&issuer={0}&digits={3}&period={4}",
                Uri.EscapeDataString(issuer),
                Uri.EscapeDataString(username),
                secret,
                Digits,
                Period);
        }

        public static bool VerifyCode(string secret, string code)
        {
            if (string.IsNullOrEmpty(code) || code.Length != Digits) return false;

            long timeStep = GetCurrentTimeStep();
            for (int i = -1; i <= 1; i++)
            {
                string expected = ComputeCode(secret, timeStep + i);
                if (expected == code) return true;
            }
            return false;
        }

        public static string ComputeCode(string secret, long timeStep)
        {
            byte[] key = FromBase32(secret);
            byte[] time = BitConverter.GetBytes(timeStep);
            if (BitConverter.IsLittleEndian) Array.Reverse(time);

            byte[] hash;
            using (var hmac = new HMACSHA1(key))
            {
                hash = hmac.ComputeHash(time);
            }

            int offset = hash[hash.Length - 1] & 0x0F;
            int binary = ((hash[offset] & 0x7F) << 24)
                       | ((hash[offset + 1] & 0xFF) << 16)
                       | ((hash[offset + 2] & 0xFF) << 8)
                       | (hash[offset + 3] & 0xFF);

            int otp = binary % (int)Math.Pow(10, Digits);
            return otp.ToString().PadLeft(Digits, '0');
        }

        private static long GetCurrentTimeStep()
        {
            long unixTime = (long)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
            return unixTime / Period;
        }

        private static readonly string Base32Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ234567";

        public static string ToBase32(byte[] bytes)
        {
            var sb = new StringBuilder();
            int bits = 0, value = 0;
            foreach (byte b in bytes)
            {
                value = (value << 8) | b;
                bits += 8;
                while (bits >= 5)
                {
                    sb.Append(Base32Chars[(value >> (bits - 5)) & 0x1F]);
                    bits -= 5;
                }
            }
            if (bits > 0)
                sb.Append(Base32Chars[(value << (5 - bits)) & 0x1F]);
            return sb.ToString();
        }

        public static byte[] FromBase32(string input)
        {
            input = input.ToUpper().TrimEnd('=');
            int byteCount = input.Length * 5 / 8;
            byte[] result = new byte[byteCount];
            int bits = 0, value = 0, index = 0;
            foreach (char c in input)
            {
                int idx = Base32Chars.IndexOf(c);
                if (idx < 0) continue;
                value = (value << 5) | idx;
                bits += 5;
                if (bits >= 8)
                {
                    result[index++] = (byte)((value >> (bits - 8)) & 0xFF);
                    bits -= 8;
                }
            }
            return result;
        }
    }
}
