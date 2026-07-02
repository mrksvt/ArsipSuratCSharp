using System;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using QRCoder;

namespace ArsipSurat
{
    public partial class MainForm : Form
    {
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, string lParam);
        private const int EM_SETCUEBANNER = 0x1501;

        private SuratRepository repository = new SuratRepository();
        private LampiranRepository lampiranRepo = new LampiranRepository();
        private Timer searchTimer;
        private Timer clockTimer;
        private Panel activeMenuPanel;

        // Dashboard controls
        private Panel DashboardPanel;
        private Label lblDashMasuk;
        private Label lblDashKeluar;
        private Label lblDashBulanIni;
        private Label lblDashTotal;
        private DataGridView dgvRecent;

        // Nomor Surat controls
        private Panel NomorSuratPanel;
        private NomorSuratRepository nomorRepo = new NomorSuratRepository();
        private MasterDepartemenRepository deptRepo = new MasterDepartemenRepository();
        private TextBox txtNomorKode;
        private ComboBox cmbNomorDept;
        private TextBox txtNomorPerihal;
        private TextBox txtNomorPenerima;
        private Label lblNomorPreview;
        private Label lblNomorFile;
        private string selectedDocxPath;
        private DataGridView dgvDashBreakdown;

        // Master Data controls
        private Panel MasterDataPanel;
        private DataGridView dgvMasterDept;
        private TextBox txtMasterSearch;

        // Pengaturan controls
        private Panel PengaturanPanel;
        private UserRepository userRepo = new UserRepository();
        
        private readonly Color PrimaryColor = Color.FromArgb(129, 11, 56);      // #810B38 deep wine
        private readonly Color PrimaryHover = Color.FromArgb(107, 10, 47);     // #6B0A2F darker wine
        private readonly Color DangerColor = Color.FromArgb(129, 11, 56);      // #810B38 wine (consistent palette)
        private readonly Color DangerHover = Color.FromArgb(107, 10, 47);      // #6B0A2F
        private readonly Color SecondaryColor = Color.FromArgb(220, 195, 170); // #DCC3AA tan/beige
        private readonly Color SecondaryHover = Color.FromArgb(196, 168, 142); // #C4A88E darker tan
        private readonly Color BgColor = Color.FromArgb(241, 226, 209);        // #F1E2D1 cream
        private readonly Color HeaderBg = Color.FromArgb(84, 26, 26);          // #541A1A dark maroon
        private readonly Color HeaderFg = Color.White;
        private readonly Color RowAlt = Color.FromArgb(241, 226, 209);         // #F1E2D1 cream
        private readonly Color GridLine = Color.FromArgb(220, 195, 170);       // #DCC3AA tan
        private readonly Color DarkBase = Color.FromArgb(84, 26, 26);          // #541A1A
        private readonly Color AccentColor = Color.FromArgb(129, 11, 56);      // #810B38

        public MainForm()
        {
            InitializeComponent();
            string logoPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "uhn.png");
            if (System.IO.File.Exists(logoPath))
                picLogo.Image = System.Drawing.Image.FromFile(logoPath);
            searchTimer = new Timer();
            searchTimer.Interval = 300;
            searchTimer.Tick += SearchTimer_Tick;
            clockTimer = new Timer();
            clockTimer.Interval = 1000;
            clockTimer.Tick += clockTimer_Tick;
            clockTimer.Start();
            ApplyTheme();
            ApplySearchPlaceholder();
            cmbFilterJenis.SelectedIndex = 0;
            cmbFilterStatus.SelectedIndex = 0;
            LoadData();
            UpdateStatusBar(0);
            InitializeDashboard();
            InitializeNomorSurat();
            InitializeMasterData();
            InitializePengaturan();
            // Show current user in navbar
            lblNavUser.Text = CurrentSession.Username ?? "admin";
            // Default view: Dashboard
            ShowDashboard();
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            // Re-run LoadData after form reaches final screen size so DGV Fill
            // columns calculate widths from the actual container, not design-time size.
            LoadData(txtSearch.Text);
        }

        private void ApplyTheme()
        {
            this.BackColor = BgColor;
            this.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
            
            StyleButton(btnNew, PrimaryColor, PrimaryHover);
            StyleButton(btnEdit, SecondaryColor, SecondaryHover);
            btnEdit.ForeColor = DarkBase;
            StyleButton(btnDelete, DangerColor, DangerHover);
            StyleButton(btnRefresh, SecondaryColor, SecondaryHover);
            btnRefresh.ForeColor = DarkBase;
            StyleButton(btnExport, SecondaryColor, SecondaryHover);
            btnExport.ForeColor = DarkBase;
            StyleButton(btnPrint, SecondaryColor, SecondaryHover);
            btnPrint.ForeColor = DarkBase;
            StyleButton(btnResetFilter, SecondaryColor, SecondaryHover);
            btnResetFilter.ForeColor = DarkBase;
            StyleButton(btnRestore, SecondaryColor, SecondaryHover);
            btnRestore.ForeColor = DarkBase;
            StyleButton(btnPermanentDelete, DangerColor, DangerHover);
            
            txtSearch.Font = new Font("Segoe UI", 9F);
            txtSearch.Size = new Size(220, 25);
            
            dgvSurat.BackgroundColor = Color.White;
            dgvSurat.BorderStyle = BorderStyle.None;
            dgvSurat.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvSurat.GridColor = GridLine;
            dgvSurat.EnableHeadersVisualStyles = false;
            dgvSurat.ColumnHeadersDefaultCellStyle.BackColor = HeaderBg;
            dgvSurat.ColumnHeadersDefaultCellStyle.ForeColor = HeaderFg;
            dgvSurat.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            dgvSurat.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvSurat.ColumnHeadersDefaultCellStyle.Padding = new Padding(5);
            dgvSurat.ColumnHeadersHeight = 35;
            dgvSurat.RowTemplate.Height = 30;
            dgvSurat.RowTemplate.DefaultCellStyle.Font = new Font("Segoe UI", 9F);
            dgvSurat.RowTemplate.DefaultCellStyle.Padding = new Padding(5);
            dgvSurat.AlternatingRowsDefaultCellStyle.BackColor = RowAlt;
            dgvSurat.DefaultCellStyle.SelectionBackColor = SecondaryColor;
            dgvSurat.DefaultCellStyle.SelectionForeColor = DarkBase;
            dgvSurat.RowHeadersVisible = true;
            dgvSurat.RowHeadersWidth = 35;
            dgvSurat.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            foreach (Control c in filterPanel.Controls)
            {
                if (c is Label && c.Name.StartsWith("labelFilter"))
                {
                    c.ForeColor = DarkBase;
                    c.Font = new Font("Segoe UI", 9F);
                }
                if (c is ComboBox)
                {
                    var cmb = (ComboBox)c;
                    cmb.FlatStyle = FlatStyle.Flat;
                    cmb.Font = new Font("Segoe UI", 9F);
                    cmb.BackColor = Color.White;
                    cmb.ForeColor = DarkBase;
                }
                if (c is DateTimePicker)
                {
                    var dtp = (DateTimePicker)c;
                    dtp.Font = new Font("Segoe UI", 9F);
                    dtp.CalendarForeColor = DarkBase;
                    dtp.CalendarMonthBackground = Color.White;
                }
            }

            ApplyNavbarTheme();
            ApplySidebarTheme();
            ApplyContentTheme();
            ApplyFooterTheme();
        }

        private void ApplyNavbarTheme()
        {
            NavbarPanel.BackColor = DarkBase;
        }

        private void ApplySidebarTheme()
        {
            SidebarPanel.BackColor = Color.FromArgb(61, 19, 19); // #3D1313
        }

        private void ApplyContentTheme()
        {
            ContentPanel.BackColor = BgColor;
            CardPanel.BackColor = Color.FromArgb(255, 250, 245); // #FFFAF5 warm white
        }

        private void ApplyFooterTheme()
        {
            FooterPanel.BackColor = SecondaryColor;
        }

        private void clockTimer_Tick(object sender, EventArgs e)
        {
            lblNavClock.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
        }

        private void SidebarMenu_MouseEnter(object sender, EventArgs e)
        {
            Control ctrl = sender as Control;
            if (ctrl is Panel)
            {
                Panel pnl = (Panel)ctrl;
                if (pnl.BackColor.ToArgb() != AccentColor.ToArgb())
                {
                    pnl.BackColor = Color.FromArgb(77, 26, 26); // #4D1A1A hover
                }
            }
            else if (ctrl is Label)
            {
                Panel parent = ctrl.Parent as Panel;
                if (parent != null && parent.BackColor.ToArgb() != AccentColor.ToArgb())
                {
                    parent.BackColor = Color.FromArgb(77, 26, 26); // #4D1A1A hover
                }
            }
        }

        private void SidebarMenu_MouseLeave(object sender, EventArgs e)
        {
            Control ctrl = sender as Control;
            if (ctrl is Panel)
            {
                Panel pnl = (Panel)ctrl;
                if (pnl != activeMenuPanel)
                {
                    pnl.BackColor = Color.FromArgb(61, 19, 19); // #3D1313 default
                }
            }
            else if (ctrl is Label)
            {
                Panel parent = ctrl.Parent as Panel;
                if (parent != null && parent != activeMenuPanel)
                {
                    parent.BackColor = Color.FromArgb(61, 19, 19); // #3D1313 default
                }
            }
        }

        private void lblMenuDashboard_Click(object sender, EventArgs e)
        {
            ShowDashboard();
        }

        private void lblMenuLaporan_Click(object sender, EventArgs e)
        {
            ShowNomorSurat();
        }

        private void lblMenuArsip_Click(object sender, EventArgs e)
        {
            ShowArsip();
        }

        private void lblMenuMasterData_Click(object sender, EventArgs e)
        {
            ShowMasterData();
        }

        private void lblMenuPengaturan_Click(object sender, EventArgs e)
        {
            ShowPengaturan();
        }

        private void lblNavLogout_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Yakin logout?", "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                CurrentSession.Clear();
                this.Hide();
                using (var loginForm = new LoginForm())
                {
                    if (loginForm.ShowDialog() == DialogResult.OK)
                    {
                        lblNavUser.Text = CurrentSession.Username;
                        this.Show();
                        ShowDashboard();
                    }
                    else
                    {
                        Application.Exit();
                    }
                }
            }
        }

        private void lblMenuLogout_Paint(object sender, PaintEventArgs e)
        {
            using (var pen = new Pen(Color.FromArgb(100, 229, 115, 115), 1))
            {
                e.Graphics.DrawLine(pen, 0, 1, e.ClipRectangle.Width, 1);
            }
        }

        private void ResetSidebarHighlight()
        {
            pnlMenuDashboard.BackColor = Color.FromArgb(61, 19, 19);
            lblMenuDashboard.Font = new Font("Segoe UI", 9F);
            lblMenuDashboard.ForeColor = SecondaryColor;
            pnlMenuArsip.BackColor = Color.FromArgb(61, 19, 19);
            lblMenuArsip.Font = new Font("Segoe UI", 9F);
            lblMenuArsip.ForeColor = SecondaryColor;
            pnlMenuLaporan.BackColor = Color.FromArgb(61, 19, 19);
            lblMenuLaporan.Font = new Font("Segoe UI", 9F);
            lblMenuLaporan.ForeColor = SecondaryColor;
            pnlMenuMasterData.BackColor = Color.FromArgb(61, 19, 19);
            lblMenuMasterData.Font = new Font("Segoe UI", 9F);
            lblMenuMasterData.ForeColor = SecondaryColor;
            pnlMenuPengaturan.BackColor = Color.FromArgb(61, 19, 19);
            lblMenuPengaturan.Font = new Font("Segoe UI", 9F);
            lblMenuPengaturan.ForeColor = SecondaryColor;
            pnlMenuLogout.BackColor = Color.FromArgb(61, 19, 19);
            lblMenuLogout.ForeColor = Color.FromArgb(229, 115, 115);
        }

        private void HighlightMenu(Panel pnl, Label lbl)
        {
            activeMenuPanel = pnl;
            pnl.BackColor = AccentColor;
            lbl.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lbl.ForeColor = Color.White;
        }

        private void HideAllPanels()
        {
            DashboardPanel.Visible = false;
            CardPanel.Visible = false;
            NomorSuratPanel.Visible = false;
            MasterDataPanel.Visible = false;
            PengaturanPanel.Visible = false;
        }

        private void ShowDashboard()
        {
            HideAllPanels();
            DashboardPanel.Visible = true;
            lblBreadcrumbPage.Text = "Dashboard";
            ResetSidebarHighlight();
            HighlightMenu(pnlMenuDashboard, lblMenuDashboard);
            LoadDashboardData();
        }

        private void ShowArsip()
        {
            HideAllPanels();
            CardPanel.Visible = true;
            lblBreadcrumbPage.Text = "Arsip Surat > Daftar Surat";
            ResetSidebarHighlight();
            HighlightMenu(pnlMenuArsip, lblMenuArsip);
            LoadData(txtSearch.Text);
        }

        private void ShowNomorSurat()
        {
            HideAllPanels();
            NomorSuratPanel.Visible = true;
            lblBreadcrumbPage.Text = "Nomor Surat";
            ResetSidebarHighlight();
            HighlightMenu(pnlMenuLaporan, lblMenuLaporan);
            LoadDeptDropdown();
            UpdateNomorPreview();
        }

        private void ShowMasterData()
        {
            HideAllPanels();
            MasterDataPanel.Visible = true;
            lblBreadcrumbPage.Text = "Master Data";
            ResetSidebarHighlight();
            HighlightMenu(pnlMenuMasterData, lblMenuMasterData);
            LoadMasterDeptData();
        }

        private void ShowPengaturan()
        {
            HideAllPanels();
            PengaturanPanel.Visible = true;
            lblBreadcrumbPage.Text = "Pengaturan";
            ResetSidebarHighlight();
            HighlightMenu(pnlMenuPengaturan, lblMenuPengaturan);
        }

        private void InitializePengaturan()
        {
            PengaturanPanel = new Panel();
            PengaturanPanel.Dock = DockStyle.Fill;
            PengaturanPanel.BackColor = Color.FromArgb(255, 250, 245);
            PengaturanPanel.BorderStyle = BorderStyle.FixedSingle;
            PengaturanPanel.Padding = new Padding(15);
            PengaturanPanel.Visible = false;

            // ── Email section ──
            var emailPanel = new Panel();
            emailPanel.Dock = DockStyle.Top;
            emailPanel.Height = 125;
            emailPanel.Padding = new Padding(5);

            var lblEmailTitle = new Label { Text = "Email & Keamanan", Font = new Font("Segoe UI", 11F, FontStyle.Bold), ForeColor = DarkBase, AutoSize = true, Location = new Point(5, 5) };
            var lblEmailCurrent = new Label { Text = "Email saat ini:", Font = new Font("Segoe UI", 9F), ForeColor = DarkBase, AutoSize = true, Location = new Point(5, 35) };
            var lblEmailValue = new Label { Text = "(memuat...)", Font = new Font("Segoe UI", 9F, FontStyle.Bold), ForeColor = AccentColor, AutoSize = true, Location = new Point(120, 35) };
            var lblEmailInput = new Label { Text = "Email baru:", Font = new Font("Segoe UI", 9F), ForeColor = DarkBase, AutoSize = true, Location = new Point(5, 62) };
            var txtEmail = new TextBox { Location = new Point(120, 59), Size = new Size(280, 24), Font = new Font("Segoe UI", 9F) };
            var btnEmailSave = new Button { Text = "Simpan Email", Location = new Point(410, 57), Size = new Size(120, 28) };
            btnEmailSave.FlatStyle = FlatStyle.Flat;
            btnEmailSave.FlatAppearance.BorderSize = 0;
            btnEmailSave.BackColor = AccentColor;
            btnEmailSave.ForeColor = Color.White;
            btnEmailSave.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            btnEmailSave.Cursor = Cursors.Hand;
            btnEmailSave.FlatAppearance.MouseOverBackColor = PrimaryHover;

            var lbl2FA = new Label { Text = "Metode 2FA:", Font = new Font("Segoe UI", 9F), ForeColor = DarkBase, AutoSize = true, Location = new Point(5, 92) };
            var cmb2FA = new ComboBox { Location = new Point(120, 89), Size = new Size(180, 24), Font = new Font("Segoe UI", 9F), DropDownStyle = ComboBoxStyle.DropDownList };
            cmb2FA.Items.AddRange(new object[] { "Nonaktif", "Email OTP", "Authenticator (TOTP)" });
            cmb2FA.SelectedIndex = 0;

            var totpPanel = new Panel { Location = new Point(5, 120), Size = new Size(540, 220), Visible = false };
            var picQr = new PictureBox { Location = new Point(0, 0), Size = new Size(150, 150), SizeMode = PictureBoxSizeMode.Zoom, BorderStyle = BorderStyle.FixedSingle, BackColor = Color.White };
            var lblTotpKey = new Label { Text = "Secret Key:", Font = new Font("Segoe UI", 9F), ForeColor = DarkBase, AutoSize = true, Location = new Point(160, 5) };
            var lblTotpKeyValue = new Label { Text = "", Font = new Font("Consolas", 9F, FontStyle.Bold), ForeColor = AccentColor, AutoSize = true, Location = new Point(240, 5) };
            var lblTotpUri = new Label { Text = "Scan QR code atau masukkan key manual di Authenticator app.", Font = new Font("Segoe UI", 8F), ForeColor = Color.FromArgb(108, 117, 125), AutoSize = true, Location = new Point(160, 25), MaximumSize = new Size(380, 0) };
            var lblTotpVerify = new Label { Text = "Kode verifikasi:", Font = new Font("Segoe UI", 9F), ForeColor = DarkBase, AutoSize = true, Location = new Point(0, 165) };
            var txtTotpVerify = new TextBox { Location = new Point(110, 162), Size = new Size(100, 24), Font = new Font("Segoe UI", 10F), MaxLength = 6 };
            var btnTotpActivate = new Button { Text = "Aktifkan", Location = new Point(220, 160), Size = new Size(90, 28) };
            btnTotpActivate.FlatStyle = FlatStyle.Flat;
            btnTotpActivate.FlatAppearance.BorderSize = 0;
            btnTotpActivate.BackColor = AccentColor;
            btnTotpActivate.ForeColor = Color.White;
            btnTotpActivate.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            btnTotpActivate.Cursor = Cursors.Hand;
            btnTotpActivate.FlatAppearance.MouseOverBackColor = PrimaryHover;

            totpPanel.Controls.AddRange(new Control[] { picQr, lblTotpKey, lblTotpKeyValue, lblTotpUri, lblTotpVerify, txtTotpVerify, btnTotpActivate });

            // ── TOTP Status Panel (shown when 2FA TOTP is already active) ──
            var totpStatusPanel = new Panel { Location = new Point(5, 120), Size = new Size(540, 55), Visible = false };
            var lblTotpStatus = new Label
            {
                Text = "\u2713 Autentikasi 2 Faktor (TOTP) sudah AKTIF",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = Color.FromArgb(34, 139, 34),
                AutoSize = true,
                Location = new Point(0, 5)
            };
            var btnTotpDisable = new Button
            {
                Text = "Nonaktifkan 2FA",
                Location = new Point(0, 30),
                Size = new Size(130, 22),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.White,
                ForeColor = Color.FromArgb(220, 53, 69),
                Font = new Font("Segoe UI", 8F),
                Cursor = Cursors.Hand
            };
            btnTotpDisable.FlatAppearance.BorderSize = 1;
            btnTotpDisable.FlatAppearance.BorderColor = Color.FromArgb(220, 53, 69);
            totpStatusPanel.Controls.AddRange(new Control[] { lblTotpStatus, btnTotpDisable });

            string pendingTotpSecret = null;
            bool suppress2FAEvent = false;

            Action<int> resizeEmailPanel = (extra) => { emailPanel.Height = 125 + extra; };

            cmb2FA.SelectedIndexChanged += (s, ev) =>
            {
                if (suppress2FAEvent) return;
                DataRow u = userRepo.GetByUsername(CurrentSession.Username);
                if (u == null) return;
                string currentMethod = u["two_factor_method"] == DBNull.Value ? "" : u["two_factor_method"].ToString();

                if (cmb2FA.SelectedIndex == 0)
                {
                    if (!string.IsNullOrEmpty(currentMethod))
                    {
                        string currentEmail = u["email"] == DBNull.Value ? "" : u["email"].ToString();
                        if (string.IsNullOrEmpty(currentEmail))
                        {
                            MessageBox.Show("Email belum diatur. Tidak dapat verifikasi.", "Gagal", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            suppress2FAEvent = true; cmb2FA.SelectedIndex = currentMethod == "totp" ? 2 : 1; suppress2FAEvent = false;
                            return;
                        }
                        string otp = GenerateOtp();
                        try { EmailHelper.SendOtpEmail(currentEmail, otp, "verifikasi perubahan email"); }
                        catch (Exception ex) { MessageBox.Show("Gagal mengirim OTP.\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); suppress2FAEvent = true; cmb2FA.SelectedIndex = currentMethod == "totp" ? 2 : 1; suppress2FAEvent = false; return; }
                        MessageBox.Show("Kode OTP telah dikirim ke: " + currentEmail, "Verifikasi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        string enteredOtp = ShowOtpDialog();
                        if (enteredOtp == null || enteredOtp != otp)
                        {
                            MessageBox.Show("OTP tidak valid. 2FA tetap aktif.", "Gagal", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            suppress2FAEvent = true; cmb2FA.SelectedIndex = currentMethod == "totp" ? 2 : 1; suppress2FAEvent = false;
                            return;
                        }
                        userRepo.SetTwoFactor(CurrentSession.UserId, null);
                        userRepo.SetTwoFactorSecret(CurrentSession.UserId, null);
                        totpPanel.Visible = false;
                        resizeEmailPanel(0);
                        MessageBox.Show("2FA dinonaktifkan.", "Pengaturan 2FA", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else if (cmb2FA.SelectedIndex == 1)
                {
                    userRepo.SetTwoFactor(CurrentSession.UserId, "email");
                    userRepo.SetTwoFactorSecret(CurrentSession.UserId, null);
                    totpPanel.Visible = false;
                    resizeEmailPanel(0);
                    MessageBox.Show("2FA Email OTP diaktifkan.", "Pengaturan 2FA", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (cmb2FA.SelectedIndex == 2)
                {
                    pendingTotpSecret = TotpHelper.GenerateSecret();
                    lblTotpKeyValue.Text = pendingTotpSecret;
                    string uri = TotpHelper.GetOtpAuthUri(CurrentSession.Username, pendingTotpSecret);
                    txtTotpVerify.Text = "";
                    try
                    {
                        var qrGenerator = new QRCodeGenerator();
                        var qrCodeData = qrGenerator.CreateQrCode(uri, QRCodeGenerator.ECCLevel.Q);
                        var qrCode = new QRCode(qrCodeData);
                        var qrBitmap = qrCode.GetGraphic(20);
                        picQr.Image?.Dispose();
                        picQr.Image = qrBitmap;
                    }
                    catch (Exception ex)
                    {
                        picQr.Image = null;
                        System.Diagnostics.Debug.WriteLine("QR generate error: " + ex.Message);
                    }
                    totpPanel.Visible = true;
                    resizeEmailPanel(220);
                }
            };

            btnTotpActivate.Click += (s, ev) =>
            {
                string code = txtTotpVerify.Text.Trim();
                if (code.Length != 6)
                {
                    MessageBox.Show("Masukkan kode 6 digit dari Authenticator.", "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (!TotpHelper.VerifyCode(pendingTotpSecret, code))
                {
                    MessageBox.Show("Kode tidak valid. Coba lagi.", "Gagal", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                userRepo.SetTwoFactor(CurrentSession.UserId, "totp");
                userRepo.SetTwoFactorSecret(CurrentSession.UserId, pendingTotpSecret);
                pendingTotpSecret = null;
                picQr.Image?.Dispose();
                picQr.Image = null;
                lblTotpKeyValue.Text = "";
                txtTotpVerify.Text = "";
                totpPanel.Visible = false;
                totpStatusPanel.Visible = true;
                resizeEmailPanel(0);
                MessageBox.Show("TOTP berhasil diaktifkan!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
            };

            Action loadEmail = () =>
            {
                DataRow u = userRepo.GetByUsername(CurrentSession.Username);
                if (u != null)
                {
                    string em = u["email"] == DBNull.Value ? "" : u["email"].ToString();
                    bool isSet = u["email_set"] != DBNull.Value && Convert.ToBoolean(u["email_set"]);
                    string twoFactor = u["two_factor_method"] == DBNull.Value ? "" : u["two_factor_method"].ToString();
                    lblEmailValue.Text = string.IsNullOrEmpty(em) ? "(belum diatur)" : em;
                    suppress2FAEvent = true;
                    if (twoFactor == "totp") cmb2FA.SelectedIndex = 2;
                    else if (twoFactor == "email") cmb2FA.SelectedIndex = 1;
                    else cmb2FA.SelectedIndex = 0;
                    suppress2FAEvent = false;
                    totpPanel.Visible = false;
                    totpStatusPanel.Visible = twoFactor == "totp";
                    resizeEmailPanel(0);
                    if (isSet)
                    {
                        lblEmailInput.Text = "Email baru (perlu OTP):";
                        btnEmailSave.Text = "Verifikasi & Simpan";
                    }
                    else
                    {
                        lblEmailInput.Text = "Email baru:";
                        btnEmailSave.Text = "Simpan Email";
                    }
                }
            };

            btnEmailSave.Click += (s, ev) =>
            {
                string newEmail = txtEmail.Text.Trim();
                if (string.IsNullOrEmpty(newEmail) || !newEmail.Contains("@"))
                {
                    MessageBox.Show("Masukkan email yang valid.", "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                DataRow u = userRepo.GetByUsername(CurrentSession.Username);
                if (u == null) return;
                bool isSet = u["email_set"] != DBNull.Value && Convert.ToBoolean(u["email_set"]);

                if (isSet)
                {
                    string currentEmail = u["email"] == DBNull.Value ? "" : u["email"].ToString();
                    if (string.IsNullOrEmpty(currentEmail))
                    {
                        MessageBox.Show("Email sebelumnya tidak ditemukan. Hubungi admin.", "Gagal", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    string otp = GenerateOtp();
                    try
                    {
                        EmailHelper.SendOtpEmail(currentEmail, otp, "verifikasi perubahan email");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Gagal mengirim OTP ke email lama.\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    MessageBox.Show("Kode OTP telah dikirim ke email lama: " + currentEmail, "OTP Terkirim", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    string enteredOtp = ShowOtpDialog();
                    if (enteredOtp == null) return;
                    if (enteredOtp != otp)
                    {
                        MessageBox.Show("Kode OTP tidak valid. Email gagal diubah.", "Gagal", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                try
                {
                    userRepo.UpdateEmail(CurrentSession.UserId, newEmail);
                    if (!isSet) userRepo.SetEmailSet(CurrentSession.UserId);
                    CurrentSession.SetUser(CurrentSession.UserId, CurrentSession.Username, newEmail);
                    lblEmailValue.Text = newEmail;
                    txtEmail.Text = "";
                    lblEmailInput.Text = "Email baru (perlu OTP):";
                    btnEmailSave.Text = "Verifikasi & Simpan";
                    MessageBox.Show("Email berhasil disimpan!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Gagal menyimpan email.\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
             };

             btnTotpDisable.Click += (s, ev) =>
             {
                 if (MessageBox.Show("Yakin ingin menonaktifkan 2FA TOTP?", "Konfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                     return;

                 try
                 {
                     userRepo.SetTwoFactor(CurrentSession.UserId, null);
                     userRepo.SetTwoFactorSecret(CurrentSession.UserId, null);
                     pendingTotpSecret = TotpHelper.GenerateSecret();
                     lblTotpKeyValue.Text = pendingTotpSecret;
                     string uri = TotpHelper.GetOtpAuthUri(CurrentSession.Username, pendingTotpSecret);
                     txtTotpVerify.Text = "";
                     try
                     {
                         var qrGenerator = new QRCodeGenerator();
                         var qrCodeData = qrGenerator.CreateQrCode(uri, QRCodeGenerator.ECCLevel.Q);
                         var qrCode = new QRCode(qrCodeData);
                         var qrBitmap = qrCode.GetGraphic(20);
                         picQr.Image?.Dispose();
                         picQr.Image = qrBitmap;
                     }
                     catch (Exception ex)
                     {
                         picQr.Image = null;
                         System.Diagnostics.Debug.WriteLine("QR generate error: " + ex.Message);
                     }
                     totpPanel.Visible = true;
                     totpStatusPanel.Visible = false;
                     suppress2FAEvent = true;
                     cmb2FA.SelectedIndex = 0;
                     suppress2FAEvent = false;
                     resizeEmailPanel(220);
                     MessageBox.Show("2FA TOTP berhasil dinonaktifkan.", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                 }
                 catch (Exception ex)
                 {
                     MessageBox.Show("Gagal menonaktifkan 2FA.\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                 }
             };

             emailPanel.Controls.Add(totpPanel);
             emailPanel.Controls.Add(totpStatusPanel);
             emailPanel.Controls.Add(cmb2FA);
            emailPanel.Controls.Add(lbl2FA);
            emailPanel.Controls.Add(btnEmailSave);
            emailPanel.Controls.Add(txtEmail);
            emailPanel.Controls.Add(lblEmailInput);
            emailPanel.Controls.Add(lblEmailValue);
            emailPanel.Controls.Add(lblEmailCurrent);
            emailPanel.Controls.Add(lblEmailTitle);

            // ── Separator ──
            var separator = new Panel();
            separator.Dock = DockStyle.Top;
            separator.Height = 1;
            separator.BackColor = GridLine;

            // ── Password section ──
            var lblPwTitle = new Label();
            lblPwTitle.Dock = DockStyle.Top;
            lblPwTitle.Text = "Ganti Password";
            lblPwTitle.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblPwTitle.ForeColor = DarkBase;
            lblPwTitle.Height = 35;
            lblPwTitle.TextAlign = ContentAlignment.MiddleLeft;

            var formPanel = new Panel();
            formPanel.Dock = DockStyle.Top;
            formPanel.Height = 260;
            formPanel.Padding = new Padding(5);

            int y = 10;
            int lblX = 5, inputX = 180, inputW = 270;

            var lblOld = new Label { Text = "Password Lama:", AutoSize = true, Font = new Font("Segoe UI", 9F), ForeColor = DarkBase, Location = new Point(lblX, y + 3) };
            var txtOld = new TextBox { Location = new Point(inputX, y), Size = new Size(inputW, 24), Font = new Font("Segoe UI", 10F), PasswordChar = '\u25CF' };
            var eyeX = inputX + inputW + 2;
            var btnEyeOldOpen = new Button { Text = "\u25C9", Location = new Point(eyeX, y), Size = new Size(28, 24), FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI Symbol", 11F), Cursor = Cursors.Hand, BackColor = Color.FromArgb(255, 250, 245), ForeColor = DarkBase, Visible = false };
            btnEyeOldOpen.FlatAppearance.BorderSize = 0;
            btnEyeOldOpen.FlatAppearance.MouseOverBackColor = SecondaryColor;
            var btnEyeOldClosed = new Button { Text = "\u25CB", Location = new Point(eyeX, y), Size = new Size(28, 24), FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI Symbol", 11F), Cursor = Cursors.Hand, BackColor = Color.FromArgb(255, 250, 245), ForeColor = Color.FromArgb(150, 150, 150) };
            btnEyeOldClosed.FlatAppearance.BorderSize = 0;
            btnEyeOldClosed.FlatAppearance.MouseOverBackColor = SecondaryColor;
            btnEyeOldOpen.Click += (s2, ev2) => { txtOld.PasswordChar = '\u25CF'; btnEyeOldOpen.Visible = false; btnEyeOldClosed.Visible = true; };
            btnEyeOldClosed.Click += (s2, ev2) => { txtOld.PasswordChar = '\0'; btnEyeOldClosed.Visible = false; btnEyeOldOpen.Visible = true; };
            y += 38;

            var lblNew = new Label { Text = "Password Baru:", AutoSize = true, Font = new Font("Segoe UI", 9F), ForeColor = DarkBase, Location = new Point(lblX, y + 3) };
            var txtNew = new TextBox { Location = new Point(inputX, y), Size = new Size(inputW, 24), Font = new Font("Segoe UI", 10F), PasswordChar = '\u25CF' };
            var btnEyeNewOpen = new Button { Text = "\u25C9", Location = new Point(eyeX, y), Size = new Size(28, 24), FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI Symbol", 11F), Cursor = Cursors.Hand, BackColor = Color.FromArgb(255, 250, 245), ForeColor = DarkBase, Visible = false };
            btnEyeNewOpen.FlatAppearance.BorderSize = 0;
            btnEyeNewOpen.FlatAppearance.MouseOverBackColor = SecondaryColor;
            var btnEyeNewClosed = new Button { Text = "\u25CB", Location = new Point(eyeX, y), Size = new Size(28, 24), FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI Symbol", 11F), Cursor = Cursors.Hand, BackColor = Color.FromArgb(255, 250, 245), ForeColor = Color.FromArgb(150, 150, 150) };
            btnEyeNewClosed.FlatAppearance.BorderSize = 0;
            btnEyeNewClosed.FlatAppearance.MouseOverBackColor = SecondaryColor;
            btnEyeNewOpen.Click += (s2, ev2) => { txtNew.PasswordChar = '\u25CF'; btnEyeNewOpen.Visible = false; btnEyeNewClosed.Visible = true; };
            btnEyeNewClosed.Click += (s2, ev2) => { txtNew.PasswordChar = '\0'; btnEyeNewClosed.Visible = false; btnEyeNewOpen.Visible = true; };
            y += 28;

            var lblStrength = new Label { Text = "", AutoSize = true, Font = new Font("Segoe UI", 8F), ForeColor = Color.FromArgb(108, 117, 125), Location = new Point(inputX, y) };
            y += 25;

            var lblConfirm = new Label { Text = "Konfirmasi Password:", AutoSize = true, Font = new Font("Segoe UI", 9F), ForeColor = DarkBase, Location = new Point(lblX, y + 3) };
            var txtConfirm = new TextBox { Location = new Point(inputX, y), Size = new Size(inputW, 24), Font = new Font("Segoe UI", 10F), PasswordChar = '\u25CF' };
            y += 45;

            var btnSave = new Button { Text = "Simpan Perubahan", Location = new Point(inputX, y), Size = new Size(180, 38) };
            btnSave.FlatStyle = FlatStyle.Flat;
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.BackColor = AccentColor;
            btnSave.ForeColor = Color.White;
            btnSave.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            btnSave.Cursor = Cursors.Hand;
            btnSave.FlatAppearance.MouseOverBackColor = PrimaryHover;
            btnSave.FlatAppearance.MouseDownBackColor = Color.FromArgb(90, 8, 38);
            y += 48;

            var lblMessage = new Label { Text = "", AutoSize = true, Font = new Font("Segoe UI", 9F), Location = new Point(inputX, y), MaximumSize = new Size(inputW, 0) };

            txtNew.TextChanged += (s, ev) =>
            {
                string pw = txtNew.Text;
                if (pw.Length == 0) { lblStrength.Text = ""; return; }
                int score = 0;
                if (pw.Length >= 8) score++;
                if (pw.Length >= 12) score++;
                if (System.Text.RegularExpressions.Regex.IsMatch(pw, "[A-Z]")) score++;
                if (System.Text.RegularExpressions.Regex.IsMatch(pw, "[0-9]")) score++;
                if (System.Text.RegularExpressions.Regex.IsMatch(pw, "[^a-zA-Z0-9]")) score++;
                if (score <= 1) { lblStrength.Text = "Kekuatan: Lemah"; lblStrength.ForeColor = Color.FromArgb(220, 53, 69); }
                else if (score <= 3) { lblStrength.Text = "Kekuatan: Sedang"; lblStrength.ForeColor = Color.FromArgb(255, 193, 7); }
                else { lblStrength.Text = "Kekuatan: Kuat"; lblStrength.ForeColor = Color.FromArgb(40, 167, 69); }
            };

            btnSave.Click += (s, ev) =>
            {
                lblMessage.Text = "";
                string oldPw = txtOld.Text;
                string newPw = txtNew.Text;
                string confPw = txtConfirm.Text;

                if (string.IsNullOrEmpty(oldPw) || string.IsNullOrEmpty(newPw) || string.IsNullOrEmpty(confPw))
                {
                    lblMessage.Text = "Semua field wajib diisi";
                    lblMessage.ForeColor = Color.FromArgb(220, 53, 69);
                    return;
                }

                DataRow user = userRepo.GetByUsername(CurrentSession.Username);
                if (user == null)
                {
                    lblMessage.Text = "User tidak ditemukan";
                    lblMessage.ForeColor = Color.FromArgb(220, 53, 69);
                    return;
                }

                string storedHash = user["password_hash"].ToString();
                if (!PasswordHelper.VerifyPassword(oldPw, storedHash))
                {
                    lblMessage.Text = "Password lama tidak sesuai";
                    lblMessage.ForeColor = Color.FromArgb(220, 53, 69);
                    txtOld.Focus();
                    return;
                }

                if (newPw.Length < 8)
                {
                    lblMessage.Text = "Password baru minimal 8 karakter";
                    lblMessage.ForeColor = Color.FromArgb(220, 53, 69);
                    return;
                }

                if (newPw == oldPw)
                {
                    lblMessage.Text = "Password baru tidak boleh sama dengan password lama";
                    lblMessage.ForeColor = Color.FromArgb(220, 53, 69);
                    return;
                }

                if (newPw != confPw)
                {
                    lblMessage.Text = "Konfirmasi password tidak cocok";
                    lblMessage.ForeColor = Color.FromArgb(220, 53, 69);
                    return;
                }

                if (MessageBox.Show("Apakah yakin akan mengubah password?", "Konfirmasi",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                    return;

                string otp = GenerateOtp();
                string userEmail = user["email"] == DBNull.Value ? "" : user["email"].ToString();
                if (string.IsNullOrEmpty(userEmail))
                {
                    MessageBox.Show("Email belum diatur. Silakan atur email terlebih dahulu di bagian atas.", "Gagal", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                try
                {
                    EmailHelper.SendOtpEmail(userEmail, otp, "mengubah password");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Gagal mengirim email OTP.\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                MessageBox.Show("Kode OTP telah dikirim ke email: " + userEmail, "OTP Terkirim", MessageBoxButtons.OK, MessageBoxIcon.Information);

                string enteredOtp = ShowOtpDialog();
                if (enteredOtp == null) return;

                if (enteredOtp != otp)
                {
                    MessageBox.Show("Kode OTP tidak valid. Password gagal diubah.", "Gagal", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                try
                {
                    string newHash = PasswordHelper.HashPassword(newPw);
                    userRepo.UpdatePassword(CurrentSession.UserId, newHash);
                    MessageBox.Show("Password berhasil diubah!", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtOld.Text = "";
                    txtNew.Text = "";
                    txtConfirm.Text = "";
                    lblStrength.Text = "";
                    lblMessage.Text = "";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Gagal menyimpan password.\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };

            formPanel.Controls.Add(lblMessage);
            formPanel.Controls.Add(btnSave);
            formPanel.Controls.Add(txtConfirm);
            formPanel.Controls.Add(lblConfirm);
            formPanel.Controls.Add(lblStrength);
            formPanel.Controls.Add(btnEyeNewOpen);
            formPanel.Controls.Add(btnEyeNewClosed);
            formPanel.Controls.Add(txtNew);
            formPanel.Controls.Add(lblNew);
            formPanel.Controls.Add(btnEyeOldOpen);
            formPanel.Controls.Add(btnEyeOldClosed);
            formPanel.Controls.Add(txtOld);
            formPanel.Controls.Add(lblOld);

            // Z-order: last added = docked first from top
            PengaturanPanel.Controls.Add(formPanel);
            PengaturanPanel.Controls.Add(lblPwTitle);
            PengaturanPanel.Controls.Add(separator);
            PengaturanPanel.Controls.Add(emailPanel);

            ContentPanel.Controls.Add(PengaturanPanel);
            PengaturanPanel.BringToFront();
            loadEmail();
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
            form.BackColor = Color.FromArgb(255, 250, 245);
            form.Font = new Font("Segoe UI", 9F);

            var lblInfo = new Label { Text = "Masukkan kode OTP:", AutoSize = true, Location = new Point(30, 25), ForeColor = DarkBase };
            var txtOtp = new TextBox { Location = new Point(30, 50), Size = new Size(300, 28), Font = new Font("Segoe UI", 14F), MaxLength = 6, TextAlign = HorizontalAlignment.Center };
            var lblError = new Label { Text = "", AutoSize = true, Location = new Point(30, 85), ForeColor = Color.FromArgb(220, 53, 69), Visible = false };

            var btnVerify = new Button { Text = "Verifikasi", Location = new Point(30, 115), Size = new Size(140, 35) };
            btnVerify.FlatStyle = FlatStyle.Flat;
            btnVerify.FlatAppearance.BorderSize = 0;
            btnVerify.BackColor = AccentColor;
            btnVerify.ForeColor = Color.White;
            btnVerify.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            btnVerify.Cursor = Cursors.Hand;
            btnVerify.FlatAppearance.MouseOverBackColor = PrimaryHover;

            var btnCancel = new Button { Text = "Batal", Location = new Point(185, 115), Size = new Size(140, 35) };
            btnCancel.FlatStyle = FlatStyle.Flat;
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.BackColor = SecondaryColor;
            btnCancel.ForeColor = DarkBase;
            btnCancel.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            btnCancel.Cursor = Cursors.Hand;
            btnCancel.FlatAppearance.MouseOverBackColor = SecondaryHover;

            btnVerify.Click += (s2, ev2) =>
            {
                if (txtOtp.Text.Trim().Length != 6)
                {
                    lblError.Text = "OTP harus 6 digit";
                    lblError.Visible = true;
                    return;
                }
                form.DialogResult = DialogResult.OK;
                form.Close();
            };
            btnCancel.Click += (s2, ev2) => { form.DialogResult = DialogResult.Cancel; form.Close(); };

            form.Controls.AddRange(new Control[] { lblInfo, txtOtp, lblError, btnVerify, btnCancel });
            form.AcceptButton = btnVerify;
            form.CancelButton = btnCancel;

            if (form.ShowDialog() == DialogResult.OK)
                return txtOtp.Text.Trim();
            return null;
        }

        private void InitializeDashboard()
        {
            // Outer panel — same structure as CardPanel
            DashboardPanel = new Panel();
            DashboardPanel.Dock = DockStyle.Fill;
            DashboardPanel.BackColor = Color.FromArgb(255, 250, 245);
            DashboardPanel.BorderStyle = BorderStyle.FixedSingle;
            DashboardPanel.Padding = new Padding(15);
            DashboardPanel.Visible = false;

            // ── Stats cards row ──
            var statsTable = new TableLayoutPanel();
            statsTable.Dock = DockStyle.Top;
            statsTable.Height = 110;
            statsTable.ColumnCount = 4;
            statsTable.RowCount = 1;
            statsTable.ColumnStyles.Clear();
            statsTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            statsTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            statsTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            statsTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            statsTable.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            statsTable.Padding = new Padding(0, 5, 0, 10);
            statsTable.GrowStyle = TableLayoutPanelGrowStyle.FixedSize;

            lblDashMasuk = CreateStatCard("0", Color.FromArgb(41, 98, 255));
            lblDashKeluar = CreateStatCard("0", Color.FromArgb(23, 162, 184));
            lblDashBulanIni = CreateStatCard("0", Color.FromArgb(255, 193, 7));
            lblDashTotal = CreateStatCard("0", AccentColor);

            statsTable.Controls.Add(CreateStatCardPanel("📧", "Surat Masuk", lblDashMasuk, Color.FromArgb(41, 98, 255)), 0, 0);
            statsTable.Controls.Add(CreateStatCardPanel("📤", "Surat Keluar", lblDashKeluar, Color.FromArgb(23, 162, 184)), 1, 0);
            statsTable.Controls.Add(CreateStatCardPanel("📅", "Bulan Ini", lblDashBulanIni, Color.FromArgb(255, 193, 7)), 2, 0);
            statsTable.Controls.Add(CreateStatCardPanel("📊", "Total Surat", lblDashTotal, AccentColor), 3, 0);

            // ── Recent grid section ──
            var recentHeader = new Label();
            recentHeader.Dock = DockStyle.Top;
            recentHeader.Text = "10 Surat Terakhir";
            recentHeader.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            recentHeader.ForeColor = DarkBase;
            recentHeader.Height = 35;
            recentHeader.TextAlign = ContentAlignment.MiddleLeft;

            dgvRecent = new DataGridView();
            dgvRecent.Dock = DockStyle.Fill;
            dgvRecent.ReadOnly = true;
            dgvRecent.AllowUserToAddRows = false;
            dgvRecent.AllowUserToDeleteRows = false;
            dgvRecent.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvRecent.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvRecent.BackgroundColor = Color.White;
            dgvRecent.BorderStyle = BorderStyle.None;
            dgvRecent.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvRecent.GridColor = GridLine;
            dgvRecent.EnableHeadersVisualStyles = false;
            dgvRecent.ColumnHeadersDefaultCellStyle.BackColor = HeaderBg;
            dgvRecent.ColumnHeadersDefaultCellStyle.ForeColor = HeaderFg;
            dgvRecent.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            dgvRecent.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvRecent.ColumnHeadersDefaultCellStyle.Padding = new Padding(5);
            dgvRecent.ColumnHeadersHeight = 35;
            dgvRecent.RowTemplate.Height = 28;
            dgvRecent.RowTemplate.DefaultCellStyle.Font = new Font("Segoe UI", 9F);
            dgvRecent.RowTemplate.DefaultCellStyle.Padding = new Padding(5);
            dgvRecent.AlternatingRowsDefaultCellStyle.BackColor = RowAlt;
            dgvRecent.DefaultCellStyle.SelectionBackColor = SecondaryColor;
            dgvRecent.DefaultCellStyle.SelectionForeColor = DarkBase;
            dgvRecent.RowHeadersVisible = true;
            dgvRecent.RowHeadersWidth = 35;

            // ── Breakdown table (migrated from Laporan) ──
            var breakdownHeader = new Label();
            breakdownHeader.Dock = DockStyle.Top;
            breakdownHeader.Text = "Rekap Bulanan — Jenis × Status";
            breakdownHeader.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            breakdownHeader.ForeColor = DarkBase;
            breakdownHeader.Height = 35;
            breakdownHeader.TextAlign = ContentAlignment.MiddleLeft;

            dgvDashBreakdown = new DataGridView();
            dgvDashBreakdown.Dock = DockStyle.Top;
            dgvDashBreakdown.Height = 100;
            dgvDashBreakdown.ReadOnly = true;
            dgvDashBreakdown.AllowUserToAddRows = false;
            dgvDashBreakdown.AllowUserToDeleteRows = false;
            dgvDashBreakdown.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvDashBreakdown.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvDashBreakdown.BackgroundColor = Color.White;
            dgvDashBreakdown.BorderStyle = BorderStyle.None;
            dgvDashBreakdown.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvDashBreakdown.GridColor = GridLine;
            dgvDashBreakdown.EnableHeadersVisualStyles = false;
            dgvDashBreakdown.ColumnHeadersDefaultCellStyle.BackColor = HeaderBg;
            dgvDashBreakdown.ColumnHeadersDefaultCellStyle.ForeColor = HeaderFg;
            dgvDashBreakdown.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            dgvDashBreakdown.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvDashBreakdown.ColumnHeadersDefaultCellStyle.Padding = new Padding(5);
            dgvDashBreakdown.ColumnHeadersHeight = 32;
            dgvDashBreakdown.RowTemplate.Height = 26;
            dgvDashBreakdown.RowTemplate.DefaultCellStyle.Font = new Font("Segoe UI", 9F);
            dgvDashBreakdown.RowTemplate.DefaultCellStyle.Padding = new Padding(5);
            dgvDashBreakdown.AlternatingRowsDefaultCellStyle.BackColor = RowAlt;
            dgvDashBreakdown.DefaultCellStyle.SelectionBackColor = SecondaryColor;
            dgvDashBreakdown.DefaultCellStyle.SelectionForeColor = DarkBase;
            dgvDashBreakdown.RowHeadersVisible = false;

            // Z-order: Fill last
            DashboardPanel.Controls.Add(dgvRecent);
            DashboardPanel.Controls.Add(recentHeader);
            DashboardPanel.Controls.Add(dgvDashBreakdown);
            DashboardPanel.Controls.Add(breakdownHeader);
            DashboardPanel.Controls.Add(statsTable);

            // Add to ContentPanel, same parent as CardPanel
            ContentPanel.Controls.Add(DashboardPanel);
            DashboardPanel.BringToFront();
        }

        private Panel CreateStatCardPanel(string icon, string title, Label numberLabel, Color accentColor)
        {
            var card = new Panel();
            card.Dock = DockStyle.Fill;
            card.BackColor = Color.White;
            card.Margin = new Padding(4, 0, 4, 0);
            card.Padding = new Padding(14, 10, 14, 10);

            // Left accent border via a thin panel
            var accent = new Panel();
            accent.Dock = DockStyle.Left;
            accent.Width = 4;
            accent.BackColor = accentColor;

            var lblIcon = new Label();
            lblIcon.Text = icon;
            lblIcon.Font = new Font("Segoe UI", 16F);
            lblIcon.AutoSize = true;
            lblIcon.Location = new Point(22, 8);

            var lblTitle = new Label();
            lblTitle.Text = title;
            lblTitle.Font = new Font("Segoe UI", 8.5F);
            lblTitle.ForeColor = Color.FromArgb(108, 117, 125);
            lblTitle.AutoSize = true;
            lblTitle.Location = new Point(22, 42);

            numberLabel.Location = new Point(22, 60);
            numberLabel.AutoSize = true;

            card.Controls.Add(numberLabel);
            card.Controls.Add(lblTitle);
            card.Controls.Add(lblIcon);
            card.Controls.Add(accent);

            return card;
        }

        private Label CreateStatCard(string value, Color accent)
        {
            var lbl = new Label();
            lbl.Text = value;
            lbl.Font = new Font("Segoe UI", 22F, FontStyle.Bold);
            lbl.ForeColor = DarkBase;
            return lbl;
        }

        private void LoadDashboardData()
        {
            try
            {
                DataTable stats = repository.GetStats();
                foreach (DataRow row in stats.Rows)
                {
                    string jenis = row["jenis"].ToString();
                    string total = row["total"].ToString();
                    switch (jenis)
                    {
                        case "masuk": lblDashMasuk.Text = total; break;
                        case "keluar": lblDashKeluar.Text = total; break;
                        case "bulan_ini": lblDashBulanIni.Text = total; break;
                        case "total": lblDashTotal.Text = total; break;
                    }
                }

                DataTable recent = repository.GetRecent(10);
                dgvRecent.DataSource = recent;

                if (dgvRecent.Columns.Contains("Nomor Surat"))
                {
                    dgvRecent.Columns["Nomor Surat"].FillWeight = 20;
                    dgvRecent.Columns["Nomor Surat"].MinimumWidth = 100;
                }
                if (dgvRecent.Columns.Contains("Tanggal"))
                {
                    dgvRecent.Columns["Tanggal"].FillWeight = 12;
                    dgvRecent.Columns["Tanggal"].MinimumWidth = 80;
                }
                if (dgvRecent.Columns.Contains("Jenis"))
                {
                    dgvRecent.Columns["Jenis"].FillWeight = 10;
                    dgvRecent.Columns["Jenis"].MinimumWidth = 60;
                }
                if (dgvRecent.Columns.Contains("Pengirim/Penerima"))
                {
                    dgvRecent.Columns["Pengirim/Penerima"].FillWeight = 18;
                    dgvRecent.Columns["Pengirim/Penerima"].MinimumWidth = 100;
                }
                if (dgvRecent.Columns.Contains("Perihal"))
                {
                    dgvRecent.Columns["Perihal"].FillWeight = 25;
                    dgvRecent.Columns["Perihal"].MinimumWidth = 120;
                }
                if (dgvRecent.Columns.Contains("Status"))
                {
                    dgvRecent.Columns["Status"].FillWeight = 12;
                    dgvRecent.Columns["Status"].MinimumWidth = 70;
                }

                // Uppercase headers
                foreach (DataGridViewColumn col in dgvRecent.Columns)
                {
                    if (col.Visible) col.HeaderText = col.HeaderText.ToUpper();
                }

                // Color Jenis column
                foreach (DataGridViewRow row in dgvRecent.Rows)
                {
                    string jenis = row.Cells["Jenis"].Value == null ? "" : row.Cells["Jenis"].Value.ToString();
                    if (jenis == "Masuk")
                        row.Cells["Jenis"].Style.ForeColor = Color.FromArgb(41, 98, 255);
                    else if (jenis == "Keluar")
                        row.Cells["Jenis"].Style.ForeColor = Color.FromArgb(23, 162, 184);
                }

                // Breakdown Jenis × Status (bulan berjalan)
                DateTime monthStart = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
                DateTime monthEnd = DateTime.Today;
                DataTable summary = repository.GetLaporanSummary(monthStart, monthEnd);

                var breakdown = new DataTable();
                breakdown.Columns.Add("Jenis", typeof(string));
                breakdown.Columns.Add("Draft", typeof(int));
                breakdown.Columns.Add("Diterima", typeof(int));
                breakdown.Columns.Add("Diproses", typeof(int));
                breakdown.Columns.Add("Selesai", typeof(int));
                breakdown.Columns.Add("Total", typeof(int));

                foreach (string j in new[] { "Masuk", "Keluar" })
                {
                    var bRow = breakdown.NewRow();
                    bRow["Jenis"] = j;
                    int rowTotal = 0;
                    foreach (DataRow sRow in summary.Rows)
                    {
                        if (sRow["jenis_surat"].ToString() == j)
                        {
                            string st = sRow["status"].ToString();
                            int cnt = Convert.ToInt32(sRow["total"]);
                            if (breakdown.Columns.Contains(st)) bRow[st] = cnt;
                            rowTotal += cnt;
                        }
                    }
                    bRow["Total"] = rowTotal;
                    breakdown.Rows.Add(bRow);
                }
                dgvDashBreakdown.DataSource = breakdown;
                foreach (DataGridViewColumn col in dgvDashBreakdown.Columns)
                    col.HeaderText = col.HeaderText.ToUpper();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Dashboard error: " + ex.Message);
            }
        }

        private void InitializeNomorSurat()
        {
            NomorSuratPanel = new Panel();
            NomorSuratPanel.Dock = DockStyle.Fill;
            NomorSuratPanel.BackColor = Color.FromArgb(255, 250, 245);
            NomorSuratPanel.BorderStyle = BorderStyle.FixedSingle;
            NomorSuratPanel.Padding = new Padding(15);
            NomorSuratPanel.Visible = false;

            // ── Title ──
            var lblTitle = new Label();
            lblTitle.Dock = DockStyle.Top;
            lblTitle.Text = "Generator Nomor Surat Keluar";
            lblTitle.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblTitle.ForeColor = DarkBase;
            lblTitle.Height = 35;
            lblTitle.TextAlign = ContentAlignment.MiddleLeft;

            // ── Form panel ──
            var formPanel = new Panel();
            formPanel.Dock = DockStyle.Top;
            formPanel.Height = 220;
            formPanel.Padding = new Padding(5);

            int y = 5;
            int lblX = 5, inputX = 140, inputW = 250;

            // Kode Surat
            var lblKode = MakeLabel("Kode Surat:", lblX, y + 3);
            txtNomorKode = MakeInput(inputX, y, inputW, "SK");
            txtNomorKode.TextChanged += (s, ev) => UpdateNomorPreview();
            y += 32;

            // Departemen (dropdown from Master Data)
            var lblDept = MakeLabel("Departemen:", lblX, y + 3);
            cmbNomorDept = new ComboBox();
            cmbNomorDept.Location = new Point(inputX, y);
            cmbNomorDept.Size = new Size(inputW, 24);
            cmbNomorDept.Font = new Font("Segoe UI", 9F);
            cmbNomorDept.FlatStyle = FlatStyle.Flat;
            cmbNomorDept.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbNomorDept.BackColor = Color.White;
            cmbNomorDept.ForeColor = DarkBase;
            cmbNomorDept.SelectedIndexChanged += (s, ev) => UpdateNomorPreview();
            y += 32;

            // Perihal
            var lblPerihal = MakeLabel("Perihal:", lblX, y + 3);
            txtNomorPerihal = MakeInput(inputX, y, inputW, "");
            y += 32;

            // Penerima
            var lblPenerima = MakeLabel("Penerima:", lblX, y + 3);
            txtNomorPenerima = MakeInput(inputX, y, inputW, "");
            y += 32;

            // File upload row
            var lblFile = MakeLabel("File DOCX:", lblX, y + 3);
            lblNomorFile = new Label();
            lblNomorFile.Text = "(belum dipilih)";
            lblNomorFile.Font = new Font("Segoe UI", 9F);
            lblNomorFile.ForeColor = Color.FromArgb(108, 117, 125);
            lblNomorFile.AutoSize = true;
            lblNomorFile.Location = new Point(inputX, y + 3);

            var btnBrowse = new Button();
            btnBrowse.Text = "Browse...";
            btnBrowse.Location = new Point(inputX + inputW + 10, y);
            btnBrowse.Size = new Size(80, 26);
            btnBrowse.FlatStyle = FlatStyle.Flat;
            btnBrowse.FlatAppearance.BorderSize = 0;
            btnBrowse.BackColor = Color.FromArgb(23, 162, 184);
            btnBrowse.ForeColor = Color.White;
            btnBrowse.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            btnBrowse.Cursor = Cursors.Hand;
            btnBrowse.FlatAppearance.MouseOverBackColor = Color.FromArgb(18, 130, 150);
            btnBrowse.Click += (s, ev) =>
            {
                using (var ofd = new OpenFileDialog())
                {
                    ofd.Filter = "Word Document|*.docx";
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        selectedDocxPath = ofd.FileName;
                        lblNomorFile.Text = Path.GetFileName(selectedDocxPath);
                        lblNomorFile.ForeColor = DarkBase;
                    }
                }
            };
            y += 35;

            formPanel.Controls.Add(btnBrowse);
            formPanel.Controls.Add(lblNomorFile);
            formPanel.Controls.Add(lblFile);
            formPanel.Controls.Add(txtNomorPenerima);
            formPanel.Controls.Add(lblPenerima);
            formPanel.Controls.Add(txtNomorPerihal);
            formPanel.Controls.Add(lblPerihal);
            formPanel.Controls.Add(cmbNomorDept);
            formPanel.Controls.Add(lblDept);
            formPanel.Controls.Add(txtNomorKode);
            formPanel.Controls.Add(lblKode);

            // ── Preview + Generate row ──
            var previewPanel = new Panel();
            previewPanel.Dock = DockStyle.Top;
            previewPanel.Height = 70;
            previewPanel.Padding = new Padding(5);

            var lblPreviewTitle = new Label();
            lblPreviewTitle.Text = "Nomor Surat Preview:";
            lblPreviewTitle.Font = new Font("Segoe UI", 9F);
            lblPreviewTitle.ForeColor = Color.FromArgb(108, 117, 125);
            lblPreviewTitle.AutoSize = true;
            lblPreviewTitle.Location = new Point(5, 5);

            lblNomorPreview = new Label();
            lblNomorPreview.Text = "001/SK/HRD/VII/2026";
            lblNomorPreview.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblNomorPreview.ForeColor = AccentColor;
            lblNomorPreview.AutoSize = true;
            lblNomorPreview.Location = new Point(5, 28);

            var btnGenerate = new Button();
            btnGenerate.Text = "Generate & Print";
            btnGenerate.Location = new Point(400, 22);
            btnGenerate.Size = new Size(150, 35);
            btnGenerate.FlatStyle = FlatStyle.Flat;
            btnGenerate.FlatAppearance.BorderSize = 0;
            btnGenerate.BackColor = AccentColor;
            btnGenerate.ForeColor = Color.White;
            btnGenerate.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnGenerate.Cursor = Cursors.Hand;
            btnGenerate.FlatAppearance.MouseOverBackColor = PrimaryHover;
            btnGenerate.FlatAppearance.MouseDownBackColor = Color.FromArgb(90, 8, 38);
            btnGenerate.Click += (s, ev) => GenerateNomorSurat();

            previewPanel.Controls.Add(btnGenerate);
            previewPanel.Controls.Add(lblNomorPreview);
            previewPanel.Controls.Add(lblPreviewTitle);

            // Z-order: Fill last (no fill needed — fixed height form)
            NomorSuratPanel.Controls.Add(previewPanel);
            NomorSuratPanel.Controls.Add(formPanel);
            NomorSuratPanel.Controls.Add(lblTitle);

            ContentPanel.Controls.Add(NomorSuratPanel);
            NomorSuratPanel.BringToFront();
        }

        private Label MakeLabel(string text, int x, int y)
        {
            var lbl = new Label();
            lbl.Text = text;
            lbl.Font = new Font("Segoe UI", 9F);
            lbl.ForeColor = DarkBase;
            lbl.AutoSize = true;
            lbl.Location = new Point(x, y);
            return lbl;
        }

        private TextBox MakeInput(int x, int y, int w, string defaultVal)
        {
            var txt = new TextBox();
            txt.Location = new Point(x, y);
            txt.Size = new Size(w, 24);
            txt.Font = new Font("Segoe UI", 9F);
            txt.Text = defaultVal;
            return txt;
        }

        private void UpdateNomorPreview()
        {
            if (lblNomorPreview == null) return;
            string kode = txtNomorKode.Text.Trim();
            string dept = cmbNomorDept.SelectedItem == null ? "HRD" : cmbNomorDept.SelectedItem.ToString();
            if (string.IsNullOrEmpty(kode)) kode = "SK";

            try
            {
                int nextNum = nomorRepo.PeekNextNumber(DateTime.Today.Year, kode);
                lblNomorPreview.Text = NomorSuratRepository.FormatNomor(nextNum, kode, dept, DateTime.Today);
            }
            catch
            {
                lblNomorPreview.Text = "001/" + kode + "/" + dept + "/" + NomorSuratRepository.ToRoman(DateTime.Today.Month) + "/" + DateTime.Today.Year;
            }
        }

        private void GenerateNomorSurat()
        {
            // Validate inputs
            string kode = txtNomorKode.Text.Trim();
            string dept = cmbNomorDept.SelectedItem == null ? "" : cmbNomorDept.SelectedItem.ToString();
            string perihal = txtNomorPerihal.Text.Trim();
            string penerima = txtNomorPenerima.Text.Trim();

            if (string.IsNullOrEmpty(kode))
            {
                MessageBox.Show("Kode Surat wajib diisi", "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNomorKode.Focus();
                return;
            }
            if (string.IsNullOrEmpty(dept))
            {
                MessageBox.Show("Departemen wajib dipilih", "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbNomorDept.Focus();
                return;
            }
            if (string.IsNullOrEmpty(perihal))
            {
                MessageBox.Show("Perihal wajib diisi", "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNomorPerihal.Focus();
                return;
            }
            if (string.IsNullOrEmpty(penerima))
            {
                MessageBox.Show("Penerima wajib diisi", "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNomorPenerima.Focus();
                return;
            }
            if (string.IsNullOrEmpty(selectedDocxPath) || !File.Exists(selectedDocxPath))
            {
                MessageBox.Show("File DOCX belum dipilih atau tidak ditemukan", "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // 1. Increment counter
                int newNumber = nomorRepo.GetNextNumber(DateTime.Today.Year, kode);
                string nomorSurat = NomorSuratRepository.FormatNomor(newNumber, kode, dept, DateTime.Today);

                // 2. Copy docx to temp and replace placeholder
                string outputDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SuratKeluar");
                if (!Directory.Exists(outputDir)) Directory.CreateDirectory(outputDir);

                string safeName = nomorSurat.Replace("/", "-");
                string outputDocx = Path.Combine(outputDir, safeName + ".docx");
                File.Copy(selectedDocxPath, outputDocx, true);

                bool replaced = DocxHelper.ReplacePlaceholder(outputDocx, "{{NOMOR_SURAT}}", nomorSurat);
                if (!replaced)
                {
                    // Rollback counter
                    MessageBox.Show("Placeholder {{NOMOR_SURAT}} tidak ditemukan dalam dokumen DOCX.\nPastikan dokumen mengandung teks {{NOMOR_SURAT}}.",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    // Note: counter not rolled back — gap is acceptable per spec
                    File.Delete(outputDocx);
                    return;
                }

                // 3. Convert to PDF via LibreOffice (graceful fallback)
                string outputPdf = null;
                string sofficePath = FindSoffice();
                if (sofficePath != null)
                {
                    var psi = new ProcessStartInfo
                    {
                        FileName = sofficePath,
                        Arguments = string.Format("--headless --convert-to pdf --outdir \"{0}\" \"{1}\"", outputDir, outputDocx),
                        UseShellExecute = false,
                        CreateNoWindow = true,
                        RedirectStandardError = true
                    };
                    var proc = Process.Start(psi);
                    if (!proc.WaitForExit(30000))
                    {
                        proc.Kill();
                        MessageBox.Show("Konversi PDF timeout (30 detik). DOCX tetap tersimpan.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        outputPdf = Path.Combine(outputDir, safeName + ".pdf");
                        if (!File.Exists(outputPdf)) outputPdf = null;
                    }
                }
                else
                {
                    MessageBox.Show("LibreOffice tidak ditemukan. Konversi PDF dilewati.\nFile DOCX tetap tersimpan dan dapat dibuka manual.\n\nUntuk mengaktifkan PDF, install LibreOffice dari https://www.libreoffice.org",
                        "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                // 4. Open result file
                string openFile = outputPdf ?? outputDocx;
                if (File.Exists(openFile)) Process.Start(openFile);

                // 5. Insert to Arsip Surat
                var surat = new Surat
                {
                    NomorSurat = nomorSurat,
                    TanggalSurat = DateTime.Today,
                    JenisSurat = "Keluar",
                    Pengirim = "",
                    Penerima = penerima,
                    Perihal = perihal,
                    Status = "Diproses",
                    Keterangan = "Dibuat via Generator Nomor Surat"
                };
                repository.Insert(surat);

                // 6. Success
                MessageBox.Show(string.Format("Nomor surat berhasil dibuat:\n\n{0}\n\nFile: {1}\nData sudah otomatis masuk ke Arsip Surat.",
                    nomorSurat, Path.GetFileName(openFile)),
                    "Berhasil", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Reset inputs
                txtNomorPerihal.Text = "";
                txtNomorPenerima.Text = "";
                selectedDocxPath = null;
                lblNomorFile.Text = "(belum dipilih)";
                lblNomorFile.ForeColor = Color.FromArgb(108, 117, 125);
                UpdateNomorPreview();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string FindSoffice()
        {
            string[] paths = {
                @"C:\Program Files\LibreOffice\program\soffice.exe",
                @"C:\Program Files (x86)\LibreOffice\program\soffice.exe"
            };
            foreach (string p in paths)
            {
                if (File.Exists(p)) return p;
            }
            return null;
        }

        private void LoadDeptDropdown()
        {
            cmbNomorDept.Items.Clear();
            DataTable depts = deptRepo.GetActiveList();
            foreach (DataRow row in depts.Rows)
            {
                cmbNomorDept.Items.Add(row["nama"].ToString());
            }
            if (cmbNomorDept.Items.Count > 0)
                cmbNomorDept.SelectedIndex = 0;
            else
                MessageBox.Show("Belum ada departemen aktif.\nSilakan isi Master Data terlebih dahulu.",
                    "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void InitializeMasterData()
        {
            MasterDataPanel = new Panel();
            MasterDataPanel.Dock = DockStyle.Fill;
            MasterDataPanel.BackColor = Color.FromArgb(255, 250, 245);
            MasterDataPanel.BorderStyle = BorderStyle.FixedSingle;
            MasterDataPanel.Padding = new Padding(15);
            MasterDataPanel.Visible = false;

            // ── Toolbar ──
            var toolbar = new FlowLayoutPanel();
            toolbar.Dock = DockStyle.Top;
            toolbar.AutoSize = true;
            toolbar.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            toolbar.FlowDirection = FlowDirection.LeftToRight;
            toolbar.WrapContents = false;
            toolbar.Padding = new Padding(0, 5, 0, 5);
            toolbar.Margin = new Padding(0);

            txtMasterSearch = new TextBox();
            txtMasterSearch.Size = new Size(200, 25);
            txtMasterSearch.Font = new Font("Segoe UI", 9F);
            txtMasterSearch.Margin = new Padding(0, 0, 10, 0);
            txtMasterSearch.TextChanged += (s, ev) => LoadMasterDeptData(txtMasterSearch.Text);

            var btnNew = MakeToolbarButton("New", AccentColor, PrimaryHover);
            btnNew.Click += (s, ev) => ShowDeptEditDialog(null);

            var btnEdit = MakeToolbarButton("Edit", SecondaryColor, SecondaryHover);
            btnEdit.ForeColor = DarkBase;
            btnEdit.Click += (s, ev) =>
            {
                if (dgvMasterDept.CurrentRow == null) return;
                int id = (int)dgvMasterDept.CurrentRow.Cells["id"].Value;
                ShowDeptEditDialog(id);
            };

            var btnDelete = MakeToolbarButton("Nonaktifkan", DangerColor, DangerHover);
            btnDelete.Click += (s, ev) =>
            {
                if (dgvMasterDept.CurrentRow == null) return;
                int id = (int)dgvMasterDept.CurrentRow.Cells["id"].Value;
                string nama = dgvMasterDept.CurrentRow.Cells["Nama"].Value.ToString();
                if (MessageBox.Show("Nonaktifkan departemen \"" + nama + "\"?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    deptRepo.SetActive(id, false);
                    LoadMasterDeptData(txtMasterSearch.Text);
                }
            };

            var btnRefresh = MakeToolbarButton("Refresh", SecondaryColor, SecondaryHover);
            btnRefresh.ForeColor = DarkBase;
            btnRefresh.Click += (s, ev) => LoadMasterDeptData(txtMasterSearch.Text);

            toolbar.Controls.Add(txtMasterSearch);
            toolbar.Controls.Add(btnNew);
            toolbar.Controls.Add(btnEdit);
            toolbar.Controls.Add(btnDelete);
            toolbar.Controls.Add(btnRefresh);

            // ── DataGridView ──
            dgvMasterDept = new DataGridView();
            dgvMasterDept.Dock = DockStyle.Fill;
            dgvMasterDept.ReadOnly = true;
            dgvMasterDept.AllowUserToAddRows = false;
            dgvMasterDept.AllowUserToDeleteRows = false;
            dgvMasterDept.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvMasterDept.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvMasterDept.BackgroundColor = Color.White;
            dgvMasterDept.BorderStyle = BorderStyle.None;
            dgvMasterDept.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvMasterDept.GridColor = GridLine;
            dgvMasterDept.EnableHeadersVisualStyles = false;
            dgvMasterDept.ColumnHeadersDefaultCellStyle.BackColor = HeaderBg;
            dgvMasterDept.ColumnHeadersDefaultCellStyle.ForeColor = HeaderFg;
            dgvMasterDept.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            dgvMasterDept.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvMasterDept.ColumnHeadersDefaultCellStyle.Padding = new Padding(5);
            dgvMasterDept.ColumnHeadersHeight = 35;
            dgvMasterDept.RowTemplate.Height = 30;
            dgvMasterDept.RowTemplate.DefaultCellStyle.Font = new Font("Segoe UI", 9F);
            dgvMasterDept.RowTemplate.DefaultCellStyle.Padding = new Padding(5);
            dgvMasterDept.AlternatingRowsDefaultCellStyle.BackColor = RowAlt;
            dgvMasterDept.DefaultCellStyle.SelectionBackColor = SecondaryColor;
            dgvMasterDept.DefaultCellStyle.SelectionForeColor = DarkBase;
            dgvMasterDept.RowHeadersVisible = true;
            dgvMasterDept.RowHeadersWidth = 35;

            // Z-order
            MasterDataPanel.Controls.Add(dgvMasterDept);
            MasterDataPanel.Controls.Add(toolbar);

            ContentPanel.Controls.Add(MasterDataPanel);
            MasterDataPanel.BringToFront();
        }

        private Button MakeToolbarButton(string text, Color bg, Color hover)
        {
            var btn = new Button();
            btn.Text = text;
            btn.AutoSize = true;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.BackColor = bg;
            btn.ForeColor = Color.White;
            btn.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            btn.Cursor = Cursors.Hand;
            btn.FlatAppearance.MouseOverBackColor = hover;
            btn.Margin = new Padding(0, 0, 6, 0);
            return btn;
        }

        private void LoadMasterDeptData(string search = "")
        {
            DataTable dt;
            if (string.IsNullOrEmpty(search))
                dt = deptRepo.GetAll();
            else
                dt = deptRepo.Search(search);

            dgvMasterDept.DataSource = dt;

            if (dgvMasterDept.Columns.Contains("id"))
            {
                dgvMasterDept.Columns["id"].Visible = false;
            }
            if (dgvMasterDept.Columns.Contains("Nama"))
            {
                dgvMasterDept.Columns["Nama"].FillWeight = 30;
                dgvMasterDept.Columns["Nama"].MinimumWidth = 120;
            }
            if (dgvMasterDept.Columns.Contains("Keterangan"))
            {
                dgvMasterDept.Columns["Keterangan"].FillWeight = 40;
                dgvMasterDept.Columns["Keterangan"].MinimumWidth = 150;
            }
            if (dgvMasterDept.Columns.Contains("Aktif"))
            {
                dgvMasterDept.Columns["Aktif"].FillWeight = 10;
                dgvMasterDept.Columns["Aktif"].MinimumWidth = 50;
            }
            if (dgvMasterDept.Columns.Contains("Dibuat"))
            {
                dgvMasterDept.Columns["Dibuat"].FillWeight = 20;
                dgvMasterDept.Columns["Dibuat"].MinimumWidth = 100;
            }

            foreach (DataGridViewColumn col in dgvMasterDept.Columns)
                if (col.Visible) col.HeaderText = col.HeaderText.ToUpper();

            // Color active/inactive rows
            foreach (DataGridViewRow row in dgvMasterDept.Rows)
            {
                object aktifVal = row.Cells["Aktif"].Value;
                bool aktif = aktifVal != null && aktifVal != DBNull.Value && Convert.ToBoolean(aktifVal);
                if (!aktif)
                {
                    row.DefaultCellStyle.ForeColor = Color.FromArgb(150, 150, 150);
                    row.DefaultCellStyle.BackColor = Color.FromArgb(245, 245, 245);
                }
            }
        }

        private void ShowDeptEditDialog(int? editId)
        {
            string currentNama = "";
            string currentKeterangan = "";
            bool currentActive = true;

            if (editId.HasValue)
            {
                // Load existing data from grid row
                foreach (DataGridViewRow row in dgvMasterDept.Rows)
                {
                    if (row.Cells["id"].Value != null && (int)row.Cells["id"].Value == editId.Value)
                    {
                        currentNama = row.Cells["Nama"].Value.ToString();
                        currentKeterangan = row.Cells["Keterangan"].Value == DBNull.Value ? "" : row.Cells["Keterangan"].Value.ToString();
                        object aktifVal = row.Cells["Aktif"].Value;
                        currentActive = aktifVal != null && aktifVal != DBNull.Value && Convert.ToBoolean(aktifVal);
                        break;
                    }
                }
            }

            // Build dialog form
            var form = new Form();
            form.Text = editId.HasValue ? "Edit Departemen" : "Tambah Departemen";
            form.Size = new Size(400, 230);
            form.StartPosition = FormStartPosition.CenterParent;
            form.FormBorderStyle = FormBorderStyle.FixedDialog;
            form.MaximizeBox = false;
            form.MinimizeBox = false;
            form.BackColor = Color.FromArgb(255, 250, 245);
            form.Font = new Font("Segoe UI", 9F);

            var lblNama = new Label { Text = "Nama:", AutoSize = true, Location = new Point(15, 20), ForeColor = DarkBase };
            var txtNama = new TextBox { Location = new Point(120, 17), Size = new Size(240, 24), Text = currentNama };

            var lblKet = new Label { Text = "Keterangan:", AutoSize = true, Location = new Point(15, 55), ForeColor = DarkBase };
            var txtKet = new TextBox { Location = new Point(120, 52), Size = new Size(240, 24), Text = currentKeterangan };

            var chkActive = new CheckBox { Text = "Aktif", Location = new Point(120, 87), Checked = currentActive };

            var btnSave = new Button { Text = "Simpan", Location = new Point(120, 125), Size = new Size(90, 30) };
            btnSave.FlatStyle = FlatStyle.Flat;
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.BackColor = AccentColor;
            btnSave.ForeColor = Color.White;
            btnSave.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnSave.Cursor = Cursors.Hand;
            btnSave.FlatAppearance.MouseOverBackColor = PrimaryHover;

            var btnCancel = new Button { Text = "Batal", Location = new Point(220, 125), Size = new Size(90, 30) };
            btnCancel.FlatStyle = FlatStyle.Flat;
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.BackColor = SecondaryColor;
            btnCancel.ForeColor = DarkBase;
            btnCancel.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnCancel.Cursor = Cursors.Hand;
            btnCancel.FlatAppearance.MouseOverBackColor = SecondaryHover;
            btnCancel.Click += (s, ev) => form.DialogResult = DialogResult.Cancel;

            btnSave.Click += (s, ev) =>
            {
                string nama = txtNama.Text.Trim();
                string keterangan = txtKet.Text.Trim();
                bool active = chkActive.Checked;

                if (string.IsNullOrEmpty(nama))
                {
                    MessageBox.Show("Nama wajib diisi", "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtNama.Focus();
                    return;
                }

                int excludeId = editId.HasValue ? editId.Value : 0;
                if (deptRepo.IsNamaExists(nama, excludeId))
                {
                    MessageBox.Show("Nama departemen \"" + nama + "\" sudah ada", "Duplikat", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtNama.Focus();
                    return;
                }

                try
                {
                    if (editId.HasValue)
                        deptRepo.Update(editId.Value, nama, keterangan, active);
                    else
                        deptRepo.Insert(nama, keterangan);

                    form.DialogResult = DialogResult.OK;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };

            form.Controls.AddRange(new Control[] { lblNama, txtNama, lblKet, txtKet, chkActive, btnSave, btnCancel });
            form.AcceptButton = btnSave;
            form.CancelButton = btnCancel;

            if (form.ShowDialog() == DialogResult.OK)
                LoadMasterDeptData(txtMasterSearch.Text);
        }

        private void ApplySearchPlaceholder()
        {
            SendMessage(txtSearch.Handle, EM_SETCUEBANNER, (IntPtr)1, "Cari nomor surat...");
        }

        private void StyleButton(Button btn, Color bg, Color hover)
        {
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.BackColor = bg;
            btn.ForeColor = Color.White;
            btn.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
            btn.Cursor = Cursors.Hand;
            btn.FlatAppearance.MouseOverBackColor = hover;
            int r = Math.Max(0, hover.R - 20);
            int g = Math.Max(0, hover.G - 20);
            int b = Math.Max(0, hover.B - 20);
            btn.FlatAppearance.MouseDownBackColor = Color.FromArgb(r, g, b);
        }

        private void LoadData(string search = "")
        {
            if (!dgvSurat.Created) return;
            bool showDeleted = chkShowDeleted.Checked;
            DateTime? dateFrom = dtpFilterFrom.Checked ? (DateTime?)dtpFilterFrom.Value.Date : null;
            DateTime? dateTo = dtpFilterTo.Checked ? (DateTime?)dtpFilterTo.Value.Date : null;
            string jenis = cmbFilterJenis.SelectedIndex > 0 ? cmbFilterJenis.SelectedItem.ToString() : null;
            string status = cmbFilterStatus.SelectedIndex > 0 ? cmbFilterStatus.SelectedItem.ToString() : null;
            
            DataTable dt = repository.GetAll(search, showDeleted, dateFrom, dateTo, jenis, status);
            dgvSurat.DataSource = null;
            dgvSurat.DataSource = dt;
            if (dgvSurat.Columns.Contains("id"))
            {
                dgvSurat.Columns["id"].Visible = false;
                dgvSurat.Columns["id"].FillWeight = 1;
            }
            if (dgvSurat.Columns.Contains("Created"))
            {
                dgvSurat.Columns["Created"].Visible = false;
                dgvSurat.Columns["Created"].FillWeight = 1;
            }
            if (dgvSurat.Columns.Contains("Nomor Surat"))
            {
                dgvSurat.Columns["Nomor Surat"].FillWeight = 280;
                dgvSurat.Columns["Nomor Surat"].MinimumWidth = 120;
            }
            if (dgvSurat.Columns.Contains("Tanggal"))
            {
                dgvSurat.Columns["Tanggal"].FillWeight = 160;
                dgvSurat.Columns["Tanggal"].MinimumWidth = 100;
            }
            if (dgvSurat.Columns.Contains("Jenis"))
            {
                dgvSurat.Columns["Jenis"].FillWeight = 130;
                dgvSurat.Columns["Jenis"].MinimumWidth = 80;
            }
            if (dgvSurat.Columns.Contains("Pengirim/Penerima"))
            {
                dgvSurat.Columns["Pengirim/Penerima"].FillWeight = 280;
                dgvSurat.Columns["Pengirim/Penerima"].MinimumWidth = 120;
            }
            if (dgvSurat.Columns.Contains("Perihal"))
            {
                dgvSurat.Columns["Perihal"].FillWeight = 600;
                dgvSurat.Columns["Perihal"].MinimumWidth = 150;
            }
            if (dgvSurat.Columns.Contains("Status"))
            {
                dgvSurat.Columns["Status"].FillWeight = 130;
                dgvSurat.Columns["Status"].MinimumWidth = 90;
            }
            // Uppercase all visible column headers
            foreach (DataGridViewColumn col in dgvSurat.Columns)
            {
                if (col.Visible)
                {
                    col.HeaderText = col.HeaderText.ToUpper();
                }
            }
            ApplyStatusColors();
            UpdateStatusBar(dt.Rows.Count);
        }

        private void UpdateStatusBar(int count)
        {
            lblStatus.Text = string.Format("Total: {0} surat", count);
        }

        private void dgvSurat_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            string num = (e.RowIndex + 1).ToString();
            using (var brush = new SolidBrush(AccentColor))
            {
                var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
                e.Graphics.DrawString(num, new Font("Segoe UI", 8F), brush,
                    new RectangleF(e.RowBounds.Location.X, e.RowBounds.Location.Y, dgvSurat.RowHeadersWidth, e.RowBounds.Height),
                    sf);
            }
        }

        private void ApplyStatusColors()
        {
            bool showDeleted = chkShowDeleted.Checked;
            foreach (DataGridViewRow row in dgvSurat.Rows)
            {
                string status = row.Cells["Status"].Value == null ? "" : row.Cells["Status"].Value.ToString();
                if (showDeleted)
                {
                    row.DefaultCellStyle.BackColor = BgColor;
                    row.DefaultCellStyle.ForeColor = Color.FromArgb(140, 107, 91);
                }
                else
                {
                    switch (status)
                    {
                        case "Draft":
                            row.DefaultCellStyle.BackColor = Color.FromArgb(245, 237, 228);
                            row.DefaultCellStyle.ForeColor = Color.FromArgb(107, 91, 78);
                            break;
                        case "Diterima":
                            row.DefaultCellStyle.BackColor = BgColor;
                            row.DefaultCellStyle.ForeColor = DarkBase;
                            break;
                        case "Diproses":
                            row.DefaultCellStyle.BackColor = Color.FromArgb(232, 213, 192);
                            row.DefaultCellStyle.ForeColor = Color.FromArgb(107, 69, 32);
                            break;
                        case "Selesai":
                            row.DefaultCellStyle.BackColor = BgColor;
                            row.DefaultCellStyle.ForeColor = Color.FromArgb(61, 19, 19);
                            break;
                    }
                }
            }
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            using (var form = new SuratInputForm())
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    LoadData();
                }
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvSurat.CurrentRow == null) return;
            int id = (int)dgvSurat.CurrentRow.Cells["id"].Value;
            using (var form = new SuratInputForm(id))
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    LoadData();
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvSurat.CurrentRow == null) return;
            int id = (int)dgvSurat.CurrentRow.Cells["id"].Value;
            string nomor = dgvSurat.CurrentRow.Cells["Nomor Surat"].Value.ToString();
            if (MessageBox.Show(string.Format("Yakin hapus surat {0}?", nomor), "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                repository.SoftDelete(id);
                LoadData();
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)dgvSurat.DataSource;
            if (dt == null || dt.Rows.Count == 0)
            {
                MessageBox.Show("Tidak ada data untuk di-export", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            ReportHelper.ShowExportDialog(dt, "ArsipSurat");
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (dgvSurat.Rows.Count == 0)
            {
                MessageBox.Show("Tidak ada data untuk di-print", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            ReportHelper.PrintDataGridView(dgvSurat, "Laporan Arsip Surat");
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            searchTimer.Stop();
            searchTimer.Start();
        }

        private void SearchTimer_Tick(object sender, EventArgs e)
        {
            searchTimer.Stop();
            LoadData(txtSearch.Text);
        }

        private void chkShowDeleted_CheckedChanged(object sender, EventArgs e)
        {
            bool showDeleted = chkShowDeleted.Checked;
            btnNew.Visible = !showDeleted;
            btnEdit.Visible = !showDeleted;
            btnDelete.Visible = !showDeleted;
            btnRestore.Visible = showDeleted;
            btnPermanentDelete.Visible = showDeleted;
            LoadData(txtSearch.Text);
        }

        private void btnResetFilter_Click(object sender, EventArgs e)
        {
            dtpFilterFrom.Checked = false;
            dtpFilterTo.Checked = false;
            cmbFilterJenis.SelectedIndex = 0;
            cmbFilterStatus.SelectedIndex = 0;
            txtSearch.Text = "";
            LoadData();
        }

        private void dtpFilterFrom_ValueChanged(object sender, EventArgs e)
        {
            LoadData(txtSearch.Text);
        }

        private void dtpFilterTo_ValueChanged(object sender, EventArgs e)
        {
            LoadData(txtSearch.Text);
        }

        private void cmbFilterJenis_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadData(txtSearch.Text);
        }

        private void cmbFilterStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadData(txtSearch.Text);
        }

        private void btnRestore_Click(object sender, EventArgs e)
        {
            if (dgvSurat.CurrentRow == null) return;
            int id = (int)dgvSurat.CurrentRow.Cells["id"].Value;
            string nomor = dgvSurat.CurrentRow.Cells["Nomor Surat"].Value.ToString();
            if (MessageBox.Show(string.Format("Restore surat {0}?", nomor), "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                repository.Restore(id);
                LoadData(txtSearch.Text);
            }
        }

        private void btnPermanentDelete_Click(object sender, EventArgs e)
        {
            if (dgvSurat.CurrentRow == null) return;
            int id = (int)dgvSurat.CurrentRow.Cells["id"].Value;
            string nomor = dgvSurat.CurrentRow.Cells["Nomor Surat"].Value.ToString();
            if (MessageBox.Show(string.Format("HAPUS PERMANEN surat {0}?\nTindakan ini tidak dapat dibatalkan!", nomor), "Peringatan", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                lampiranRepo.DeleteBySuratId(id);
                repository.PermanentDelete(id);
                LoadData(txtSearch.Text);
            }
        }

        private void dgvSurat_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            int id = (int)dgvSurat.Rows[e.RowIndex].Cells["id"].Value;
            bool showDeleted = chkShowDeleted.Checked;
            using (var form = new SuratDetailForm(id, showDeleted))
            {
                form.ShowDialog();
                LoadData();
            }
        }

    }
}
