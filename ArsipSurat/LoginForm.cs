using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace ArsipSurat
{
    public class LoginForm : Form
    {
        private TextBox txtUsername;
        private TextBox txtPassword;
        private Label lblError;
        private UserRepository userRepo = new UserRepository();

        public LoginForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();

            // Form
            this.Text = "Login — Arsip Surat";
            this.Size = new Size(420, 380);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = Color.FromArgb(241, 226, 209);
            this.Font = new Font("Segoe UI", 9F);

            // Header panel
            var headerPanel = new Panel();
            headerPanel.Dock = DockStyle.Top;
            headerPanel.Height = 90;
            headerPanel.BackColor = Color.FromArgb(84, 26, 26);
            headerPanel.Padding = new Padding(20);

            var lblTitle = new Label();
            lblTitle.Text = "ARSIP SURAT";
            lblTitle.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            lblTitle.ForeColor = Color.White;
            lblTitle.AutoSize = true;
            lblTitle.Location = new Point(20, 15);

            var lblSubtitle = new Label();
            lblSubtitle.Text = "Sistem Manajemen Arsip";
            lblSubtitle.Font = new Font("Segoe UI", 9F);
            lblSubtitle.ForeColor = Color.FromArgb(220, 195, 170);
            lblSubtitle.AutoSize = true;
            lblSubtitle.Location = new Point(22, 50);

            headerPanel.Controls.Add(lblSubtitle);
            headerPanel.Controls.Add(lblTitle);

            // Content panel
            var contentPanel = new Panel();
            contentPanel.Dock = DockStyle.Fill;
            contentPanel.Padding = new Padding(40, 20, 40, 20);

            var lblUser = new Label();
            lblUser.Text = "Username";
            lblUser.Font = new Font("Segoe UI", 9F);
            lblUser.ForeColor = Color.FromArgb(84, 26, 26);
            lblUser.AutoSize = true;
            lblUser.Location = new Point(40, 20);

            txtUsername = new TextBox();
            txtUsername.Location = new Point(40, 42);
            txtUsername.Size = new Size(320, 28);
            txtUsername.Font = new Font("Segoe UI", 10F);

            var lblPass = new Label();
            lblPass.Text = "Password";
            lblPass.Font = new Font("Segoe UI", 9F);
            lblPass.ForeColor = Color.FromArgb(84, 26, 26);
            lblPass.AutoSize = true;
            lblPass.Location = new Point(40, 80);

            txtPassword = new TextBox();
            txtPassword.Location = new Point(40, 102);
            txtPassword.Size = new Size(290, 28);
            txtPassword.Font = new Font("Segoe UI", 10F);
            txtPassword.PasswordChar = '\u25CF';

            var btnEyeOpen = new Button();
            btnEyeOpen.Text = "\u25C9";
            btnEyeOpen.Location = new Point(330, 102);
            btnEyeOpen.Size = new Size(30, 28);
            btnEyeOpen.FlatStyle = FlatStyle.Flat;
            btnEyeOpen.FlatAppearance.BorderSize = 0;
            btnEyeOpen.BackColor = Color.FromArgb(241, 226, 209);
            btnEyeOpen.ForeColor = Color.FromArgb(84, 26, 26);
            btnEyeOpen.Font = new Font("Segoe UI Symbol", 12F);
            btnEyeOpen.Cursor = Cursors.Hand;
            btnEyeOpen.FlatAppearance.MouseOverBackColor = Color.FromArgb(220, 195, 170);
            btnEyeOpen.Visible = false;

            var btnEyeClosed = new Button();
            btnEyeClosed.Text = "\u25CB";
            btnEyeClosed.Location = new Point(330, 102);
            btnEyeClosed.Size = new Size(30, 28);
            btnEyeClosed.FlatStyle = FlatStyle.Flat;
            btnEyeClosed.FlatAppearance.BorderSize = 0;
            btnEyeClosed.BackColor = Color.FromArgb(241, 226, 209);
            btnEyeClosed.ForeColor = Color.FromArgb(150, 150, 150);
            btnEyeClosed.Font = new Font("Segoe UI Symbol", 12F);
            btnEyeClosed.Cursor = Cursors.Hand;
            btnEyeClosed.FlatAppearance.MouseOverBackColor = Color.FromArgb(220, 195, 170);

            btnEyeOpen.Click += (s, ev) => { txtPassword.PasswordChar = '\u25CF'; btnEyeOpen.Visible = false; btnEyeClosed.Visible = true; };
            btnEyeClosed.Click += (s, ev) => { txtPassword.PasswordChar = '\0'; btnEyeClosed.Visible = false; btnEyeOpen.Visible = true; };

            lblError = new Label();
            lblError.Text = "";
            lblError.Font = new Font("Segoe UI", 9F);
            lblError.ForeColor = Color.FromArgb(220, 53, 69);
            lblError.AutoSize = true;
            lblError.Location = new Point(40, 145);
            lblError.Visible = false;

            var btnLogin = new Button();
            btnLogin.Text = "Login";
            btnLogin.Location = new Point(40, 180);
            btnLogin.Size = new Size(320, 40);
            btnLogin.FlatStyle = FlatStyle.Flat;
            btnLogin.FlatAppearance.BorderSize = 0;
            btnLogin.BackColor = Color.FromArgb(129, 11, 56);
            btnLogin.ForeColor = Color.White;
            btnLogin.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            btnLogin.Cursor = Cursors.Hand;
            btnLogin.FlatAppearance.MouseOverBackColor = Color.FromArgb(107, 10, 47);
            btnLogin.Click += btnLogin_Click;

            // Version label
            var lblVersion = new Label();
            lblVersion.Text = "v1.0.0";
            lblVersion.Font = new Font("Segoe UI", 8F);
            lblVersion.ForeColor = Color.FromArgb(180, 180, 180);
            lblVersion.AutoSize = true;
            lblVersion.Location = new Point(40, 240);

            contentPanel.Controls.Add(lblVersion);
            contentPanel.Controls.Add(btnLogin);
            contentPanel.Controls.Add(lblError);
            contentPanel.Controls.Add(btnEyeOpen);
            contentPanel.Controls.Add(btnEyeClosed);
            contentPanel.Controls.Add(txtPassword);
            contentPanel.Controls.Add(lblPass);
            contentPanel.Controls.Add(txtUsername);
            contentPanel.Controls.Add(lblUser);

            this.Controls.Add(contentPanel);
            this.Controls.Add(headerPanel);

            this.AcceptButton = btnLogin;
            this.ResumeLayout(false);
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text;

            if (string.IsNullOrEmpty(username))
            {
                ShowError("Username wajib diisi");
                txtUsername.Focus();
                return;
            }
            if (string.IsNullOrEmpty(password))
            {
                ShowError("Password wajib diisi");
                txtPassword.Focus();
                return;
            }

            try
            {
                DataRow user = userRepo.GetByUsername(username);

                if (user == null)
                {
                    ShowError("Username atau password salah");
                    txtPassword.Focus();
                    return;
                }

                bool isActive = user["is_active"] != DBNull.Value && Convert.ToBoolean(user["is_active"]);
                if (!isActive)
                {
                    ShowError("Akun ini tidak aktif. Hubungi administrator.");
                    return;
                }

                string storedHash = user["password_hash"].ToString();
                bool valid = PasswordHelper.VerifyPassword(password, storedHash);

                if (!valid)
                {
                    ShowError("Username atau password salah");
                    txtPassword.Focus();
                    return;
                }

                // Login sukses
                int userId = Convert.ToInt32(user["id"]);
                string email = user["email"] == DBNull.Value ? "" : user["email"].ToString();
                string twoFactor = user["two_factor_method"] == DBNull.Value ? "" : user["two_factor_method"].ToString();

                if (!string.IsNullOrEmpty(twoFactor))
                {
                    if (twoFactor == "totp")
                    {
                        string secret = user["two_factor_secret"] == DBNull.Value ? "" : user["two_factor_secret"].ToString();
                        if (string.IsNullOrEmpty(secret))
                        {
                            ShowError("TOTP belum dikonfigurasi. Hubungi administrator.");
                            return;
                        }

                        string totpResult = ShowTotpDialog();
                        if (totpResult == null) return;

                        if (totpResult == "EMAIL_FALLBACK")
                        {
                            if (string.IsNullOrEmpty(email))
                            {
                                ShowError("Email belum diatur. Tidak dapat mengirim OTP.");
                                return;
                            }
                            string otp = GenerateOtp();
                            try { EmailHelper.SendOtpEmail(email, otp, "login"); }
                            catch (Exception ex) { ShowError("Gagal mengirim OTP: " + ex.Message); return; }

                            string enteredOtp = ShowOtpDialog();
                            if (enteredOtp == null) return;
                            if (enteredOtp != otp) { ShowError("Kode OTP tidak valid."); return; }
                        }
                        else
                        {
                            if (!TotpHelper.VerifyCode(secret, totpResult))
                            {
                                ShowError("Kode TOTP tidak valid.");
                                return;
                            }
                        }
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(email))
                        {
                            ShowError("2FA aktif tapi email belum diatur. Hubungi administrator.");
                            return;
                        }
                        string otp = GenerateOtp();
                        try { EmailHelper.SendOtpEmail(email, otp, "login"); }
                        catch (Exception ex) { ShowError("Gagal mengirim OTP: " + ex.Message); return; }

                        string enteredOtp = ShowOtpDialog();
                        if (enteredOtp == null) return;
                        if (enteredOtp != otp) { ShowError("Kode OTP tidak valid."); return; }
                    }
                }

                CurrentSession.SetUser(userId, username, email);
                userRepo.UpdateLastLogin(userId);

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                ShowError("Error: " + ex.Message);
            }
        }

        private void ShowError(string message)
        {
            lblError.Text = message;
            lblError.Visible = true;
        }

        private string GenerateOtp()
        {
            var rng = new Random();
            return rng.Next(100000, 999999).ToString();
        }

        private string ShowOtpDialog()
        {
            var form = new Form();
            form.Text = "Verifikasi OTP";
            form.Size = new Size(380, 200);
            form.StartPosition = FormStartPosition.CenterParent;
            form.FormBorderStyle = FormBorderStyle.FixedDialog;
            form.MaximizeBox = false;
            form.MinimizeBox = false;
            form.BackColor = Color.FromArgb(241, 226, 209);
            form.Font = new Font("Segoe UI", 9F);

            var lblInfo = new Label { Text = "Masukkan kode OTP yang dikirim ke email:", AutoSize = true, Location = new Point(30, 25), ForeColor = Color.FromArgb(84, 26, 26) };
            var txtOtp = new TextBox { Location = new Point(30, 50), Size = new Size(300, 28), Font = new Font("Segoe UI", 14F), MaxLength = 6, TextAlign = HorizontalAlignment.Center };
            var lblErr = new Label { Text = "", AutoSize = true, Location = new Point(30, 85), ForeColor = Color.FromArgb(220, 53, 69), Visible = false };

            var btnVerify = new Button { Text = "Verifikasi", Location = new Point(30, 115), Size = new Size(140, 35) };
            btnVerify.FlatStyle = FlatStyle.Flat;
            btnVerify.FlatAppearance.BorderSize = 0;
            btnVerify.BackColor = Color.FromArgb(129, 11, 56);
            btnVerify.ForeColor = Color.White;
            btnVerify.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            btnVerify.Cursor = Cursors.Hand;
            btnVerify.FlatAppearance.MouseOverBackColor = Color.FromArgb(107, 10, 47);

            var btnCancel = new Button { Text = "Batal", Location = new Point(185, 115), Size = new Size(140, 35) };
            btnCancel.FlatStyle = FlatStyle.Flat;
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.BackColor = Color.FromArgb(220, 195, 170);
            btnCancel.ForeColor = Color.FromArgb(84, 26, 26);
            btnCancel.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            btnCancel.Cursor = Cursors.Hand;
            btnCancel.FlatAppearance.MouseOverBackColor = Color.FromArgb(196, 168, 142);

            btnVerify.Click += (s2, ev2) =>
            {
                if (txtOtp.Text.Trim().Length != 6)
                {
                    lblErr.Text = "OTP harus 6 digit";
                    lblErr.Visible = true;
                    return;
                }
                form.DialogResult = DialogResult.OK;
                form.Close();
            };
            btnCancel.Click += (s2, ev2) => { form.DialogResult = DialogResult.Cancel; form.Close(); };

            form.Controls.AddRange(new Control[] { lblInfo, txtOtp, lblErr, btnVerify, btnCancel });
            form.AcceptButton = btnVerify;
            form.CancelButton = btnCancel;

            if (form.ShowDialog() == DialogResult.OK)
                return txtOtp.Text.Trim();
            return null;
        }

        private string ShowTotpDialog()
        {
            var form = new Form();
            form.Text = "Verifikasi 2FA";
            form.Size = new Size(380, 230);
            form.StartPosition = FormStartPosition.CenterParent;
            form.FormBorderStyle = FormBorderStyle.FixedDialog;
            form.MaximizeBox = false;
            form.MinimizeBox = false;
            form.BackColor = Color.FromArgb(241, 226, 209);
            form.Font = new Font("Segoe UI", 9F);

            var lblInfo = new Label { Text = "Masukkan kode dari Authenticator:", AutoSize = true, Location = new Point(30, 25), ForeColor = Color.FromArgb(84, 26, 26) };
            var txtCode = new TextBox { Location = new Point(30, 50), Size = new Size(300, 28), Font = new Font("Segoe UI", 14F), MaxLength = 6, TextAlign = HorizontalAlignment.Center };
            var lblErr = new Label { Text = "", AutoSize = true, Location = new Point(30, 85), ForeColor = Color.FromArgb(220, 53, 69), Visible = false };

            var btnVerify = new Button { Text = "Verifikasi", Location = new Point(30, 115), Size = new Size(140, 35) };
            btnVerify.FlatStyle = FlatStyle.Flat;
            btnVerify.FlatAppearance.BorderSize = 0;
            btnVerify.BackColor = Color.FromArgb(129, 11, 56);
            btnVerify.ForeColor = Color.White;
            btnVerify.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            btnVerify.Cursor = Cursors.Hand;
            btnVerify.FlatAppearance.MouseOverBackColor = Color.FromArgb(107, 10, 47);

            var btnFallback = new Button { Text = "Gunakan OTP Email", Location = new Point(30, 155), Size = new Size(300, 30) };
            btnFallback.FlatStyle = FlatStyle.Flat;
            btnFallback.FlatAppearance.BorderSize = 0;
            btnFallback.BackColor = Color.FromArgb(220, 195, 170);
            btnFallback.ForeColor = Color.FromArgb(84, 26, 26);
            btnFallback.Font = new Font("Segoe UI", 8.5F);
            btnFallback.Cursor = Cursors.Hand;
            btnFallback.FlatAppearance.MouseOverBackColor = Color.FromArgb(196, 168, 142);

            var btnCancel = new Button { Text = "Batal", Location = new Point(185, 115), Size = new Size(140, 35) };
            btnCancel.FlatStyle = FlatStyle.Flat;
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.BackColor = Color.FromArgb(220, 195, 170);
            btnCancel.ForeColor = Color.FromArgb(84, 26, 26);
            btnCancel.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            btnCancel.Cursor = Cursors.Hand;
            btnCancel.FlatAppearance.MouseOverBackColor = Color.FromArgb(196, 168, 142);

            string result = null;
            btnVerify.Click += (s2, ev2) =>
            {
                if (txtCode.Text.Trim().Length != 6)
                {
                    lblErr.Text = "Kode harus 6 digit";
                    lblErr.Visible = true;
                    return;
                }
                result = txtCode.Text.Trim();
                form.DialogResult = DialogResult.OK;
                form.Close();
            };
            btnFallback.Click += (s2, ev2) => { result = "EMAIL_FALLBACK"; form.DialogResult = DialogResult.OK; form.Close(); };
            btnCancel.Click += (s2, ev2) => { form.DialogResult = DialogResult.Cancel; form.Close(); };

            form.Controls.AddRange(new Control[] { lblInfo, txtCode, lblErr, btnVerify, btnCancel, btnFallback });
            form.AcceptButton = btnVerify;
            form.CancelButton = btnCancel;

            form.ShowDialog();
            return result;
        }
    }
}
