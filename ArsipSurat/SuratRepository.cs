using System;
using System.Data;
using MySql.Data.MySqlClient;

namespace ArsipSurat
{
    public class SuratRepository
    {
        public DataTable GetAll(string search = "", bool showDeleted = false, DateTime? dateFrom = null, DateTime? dateTo = null, string jenis = null, string status = null)
        {
            string query = @"SELECT id, nomor_surat AS `Nomor Surat`, tanggal_surat AS `Tanggal`, jenis_surat AS `Jenis`, 
                            CASE WHEN jenis_surat = 'Masuk' THEN pengirim ELSE penerima END AS `Pengirim/Penerima`,
                            perihal AS `Perihal`, status AS `Status`, created_date AS `Created`
                            FROM surat WHERE is_deleted = @deleted";
            if (!string.IsNullOrEmpty(search))
            {
                query += " AND (nomor_surat LIKE @search OR perihal LIKE @search OR pengirim LIKE @search OR penerima LIKE @search)";
            }
            if (dateFrom.HasValue)
            {
                query += " AND tanggal_surat >= @dateFrom";
            }
            if (dateTo.HasValue)
            {
                query += " AND tanggal_surat <= @dateTo";
            }
            if (!string.IsNullOrEmpty(jenis))
            {
                query += " AND jenis_surat = @jenis";
            }
            if (!string.IsNullOrEmpty(status))
            {
                query += " AND status = @status";
            }
            query += " ORDER BY tanggal_surat DESC";

            var deletedVal = showDeleted ? 1 : 0;
            var parameters = new System.Collections.Generic.List<MySqlParameter>();
            parameters.Add(new MySqlParameter("@deleted", deletedVal));
            if (!string.IsNullOrEmpty(search))
            {
                parameters.Add(new MySqlParameter("@search", "%" + search + "%"));
            }
            if (dateFrom.HasValue)
            {
                parameters.Add(new MySqlParameter("@dateFrom", dateFrom.Value));
            }
            if (dateTo.HasValue)
            {
                parameters.Add(new MySqlParameter("@dateTo", dateTo.Value));
            }
            if (!string.IsNullOrEmpty(jenis))
            {
                parameters.Add(new MySqlParameter("@jenis", jenis));
            }
            if (!string.IsNullOrEmpty(status))
            {
                parameters.Add(new MySqlParameter("@status", status));
            }
            return DatabaseHelper.ExecuteQuery(query, parameters.ToArray());
        }

        public Surat GetById(int id, bool includeDeleted = false)
        {
            string query = "SELECT * FROM surat WHERE id = @id";
            if (!includeDeleted)
            {
                query += " AND is_deleted = 0";
            }
            DataTable dt = DatabaseHelper.ExecuteQuery(query, new MySqlParameter("@id", id));
            if (dt.Rows.Count == 0) return null;
            DataRow row = dt.Rows[0];
            return new Surat
            {
                Id = (int)row["id"],
                NomorSurat = row["nomor_surat"].ToString(),
                TanggalSurat = (DateTime)row["tanggal_surat"],
                JenisSurat = row["jenis_surat"].ToString(),
                Pengirim = row["pengirim"] == DBNull.Value ? null : row["pengirim"].ToString(),
                Penerima = row["penerima"] == DBNull.Value ? null : row["penerima"].ToString(),
                Perihal = row["perihal"].ToString(),
                Status = row["status"].ToString(),
                Keterangan = row["keterangan"] == DBNull.Value ? null : row["keterangan"].ToString(),
                IsOcrProcessed = (bool)row["is_ocr_processed"],
                OcrConfidence = row["ocr_confidence"] == DBNull.Value ? (decimal?)null : (decimal)row["ocr_confidence"],
                OcrRawText = row["ocr_raw_text"] == DBNull.Value ? null : row["ocr_raw_text"].ToString(),
                CreatedDate = (DateTime)row["created_date"],
                ModifiedDate = row["modified_date"] == DBNull.Value ? (DateTime?)null : (DateTime)row["modified_date"],
                IsDeleted = (bool)row["is_deleted"]
            };
        }

        public int Insert(Surat surat)
        {
            string query = @"INSERT INTO surat (nomor_surat, tanggal_surat, jenis_surat, pengirim, penerima, perihal, status, keterangan) 
                            VALUES (@nomor, @tanggal, @jenis, @pengirim, @penerima, @perihal, @status, @keterangan);
                            SELECT LAST_INSERT_ID();";
            object result = DatabaseHelper.ExecuteScalar(query,
                new MySqlParameter("@nomor", surat.NomorSurat),
                new MySqlParameter("@tanggal", surat.TanggalSurat),
                new MySqlParameter("@jenis", surat.JenisSurat),
                new MySqlParameter("@pengirim", (object)surat.Pengirim ?? DBNull.Value),
                new MySqlParameter("@penerima", (object)surat.Penerima ?? DBNull.Value),
                new MySqlParameter("@perihal", surat.Perihal),
                new MySqlParameter("@status", surat.Status),
                new MySqlParameter("@keterangan", (object)surat.Keterangan ?? DBNull.Value)
            );
            return Convert.ToInt32(result);
        }

        public void Update(Surat surat)
        {
            string query = @"UPDATE surat SET nomor_surat=@nomor, tanggal_surat=@tanggal, jenis_surat=@jenis, pengirim=@pengirim, 
                            penerima=@penerima, perihal=@perihal, status=@status, keterangan=@keterangan, modified_date=NOW() 
                            WHERE id=@id";
            DatabaseHelper.ExecuteNonQuery(query,
                new MySqlParameter("@id", surat.Id),
                new MySqlParameter("@nomor", surat.NomorSurat),
                new MySqlParameter("@tanggal", surat.TanggalSurat),
                new MySqlParameter("@jenis", surat.JenisSurat),
                new MySqlParameter("@pengirim", (object)surat.Pengirim ?? DBNull.Value),
                new MySqlParameter("@penerima", (object)surat.Penerima ?? DBNull.Value),
                new MySqlParameter("@perihal", surat.Perihal),
                new MySqlParameter("@status", surat.Status),
                new MySqlParameter("@keterangan", (object)surat.Keterangan ?? DBNull.Value)
            );
        }

        public void SoftDelete(int id)
        {
            string query = "UPDATE surat SET is_deleted = 1, modified_date = NOW() WHERE id = @id";
            DatabaseHelper.ExecuteNonQuery(query, new MySqlParameter("@id", id));
        }

        public void Restore(int id)
        {
            string query = "UPDATE surat SET is_deleted = 0, modified_date = NOW() WHERE id = @id";
            DatabaseHelper.ExecuteNonQuery(query, new MySqlParameter("@id", id));
        }

        public void PermanentDelete(int id)
        {
            string query = "DELETE FROM surat WHERE id = @id";
            DatabaseHelper.ExecuteNonQuery(query, new MySqlParameter("@id", id));
        }

        public DataTable GetStats()
        {
            string query = @"SELECT 'masuk' AS jenis, COUNT(*) AS total FROM surat WHERE jenis_surat='Masuk' AND is_deleted=0
                            UNION ALL
                            SELECT 'keluar', COUNT(*) FROM surat WHERE jenis_surat='Keluar' AND is_deleted=0
                            UNION ALL
                            SELECT 'bulan_ini', COUNT(*) FROM surat WHERE MONTH(tanggal_surat)=MONTH(CURDATE()) AND YEAR(tanggal_surat)=YEAR(CURDATE()) AND is_deleted=0
                            UNION ALL
                            SELECT 'total', COUNT(*) FROM surat WHERE is_deleted=0";
            return DatabaseHelper.ExecuteQuery(query);
        }

        public DataTable GetRecent(int limit = 10)
        {
            string query = @"SELECT nomor_surat AS `Nomor Surat`, tanggal_surat AS `Tanggal`, jenis_surat AS `Jenis`,
                            CASE WHEN jenis_surat='Masuk' THEN pengirim ELSE penerima END AS `Pengirim/Penerima`,
                            perihal AS `Perihal`, status AS `Status`
                            FROM surat WHERE is_deleted=0
                            ORDER BY tanggal_surat DESC LIMIT @limit";
            return DatabaseHelper.ExecuteQuery(query, new MySqlParameter("@limit", limit));
        }

        public DataTable GetLaporanSummary(DateTime dateFrom, DateTime dateTo)
        {
            string query = @"SELECT jenis_surat, status, COUNT(*) AS total
                            FROM surat
                            WHERE is_deleted=0 AND tanggal_surat >= @from AND tanggal_surat <= @to
                            GROUP BY jenis_surat, status
                            ORDER BY jenis_surat, status";
            return DatabaseHelper.ExecuteQuery(query,
                new MySqlParameter("@from", dateFrom.Date),
                new MySqlParameter("@to", dateTo.Date));
        }

        public DataTable GetLaporanDetail(DateTime dateFrom, DateTime dateTo)
        {
            string query = @"SELECT nomor_surat AS `Nomor Surat`, tanggal_surat AS `Tanggal`, jenis_surat AS `Jenis`,
                            CASE WHEN jenis_surat='Masuk' THEN pengirim ELSE penerima END AS `Pengirim/Penerima`,
                            perihal AS `Perihal`, status AS `Status`
                            FROM surat
                            WHERE is_deleted=0 AND tanggal_surat >= @from AND tanggal_surat <= @to
                            ORDER BY tanggal_surat DESC";
            return DatabaseHelper.ExecuteQuery(query,
                new MySqlParameter("@from", dateFrom.Date),
                new MySqlParameter("@to", dateTo.Date));
        }
    }
}
