using System;
using System.Data;
using MySql.Data.MySqlClient;

namespace ArsipSurat
{
    public class MasterDepartemenRepository
    {
        public DataTable GetAll(bool activeOnly = false)
        {
            string query = @"SELECT id, nama AS `Nama`, keterangan AS `Keterangan`,
                            is_active AS `Aktif`, created_at AS `Dibuat`
                            FROM master_departemen";
            if (activeOnly)
                query += " WHERE is_active = 1";
            query += " ORDER BY nama";
            return DatabaseHelper.ExecuteQuery(query);
        }

        public DataTable Search(string search, bool activeOnly = false)
        {
            string query = @"SELECT id, nama AS `Nama`, keterangan AS `Keterangan`,
                            is_active AS `Aktif`, created_at AS `Dibuat`
                            FROM master_departemen WHERE nama LIKE @search";
            if (activeOnly)
                query += " AND is_active = 1";
            query += " ORDER BY nama";
            return DatabaseHelper.ExecuteQuery(query, new MySqlParameter("@search", "%" + search + "%"));
        }

        public DataTable GetActiveList()
        {
            string query = "SELECT id, nama FROM master_departemen WHERE is_active = 1 ORDER BY nama";
            return DatabaseHelper.ExecuteQuery(query);
        }

        public void Insert(string nama, string keterangan)
        {
            string query = "INSERT INTO master_departemen (nama, keterangan) VALUES (@nama, @keterangan)";
            DatabaseHelper.ExecuteNonQuery(query,
                new MySqlParameter("@nama", nama),
                new MySqlParameter("@keterangan", (object)keterangan ?? DBNull.Value));
        }

        public void Update(int id, string nama, string keterangan, bool isActive)
        {
            string query = "UPDATE master_departemen SET nama=@nama, keterangan=@keterangan, is_active=@active WHERE id=@id";
            DatabaseHelper.ExecuteNonQuery(query,
                new MySqlParameter("@id", id),
                new MySqlParameter("@nama", nama),
                new MySqlParameter("@keterangan", (object)keterangan ?? DBNull.Value),
                new MySqlParameter("@active", isActive ? 1 : 0));
        }

        public void SetActive(int id, bool isActive)
        {
            string query = "UPDATE master_departemen SET is_active=@active WHERE id=@id";
            DatabaseHelper.ExecuteNonQuery(query,
                new MySqlParameter("@id", id),
                new MySqlParameter("@active", isActive ? 1 : 0));
        }

        public bool IsNamaExists(string nama, int excludeId = 0)
        {
            string query = "SELECT COUNT(*) FROM master_departemen WHERE LOWER(nama) = LOWER(@nama)";
            if (excludeId > 0)
                query += " AND id != @id";

            var parameters = new System.Collections.Generic.List<MySqlParameter>();
            parameters.Add(new MySqlParameter("@nama", nama));
            if (excludeId > 0)
                parameters.Add(new MySqlParameter("@id", excludeId));

            object result = DatabaseHelper.ExecuteScalar(query, parameters.ToArray());
            return Convert.ToInt32(result) > 0;
        }
    }
}
