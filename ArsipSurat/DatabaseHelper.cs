using System;
using System.Data;
using System.IO;
using MySql.Data.MySqlClient;
using System.Configuration;

namespace ArsipSurat
{
    public static class DatabaseHelper
    {
        private static string ConnectionString = ConfigurationManager.ConnectionStrings["ArsipSuratDb"].ConnectionString;
        private static readonly string LogFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "error.log");

        private static void LogError(Exception ex)
        {
            try
            {
                string message = string.Format("[{0}] {1}\n{2}\n\n",
                    DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    ex.Message,
                    ex.StackTrace);
                File.AppendAllText(LogFile, message);
            }
            catch
            {
            }
        }

        public static int ExecuteNonQuery(string query, params MySqlParameter[] parameters)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(ConnectionString))
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    if (parameters != null) cmd.Parameters.AddRange(parameters);
                    conn.Open();
                    return cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                LogError(ex);
                throw;
            }
        }

        public static int ExecuteNonQueryWithConnection(string connectionString, string query, params MySqlParameter[] parameters)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    if (parameters != null) cmd.Parameters.AddRange(parameters);
                    conn.Open();
                    return cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                LogError(ex);
                throw;
            }
        }

        public static DataTable ExecuteQuery(string query, params MySqlParameter[] parameters)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(ConnectionString))
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                {
                    if (parameters != null) cmd.Parameters.AddRange(parameters);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    return dt;
                }
            }
            catch (Exception ex)
            {
                LogError(ex);
                throw;
            }
        }

        public static object ExecuteScalar(string query, params MySqlParameter[] parameters)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(ConnectionString))
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    if (parameters != null) cmd.Parameters.AddRange(parameters);
                    conn.Open();
                    return cmd.ExecuteScalar();
                }
            }
            catch (Exception ex)
            {
                LogError(ex);
                throw;
            }
        }
    }
}
