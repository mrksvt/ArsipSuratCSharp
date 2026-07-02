using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ArsipSurat
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            if (!DatabaseInitializer.Initialize())
            {
                return;
            }

            // Show login form first
            using (var loginForm = new LoginForm())
            {
                if (loginForm.ShowDialog() != DialogResult.OK)
                {
                    return; // User closed login or failed
                }
            }

            Application.Run(new MainForm());
        }
    }
}
