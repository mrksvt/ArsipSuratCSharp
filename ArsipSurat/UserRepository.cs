using System;
using System.Data;
using MySql.Data.MySqlClient;

namespace ArsipSurat
{
    public class UserRepository
    {
        public DataRow GetByUsername(string username)
        {
            string query = "SELECT * FROM users WHERE username = @username";
            DataTable dt = DatabaseHelper.ExecuteQuery(query, new MySqlParameter("@username", username));
            if (dt.Rows.Count == 0) return null;
            return dt.Rows[0];
        }

        public void UpdateLastLogin(int userId)
        {
            string query = "UPDATE users SET last_login_at = NOW() WHERE id = @id";
            DatabaseHelper.ExecuteNonQuery(query, new MySqlParameter("@id", userId));
        }

        public void UpdatePassword(int userId, string passwordHash)
        {
            string query = "UPDATE users SET password_hash = @hash WHERE id = @id";
            DatabaseHelper.ExecuteNonQuery(query,
                new MySqlParameter("@hash", passwordHash),
                new MySqlParameter("@id", userId));
        }

        public void UpdateEmail(int userId, string email)
        {
            string query = "UPDATE users SET email = @email WHERE id = @id";
            DatabaseHelper.ExecuteNonQuery(query,
                new MySqlParameter("@email", email),
                new MySqlParameter("@id", userId));
        }

        public void SetEmailSet(int userId)
        {
            string query = "UPDATE users SET email_set = 1 WHERE id = @id";
            DatabaseHelper.ExecuteNonQuery(query, new MySqlParameter("@id", userId));
        }

        public void SetTwoFactor(int userId, string method)
        {
            string query = "UPDATE users SET two_factor_method = @method WHERE id = @id";
            DatabaseHelper.ExecuteNonQuery(query,
                new MySqlParameter("@method", method),
                new MySqlParameter("@id", userId));
        }

        public void SetTwoFactorSecret(int userId, string secret)
        {
            string query = "UPDATE users SET two_factor_secret = @secret WHERE id = @id";
            DatabaseHelper.ExecuteNonQuery(query,
                new MySqlParameter("@secret", secret),
                new MySqlParameter("@id", userId));
        }
    }
}
