using System;
using System.Data;
using MySql.Data.MySqlClient;

namespace ArsipSurat
{
    public class LampiranRepository
    {
        public DataTable GetBySuratId(int suratId)
        {
            string query = "SELECT id, nama_file, file_path, file_size, file_type, upload_date FROM lampiran WHERE surat_id = @suratId";
            return DatabaseHelper.ExecuteQuery(query, new MySqlParameter("@suratId", suratId));
        }

        public void Insert(int suratId, string namaFile, string filePath, long fileSize, string fileType)
        {
            string query = @"INSERT INTO lampiran (surat_id, nama_file, file_path, file_size, file_type) 
                            VALUES (@suratId, @namaFile, @filePath, @fileSize, @fileType)";
            DatabaseHelper.ExecuteNonQuery(query,
                new MySqlParameter("@suratId", suratId),
                new MySqlParameter("@namaFile", namaFile),
                new MySqlParameter("@filePath", filePath),
                new MySqlParameter("@fileSize", fileSize),
                new MySqlParameter("@fileType", fileType)
            );
        }

        public void Delete(int id)
        {
            string query = "SELECT file_path FROM lampiran WHERE id = @id";
            DataTable dt = DatabaseHelper.ExecuteQuery(query, new MySqlParameter("@id", id));
            if (dt.Rows.Count > 0)
            {
                string relativePath = dt.Rows[0]["file_path"].ToString();
                FileStorage.DeleteFile(relativePath);
            }
            query = "DELETE FROM lampiran WHERE id = @id";
            DatabaseHelper.ExecuteNonQuery(query, new MySqlParameter("@id", id));
        }

        public void DeleteBySuratId(int suratId)
        {
            DataTable dt = GetBySuratId(suratId);
            foreach (DataRow row in dt.Rows)
            {
                string relativePath = row["file_path"].ToString();
                FileStorage.DeleteFile(relativePath);
            }
            string query = "DELETE FROM lampiran WHERE surat_id = @suratId";
            DatabaseHelper.ExecuteNonQuery(query, new MySqlParameter("@suratId", suratId));
        }
    }
}
