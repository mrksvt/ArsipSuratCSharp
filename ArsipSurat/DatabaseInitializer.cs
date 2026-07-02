using System;

namespace ArsipSurat
{
    public static class DatabaseInitializer
    {
        public static bool Initialize()
        {
            try
            {
                string serverConnection = "server=localhost;uid=root;password=;";
                
                string createDatabase = "CREATE DATABASE IF NOT EXISTS arsip_surat";
                DatabaseHelper.ExecuteNonQueryWithConnection(serverConnection, createDatabase);

                string createSurat = @"CREATE TABLE IF NOT EXISTS surat (
                    id INT PRIMARY KEY AUTO_INCREMENT,
                    nomor_surat VARCHAR(50) UNIQUE NOT NULL,
                    tanggal_surat DATE NOT NULL,
                    jenis_surat VARCHAR(10) NOT NULL,
                    pengirim VARCHAR(100),
                    penerima VARCHAR(100),
                    perihal VARCHAR(500) NOT NULL,
                    status VARCHAR(20) NOT NULL,
                    keterangan TEXT,
                    is_ocr_processed TINYINT(1) DEFAULT 0,
                    ocr_confidence DECIMAL(5,2),
                    ocr_raw_text TEXT,
                    created_date DATETIME DEFAULT CURRENT_TIMESTAMP,
                    modified_date DATETIME,
                    is_deleted TINYINT(1) DEFAULT 0,
                    INDEX idx_nomor_surat (nomor_surat),
                    INDEX idx_tanggal_surat (tanggal_surat),
                    INDEX idx_jenis_surat (jenis_surat),
                    INDEX idx_status (status)
                )";

                string createLampiran = @"CREATE TABLE IF NOT EXISTS lampiran (
                    id INT PRIMARY KEY AUTO_INCREMENT,
                    surat_id INT,
                    nama_file VARCHAR(255) NOT NULL,
                    file_path VARCHAR(500) NOT NULL,
                    file_size BIGINT,
                    file_type VARCHAR(50),
                    upload_date DATETIME DEFAULT CURRENT_TIMESTAMP,
                    FOREIGN KEY (surat_id) REFERENCES surat(id)
                )";

                string createCounter = @"CREATE TABLE IF NOT EXISTS nomor_surat_counter (
                    id INT PRIMARY KEY AUTO_INCREMENT,
                    tahun INT NOT NULL,
                    kode_surat VARCHAR(10) NOT NULL,
                    last_number INT NOT NULL DEFAULT 0,
                    UNIQUE KEY uk_tahun_kode (tahun, kode_surat)
                )";

                string createMasterDept = @"CREATE TABLE IF NOT EXISTS master_departemen (
                    id INT PRIMARY KEY AUTO_INCREMENT,
                    nama VARCHAR(100) NOT NULL,
                    keterangan VARCHAR(255),
                    is_active TINYINT(1) DEFAULT 1,
                    created_at DATETIME DEFAULT CURRENT_TIMESTAMP
                )";

                string createUsers = @"CREATE TABLE IF NOT EXISTS users (
                    id INT PRIMARY KEY AUTO_INCREMENT,
                    username VARCHAR(50) UNIQUE NOT NULL,
                    password_hash VARCHAR(255) NOT NULL,
                    email VARCHAR(100),
                    email_set TINYINT(1) DEFAULT 0,
                    two_factor_method VARCHAR(10),
                    two_factor_secret VARCHAR(255),
                    created_at DATETIME DEFAULT CURRENT_TIMESTAMP,
                    last_login_at DATETIME,
                    is_active TINYINT(1) DEFAULT 1
                )";

                DatabaseHelper.ExecuteNonQuery(createSurat);
                DatabaseHelper.ExecuteNonQuery(createLampiran);
                DatabaseHelper.ExecuteNonQuery(createCounter);
                DatabaseHelper.ExecuteNonQuery(createMasterDept);
                DatabaseHelper.ExecuteNonQuery(createUsers);

                try
                {
                    DatabaseHelper.ExecuteNonQuery("ALTER TABLE users ADD COLUMN email_set TINYINT(1) DEFAULT 0");
                }
                catch { }

                // Seed default departments if table is empty
                object deptCount = DatabaseHelper.ExecuteScalar("SELECT COUNT(*) FROM master_departemen");
                if (Convert.ToInt32(deptCount) == 0)
                {
                    DatabaseHelper.ExecuteNonQuery("INSERT INTO master_departemen (nama, keterangan) VALUES ('HRD', 'Human Resources Department')");
                    DatabaseHelper.ExecuteNonQuery("INSERT INTO master_departemen (nama, keterangan) VALUES ('Keuangan', 'Finance Department')");
                    DatabaseHelper.ExecuteNonQuery("INSERT INTO master_departemen (nama, keterangan) VALUES ('Direktur Utama', 'Direksi / Top Management')");
                    DatabaseHelper.ExecuteNonQuery("INSERT INTO master_departemen (nama, keterangan) VALUES ('Umum', 'General Affairs')");
                    DatabaseHelper.ExecuteNonQuery("INSERT INTO master_departemen (nama, keterangan) VALUES ('IT', 'Information Technology')");
                }

                // Seed default admin user if table is empty
                object userCount = DatabaseHelper.ExecuteScalar("SELECT COUNT(*) FROM users");
                if (Convert.ToInt32(userCount) == 0)
                {
                    string hash = PasswordHelper.HashPassword("admin123");
                    DatabaseHelper.ExecuteNonQuery(
                        "INSERT INTO users (username, password_hash, email) VALUES ('admin', @hash, 'admin@arsipsurat.local')",
                        new MySql.Data.MySqlClient.MySqlParameter("@hash", hash));
                }
                return true;
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Database initialization error: " + ex.Message, "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                return false;
            }
        }
    }
}
