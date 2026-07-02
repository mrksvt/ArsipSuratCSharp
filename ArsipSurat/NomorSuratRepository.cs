using System;
using System.Data;
using MySql.Data.MySqlClient;

namespace ArsipSurat
{
    public class NomorSuratRepository
    {
        /// <summary>
        /// Atomically increment counter for year+kode and return the new number.
        /// Uses INSERT ... ON UPDATE for race-condition safety.
        /// </summary>
        public int GetNextNumber(int tahun, string kodeSurat)
        {
            // Ensure row exists, then increment atomically
            string upsert = @"INSERT INTO nomor_surat_counter (tahun, kode_surat, last_number)
                             VALUES (@tahun, @kode, 1)
                             ON DUPLICATE KEY UPDATE last_number = last_number + 1";
            DatabaseHelper.ExecuteNonQuery(upsert,
                new MySqlParameter("@tahun", tahun),
                new MySqlParameter("@kode", kodeSurat));

            // Read the current value
            string select = "SELECT last_number FROM nomor_surat_counter WHERE tahun = @tahun AND kode_surat = @kode";
            object result = DatabaseHelper.ExecuteScalar(select,
                new MySqlParameter("@tahun", tahun),
                new MySqlParameter("@kode", kodeSurat));
            return Convert.ToInt32(result);
        }

        /// <summary>
        /// Preview next number without incrementing.
        /// </summary>
        public int PeekNextNumber(int tahun, string kodeSurat)
        {
            string query = "SELECT last_number FROM nomor_surat_counter WHERE tahun = @tahun AND kode_surat = @kode";
            object result = DatabaseHelper.ExecuteScalar(query,
                new MySqlParameter("@tahun", tahun),
                new MySqlParameter("@kode", kodeSurat));
            if (result == null || result == DBNull.Value) return 1;
            return Convert.ToInt32(result) + 1;
        }

        /// <summary>
        /// Convert month number (1-12) to Roman numeral.
        /// </summary>
        public static string ToRoman(int month)
        {
            string[] roman = { "I", "II", "III", "IV", "V", "VI", "VII", "VIII", "IX", "X", "XI", "XII" };
            if (month < 1 || month > 12) return month.ToString();
            return roman[month - 1];
        }

        /// <summary>
        /// Build full surat number string.
        /// Format: {number:000}/{KODE}/{DEPT}/{bulanRomawi}/{tahun}
        /// Example: 003/SK/HRD/VII/2026
        /// </summary>
        public static string FormatNomor(int number, string kodeSurat, string departemen, DateTime tanggal)
        {
            return string.Format("{0:000}/{1}/{2}/{3}/{4}",
                number, kodeSurat, departemen, ToRoman(tanggal.Month), tanggal.Year);
        }
    }
}
