using System;
using System.Data;
using System.Security.Cryptography;
using System.Text;

/// <summary>
/// Utility untuk reset password dan email admin (emergency recovery).
/// Compile: csc.exe /out:ResetAdmin.exe ResetAdmin.cs
/// Run: ResetAdmin.exe
/// </summary>
class ResetAdmin
{
    static void Main(string[] args)
    {
        Console.WriteLine("==============================================");
        Console.WriteLine("  Arsip Surat - Reset Password & Email Admin");
        Console.WriteLine("==============================================");
        Console.WriteLine();

        // Generate password hash untuk "admin123"
        string newPasswordHash = HashPassword("admin123");

        Console.WriteLine("Password hash baru untuk 'admin123':");
        Console.WriteLine(newPasswordHash);
        Console.WriteLine();

        // Generate SQL script
        string sqlScript = @"
-- Reset Admin Password & Email
-- Generated: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + @"

USE arsip_surat;

UPDATE users 
SET 
    password_hash = '" + newPasswordHash.Replace("'", "''") + @"',
    email = 'admin@arsipsurat.local',
    email_set = 0,
    two_factor_method = NULL,
    two_factor_secret = NULL
WHERE username = 'admin';

SELECT 
    username, 
    email, 
    email_set, 
    two_factor_method,
    CASE WHEN two_factor_secret IS NULL THEN 'NULL' ELSE '(tersimpan)' END AS two_factor_secret
FROM users 
WHERE username = 'admin';
";

        // Simpan ke file SQL
        string sqlFilePath = "reset_admin.sql";
        System.IO.File.WriteAllText(sqlFilePath, sqlScript);
        Console.WriteLine("File SQL script disimpan ke: " + sqlFilePath);
        Console.WriteLine();

        // Instruksi eksekusi
        Console.WriteLine("LANGKAH SELANJUTNYA:");
        Console.WriteLine("1. Pastikan MySQL Server sedang berjalan");
        Console.WriteLine("2. Jalankan command berikut di Command Prompt:");
        Console.WriteLine();
        Console.WriteLine("   mysql -u root -p < reset_admin.sql");
        Console.WriteLine();
        Console.WriteLine("3. Masukkan password MySQL root saat diminta");
        Console.WriteLine("4. Setelah selesai, login dengan:");
        Console.WriteLine("   Username: admin");
        Console.WriteLine("   Password: admin123");
        Console.WriteLine();
        Console.WriteLine("CATATAN:");
        Console.WriteLine("- Email direset ke: admin@arsipsurat.local");
        Console.WriteLine("- 2FA dinonaktifkan");
        Console.WriteLine("- Segera ganti password setelah login");
        Console.WriteLine();
        Console.WriteLine("Tekan tombol apa saja untuk keluar...");
        Console.ReadKey();
    }

    /// <summary>
    /// Hash password menggunakan PBKDF2-SHA256 (sama dengan PasswordHelper.cs).
    /// Format: {iterations}.{base64-salt}.{base64-hash}
    /// </summary>
    static string HashPassword(string password)
    {
        int iterations = 100000;
        int saltSize = 16; // 128 bits
        int hashSize = 32; // 256 bits

        // Generate random salt
        byte[] salt = new byte[saltSize];
        using (var rng = new RNGCryptoServiceProvider())
        {
            rng.GetBytes(salt);
        }

        // Hash password
        using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations))
        {
            byte[] hash = pbkdf2.GetBytes(hashSize);

            // Format: {iterations}.{base64-salt}.{base64-hash}
            return string.Format("{0}.{1}.{2}",
                iterations,
                Convert.ToBase64String(salt),
                Convert.ToBase64String(hash));
        }
    }
}
