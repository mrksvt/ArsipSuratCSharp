using System;
using System.Configuration;
using System.Net;
using System.Net.Mail;

namespace ArsipSurat
{
    public static class EmailHelper
    {
        public static void SendOtpEmail(string toEmail, string otp, string purpose = "verifikasi")
        {
            string host = ConfigurationManager.AppSettings["SmtpHost"];
            int port = int.Parse(ConfigurationManager.AppSettings["SmtpPort"]);
            string user = ConfigurationManager.AppSettings["SmtpUser"];
            string password = ConfigurationManager.AppSettings["SmtpPassword"];
            string from = ConfigurationManager.AppSettings["SmtpFrom"];
            bool enableSsl = ConfigurationManager.AppSettings["SmtpEnableSsl"] == "true";

            using (var client = new SmtpClient(host, port))
            {
                client.EnableSsl = enableSsl;
                client.Credentials = new NetworkCredential(user, password);
                client.Timeout = 15000;

                using (var message = new MailMessage())
                {
                    message.From = new MailAddress(from);
                    message.To.Add(toEmail);
                    message.Subject = "Kode Verifikasi OTP - Arsip Surat";
                    message.IsBodyHtml = false;
                    message.Body = "Kode OTP verifikasi Anda untuk " + purpose + ":\n\n"
                        + "    " + otp + "\n\n"
                        + "Kode ini berlaku untuk satu kali penggunaan.\n"
                        + "Jangan bagikan kode ini kepada siapapun.\n\n"
                        + "---\n"
                        + "Arsip Surat - Sistem Manajemen Arsip";

                    client.Send(message);
                }
            }
        }
    }
}
