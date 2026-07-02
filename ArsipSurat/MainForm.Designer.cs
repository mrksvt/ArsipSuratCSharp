namespace ArsipSurat
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            // ── Declare all controls ──
            // Original controls (reparented to CardPanel)
            this.dgvSurat = new System.Windows.Forms.DataGridView();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.btnNew = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.chkShowDeleted = new System.Windows.Forms.CheckBox();
            this.btnRestore = new System.Windows.Forms.Button();
            this.btnPermanentDelete = new System.Windows.Forms.Button();
            this.btnExport = new System.Windows.Forms.Button();
            this.btnPrint = new System.Windows.Forms.Button();
            this.labelFilterFrom = new System.Windows.Forms.Label();
            this.dtpFilterFrom = new System.Windows.Forms.DateTimePicker();
            this.labelFilterTo = new System.Windows.Forms.Label();
            this.dtpFilterTo = new System.Windows.Forms.DateTimePicker();
            this.labelFilterJenis = new System.Windows.Forms.Label();
            this.cmbFilterJenis = new System.Windows.Forms.ComboBox();
            this.labelFilterStatus = new System.Windows.Forms.Label();
            this.cmbFilterStatus = new System.Windows.Forms.ComboBox();
            this.btnResetFilter = new System.Windows.Forms.Button();
            this.lblStatus = new System.Windows.Forms.Label();
            this.toolbarPanel = new System.Windows.Forms.FlowLayoutPanel();

            // New layout panels
            this.NavbarPanel = new System.Windows.Forms.Panel();
            this.SidebarPanel = new System.Windows.Forms.Panel();
            this.ContentPanel = new System.Windows.Forms.Panel();
            this.CardPanel = new System.Windows.Forms.Panel();
            this.filterPanel = new System.Windows.Forms.Panel();
            this.BreadcrumbPanel = new System.Windows.Forms.Panel();
            this.FooterPanel = new System.Windows.Forms.Panel();

            // Navbar controls
            this.picLogo = new System.Windows.Forms.PictureBox();
            this.lblNavTitle = new System.Windows.Forms.Label();
            this.lblNavClock = new System.Windows.Forms.Label();
            this.lblNavUser = new System.Windows.Forms.Label();
            this.lblNavLogout = new System.Windows.Forms.Label();

            // Sidebar controls
            this.lblSidebarTitle = new System.Windows.Forms.Label();
            this.lblSidebarSubtitle = new System.Windows.Forms.Label();
            this.pnlMenuDashboard = new System.Windows.Forms.Panel();
            this.lblMenuDashboard = new System.Windows.Forms.Label();
            this.pnlMenuArsip = new System.Windows.Forms.Panel();
            this.lblMenuArsip = new System.Windows.Forms.Label();
            this.pnlMenuLaporan = new System.Windows.Forms.Panel();
            this.lblMenuLaporan = new System.Windows.Forms.Label();
            this.pnlMenuMasterData = new System.Windows.Forms.Panel();
            this.lblMenuMasterData = new System.Windows.Forms.Label();
            this.pnlMenuPengaturan = new System.Windows.Forms.Panel();
            this.lblMenuPengaturan = new System.Windows.Forms.Label();
            this.pnlMenuLogout = new System.Windows.Forms.Panel();
            this.lblMenuLogout = new System.Windows.Forms.Label();

            // Breadcrumb controls
            this.lblBreadcrumbHome = new System.Windows.Forms.Label();
            this.lblBreadcrumbPage = new System.Windows.Forms.Label();

            // Footer controls
            this.lblFooterLeft = new System.Windows.Forms.Label();
            this.lblFooterRight = new System.Windows.Forms.Label();

            ((System.ComponentModel.ISupportInitialize)(this.dgvSurat)).BeginInit();
            this.SuspendLayout();

            // ================================================================
            // CardPanel — contains all original controls
            // ================================================================
            this.CardPanel.BackColor = System.Drawing.Color.FromArgb(255, 250, 245);
            this.CardPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.CardPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CardPanel.Name = "CardPanel";
            this.CardPanel.Padding = new System.Windows.Forms.Padding(10);
            this.CardPanel.TabIndex = 30;
            //
            // filterPanel — docked panel for filter row
            //
            this.filterPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.filterPanel.Name = "filterPanel";
            this.filterPanel.Size = new System.Drawing.Size(760, 30);
            this.filterPanel.TabIndex = 33;
            //
            // dgvSurat
            //
            this.dgvSurat.AllowUserToAddRows = false;
            this.dgvSurat.AllowUserToDeleteRows = false;
            this.dgvSurat.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvSurat.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSurat.Name = "dgvSurat";
            this.dgvSurat.ReadOnly = true;
            this.dgvSurat.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvSurat.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvSurat.TabIndex = 0;
            this.dgvSurat.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvSurat_CellDoubleClick);
            this.dgvSurat.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.dgvSurat_RowPostPaint);
            // 
            // txtSearch
            // 
            this.txtSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)));
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(220, 25);
            this.txtSearch.TabIndex = 1;
            this.txtSearch.Margin = new System.Windows.Forms.Padding(0, 0, 10, 0);
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
            // 
            // toolbarPanel
            // 
            this.toolbarPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.toolbarPanel.Name = "toolbarPanel";
            this.toolbarPanel.Size = new System.Drawing.Size(784, 33);
            this.toolbarPanel.FlowDirection = System.Windows.Forms.FlowDirection.LeftToRight;
            this.toolbarPanel.WrapContents = false;
            this.toolbarPanel.AutoSize = true;
            this.toolbarPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.toolbarPanel.Padding = new System.Windows.Forms.Padding(0, 5, 10, 5);
            this.toolbarPanel.Margin = new System.Windows.Forms.Padding(0);
            // 
            // btnNew
            // 
            this.btnNew.AutoSize = true;
            this.btnNew.Name = "btnNew";
            this.btnNew.TabIndex = 2;
            this.btnNew.Text = "New";
            this.btnNew.UseVisualStyleBackColor = true;
            this.btnNew.Margin = new System.Windows.Forms.Padding(0, 0, 6, 0);
            this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.AutoSize = true;
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.TabIndex = 3;
            this.btnEdit.Text = "Edit";
            this.btnEdit.UseVisualStyleBackColor = true;
            this.btnEdit.Margin = new System.Windows.Forms.Padding(0, 0, 6, 0);
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.AutoSize = true;
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.TabIndex = 4;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Margin = new System.Windows.Forms.Padding(0, 0, 6, 0);
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.AutoSize = true;
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.TabIndex = 5;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Margin = new System.Windows.Forms.Padding(12, 0, 6, 0);
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // chkShowDeleted
            // 
            this.chkShowDeleted.AutoSize = true;
            this.chkShowDeleted.Name = "chkShowDeleted";
            this.chkShowDeleted.TabIndex = 6;
            this.chkShowDeleted.Text = "Tampilkan Dihapus";
            this.chkShowDeleted.UseVisualStyleBackColor = true;
            this.chkShowDeleted.Margin = new System.Windows.Forms.Padding(12, 0, 0, 0);
            this.chkShowDeleted.CheckedChanged += new System.EventHandler(this.chkShowDeleted_CheckedChanged);
            // 
            // btnRestore
            // 
            this.btnRestore.AutoSize = true;
            this.btnRestore.Name = "btnRestore";
            this.btnRestore.TabIndex = 7;
            this.btnRestore.Text = "Restore";
            this.btnRestore.UseVisualStyleBackColor = true;
            this.btnRestore.Visible = false;
            this.btnRestore.Margin = new System.Windows.Forms.Padding(0, 0, 6, 0);
            this.btnRestore.Click += new System.EventHandler(this.btnRestore_Click);
            // 
            // btnPermanentDelete
            // 
            this.btnPermanentDelete.AutoSize = true;
            this.btnPermanentDelete.Name = "btnPermanentDelete";
            this.btnPermanentDelete.TabIndex = 8;
            this.btnPermanentDelete.Text = "Hapus Permanen";
            this.btnPermanentDelete.UseVisualStyleBackColor = true;
            this.btnPermanentDelete.Visible = false;
            this.btnPermanentDelete.Margin = new System.Windows.Forms.Padding(0, 0, 6, 0);
            this.btnPermanentDelete.Click += new System.EventHandler(this.btnPermanentDelete_Click);
            // 
            // btnExport
            // 
            this.btnExport.AutoSize = true;
            this.btnExport.Name = "btnExport";
            this.btnExport.TabIndex = 18;
            this.btnExport.Text = "Export";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Margin = new System.Windows.Forms.Padding(12, 0, 6, 0);
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // btnPrint
            // 
            this.btnPrint.AutoSize = true;
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.TabIndex = 19;
            this.btnPrint.Text = "Print";
            this.btnPrint.UseVisualStyleBackColor = true;
            this.btnPrint.Margin = new System.Windows.Forms.Padding(0, 0, 6, 0);
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            //
            // labelFilterFrom
            //
            this.labelFilterFrom.AutoSize = true;
            this.labelFilterFrom.Location = new System.Drawing.Point(2, 8);
            this.labelFilterFrom.Name = "labelFilterFrom";
            this.labelFilterFrom.Size = new System.Drawing.Size(22, 13);
            this.labelFilterFrom.TabIndex = 9;
            this.labelFilterFrom.Text = "Dari:";
            //
            // dtpFilterFrom
            //
            this.dtpFilterFrom.Checked = false;
            this.dtpFilterFrom.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFilterFrom.Location = new System.Drawing.Point(40, 4);
            this.dtpFilterFrom.Name = "dtpFilterFrom";
            this.dtpFilterFrom.ShowCheckBox = true;
            this.dtpFilterFrom.Size = new System.Drawing.Size(110, 20);
            this.dtpFilterFrom.TabIndex = 10;
            this.dtpFilterFrom.ValueChanged += new System.EventHandler(this.dtpFilterFrom_ValueChanged);
            //
            // labelFilterTo
            //
            this.labelFilterTo.AutoSize = true;
            this.labelFilterTo.Location = new System.Drawing.Point(158, 8);
            this.labelFilterTo.Name = "labelFilterTo";
            this.labelFilterTo.Size = new System.Drawing.Size(22, 13);
            this.labelFilterTo.TabIndex = 11;
            this.labelFilterTo.Text = "Sampai:";
            //
            // dtpFilterTo
            //
            this.dtpFilterTo.Checked = false;
            this.dtpFilterTo.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFilterTo.Location = new System.Drawing.Point(210, 4);
            this.dtpFilterTo.Name = "dtpFilterTo";
            this.dtpFilterTo.ShowCheckBox = true;
            this.dtpFilterTo.Size = new System.Drawing.Size(110, 20);
            this.dtpFilterTo.TabIndex = 12;
            this.dtpFilterTo.ValueChanged += new System.EventHandler(this.dtpFilterTo_ValueChanged);
            //
            // labelFilterJenis
            //
            this.labelFilterJenis.AutoSize = true;
            this.labelFilterJenis.Location = new System.Drawing.Point(330, 8);
            this.labelFilterJenis.Name = "labelFilterJenis";
            this.labelFilterJenis.Size = new System.Drawing.Size(33, 13);
            this.labelFilterJenis.TabIndex = 13;
            this.labelFilterJenis.Text = "Jenis:";
            //
            // cmbFilterJenis
            //
            this.cmbFilterJenis.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFilterJenis.FormattingEnabled = true;
            this.cmbFilterJenis.Items.AddRange(new object[] { "Semua", "Masuk", "Keluar" });
            this.cmbFilterJenis.Location = new System.Drawing.Point(365, 4);
            this.cmbFilterJenis.Name = "cmbFilterJenis";
            this.cmbFilterJenis.Size = new System.Drawing.Size(80, 21);
            this.cmbFilterJenis.TabIndex = 14;
            this.cmbFilterJenis.SelectedIndexChanged += new System.EventHandler(this.cmbFilterJenis_SelectedIndexChanged);
            //
            // labelFilterStatus
            //
            this.labelFilterStatus.AutoSize = true;
            this.labelFilterStatus.Location = new System.Drawing.Point(455, 8);
            this.labelFilterStatus.Name = "labelFilterStatus";
            this.labelFilterStatus.Size = new System.Drawing.Size(40, 13);
            this.labelFilterStatus.TabIndex = 15;
            this.labelFilterStatus.Text = "Status:";
            //
            // cmbFilterStatus
            //
            this.cmbFilterStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFilterStatus.FormattingEnabled = true;
            this.cmbFilterStatus.Items.AddRange(new object[] { "Semua", "Draft", "Diterima", "Diproses", "Selesai" });
            this.cmbFilterStatus.Location = new System.Drawing.Point(500, 4);
            this.cmbFilterStatus.Name = "cmbFilterStatus";
            this.cmbFilterStatus.Size = new System.Drawing.Size(80, 21);
            this.cmbFilterStatus.TabIndex = 16;
            this.cmbFilterStatus.SelectedIndexChanged += new System.EventHandler(this.cmbFilterStatus_SelectedIndexChanged);
            //
            // btnResetFilter
            //
            this.btnResetFilter.Location = new System.Drawing.Point(590, 4);
            this.btnResetFilter.Name = "btnResetFilter";
            this.btnResetFilter.Size = new System.Drawing.Size(50, 21);
            this.btnResetFilter.TabIndex = 17;
            this.btnResetFilter.Text = "Reset";
            this.btnResetFilter.UseVisualStyleBackColor = true;
            this.btnResetFilter.Click += new System.EventHandler(this.btnResetFilter_Click);
            //
            // lblStatus
            //
            this.lblStatus.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblStatus.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblStatus.ForeColor = System.Drawing.Color.FromArgb(84, 26, 26);
            this.lblStatus.Padding = new System.Windows.Forms.Padding(2, 4, 0, 4);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(760, 23);
            this.lblStatus.TabIndex = 20;
            this.lblStatus.Text = "Total: 0 surat";

            this.toolbarPanel.Controls.Add(this.txtSearch);
            this.toolbarPanel.Controls.Add(this.btnNew);
            this.toolbarPanel.Controls.Add(this.btnEdit);
            this.toolbarPanel.Controls.Add(this.btnDelete);
            this.toolbarPanel.Controls.Add(this.btnRefresh);
            this.toolbarPanel.Controls.Add(this.chkShowDeleted);
            this.toolbarPanel.Controls.Add(this.btnRestore);
            this.toolbarPanel.Controls.Add(this.btnPermanentDelete);
            this.toolbarPanel.Controls.Add(this.btnExport);
            this.toolbarPanel.Controls.Add(this.btnPrint);

            // Filter controls → filterPanel (docked Top inside CardPanel)
            this.filterPanel.Controls.Add(this.btnResetFilter);
            this.filterPanel.Controls.Add(this.cmbFilterStatus);
            this.filterPanel.Controls.Add(this.labelFilterStatus);
            this.filterPanel.Controls.Add(this.cmbFilterJenis);
            this.filterPanel.Controls.Add(this.labelFilterJenis);
            this.filterPanel.Controls.Add(this.dtpFilterTo);
            this.filterPanel.Controls.Add(this.labelFilterTo);
            this.filterPanel.Controls.Add(this.dtpFilterFrom);
            this.filterPanel.Controls.Add(this.labelFilterFrom);

            // CardPanel children — Z-order matters for docking:
            // Highest index → docked first. Order: Bottom → Top → Top → Fill
            this.CardPanel.Controls.Add(this.dgvSurat);       // index 0 → Dock=Fill (processed last)
            this.CardPanel.Controls.Add(this.filterPanel);    // index 1 → Dock=Top
            this.CardPanel.Controls.Add(this.toolbarPanel);   // index 2 → Dock=Top
            this.CardPanel.Controls.Add(this.lblStatus);      // index 3 → Dock=Bottom (processed first)

            // ================================================================
            // BreadcrumbPanel — page title above card
            // ================================================================
            this.BreadcrumbPanel.BackColor = System.Drawing.Color.Transparent;
            this.BreadcrumbPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.BreadcrumbPanel.Size = new System.Drawing.Size(784, 40);
            this.BreadcrumbPanel.Name = "BreadcrumbPanel";
            this.BreadcrumbPanel.TabIndex = 31;

            //
            // lblBreadcrumbHome
            //
            this.lblBreadcrumbHome.AutoSize = true;
            this.lblBreadcrumbHome.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblBreadcrumbHome.ForeColor = System.Drawing.Color.FromArgb(129, 11, 56);
            this.lblBreadcrumbHome.Location = new System.Drawing.Point(2, 12);
            this.lblBreadcrumbHome.Name = "lblBreadcrumbHome";
            this.lblBreadcrumbHome.Size = new System.Drawing.Size(25, 15);
            this.lblBreadcrumbHome.Text = "\U0001F3E0";
            //
            // lblBreadcrumbPage
            //
            this.lblBreadcrumbPage.AutoSize = true;
            this.lblBreadcrumbPage.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblBreadcrumbPage.ForeColor = System.Drawing.Color.FromArgb(84, 26, 26);
            this.lblBreadcrumbPage.Location = new System.Drawing.Point(28, 12);
            this.lblBreadcrumbPage.Name = "lblBreadcrumbPage";
            this.lblBreadcrumbPage.Size = new System.Drawing.Size(180, 15);
            this.lblBreadcrumbPage.Text = "Arsip Surat > Daftar Surat";

            this.BreadcrumbPanel.Controls.Add(this.lblBreadcrumbPage);
            this.BreadcrumbPanel.Controls.Add(this.lblBreadcrumbHome);

            // ================================================================
            // ContentPanel — fill area with padding
            // ================================================================
            this.ContentPanel.BackColor = System.Drawing.Color.FromArgb(241, 226, 209);
            this.ContentPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ContentPanel.Name = "ContentPanel";
            this.ContentPanel.Padding = new System.Windows.Forms.Padding(15);
            this.ContentPanel.TabIndex = 28;

            // Add CardPanel first (fills), then BreadcrumbPanel on top
            this.ContentPanel.Controls.Add(this.CardPanel);
            this.ContentPanel.Controls.Add(this.BreadcrumbPanel);

            // ================================================================
            // SidebarPanel — left navigation
            // ================================================================
            this.SidebarPanel.BackColor = System.Drawing.Color.FromArgb(61, 19, 19);
            this.SidebarPanel.Dock = System.Windows.Forms.DockStyle.Left;
            this.SidebarPanel.Size = new System.Drawing.Size(240, 534);
            this.SidebarPanel.Name = "SidebarPanel";
            this.SidebarPanel.TabIndex = 27;

            //
            // lblSidebarTitle
            //
            this.lblSidebarTitle.AutoSize = true;
            this.lblSidebarTitle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblSidebarTitle.ForeColor = System.Drawing.Color.White;
            this.lblSidebarTitle.Location = new System.Drawing.Point(20, 20);
            this.lblSidebarTitle.Name = "lblSidebarTitle";
            this.lblSidebarTitle.Size = new System.Drawing.Size(110, 19);
            this.lblSidebarTitle.Text = "ARSIP SURAT";
            //
            // lblSidebarSubtitle
            //
            this.lblSidebarSubtitle.AutoSize = true;
            this.lblSidebarSubtitle.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.lblSidebarSubtitle.ForeColor = System.Drawing.Color.FromArgb(220, 195, 170);
            this.lblSidebarSubtitle.Location = new System.Drawing.Point(20, 44);
            this.lblSidebarSubtitle.Name = "lblSidebarSubtitle";
            this.lblSidebarSubtitle.Size = new System.Drawing.Size(130, 13);
            this.lblSidebarSubtitle.Text = "Sistem Manajemen Arsip";

            // ── Menu Items ──
            //
            // pnlMenuDashboard
            //
            this.pnlMenuDashboard.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlMenuDashboard.Size = new System.Drawing.Size(240, 40);
            this.pnlMenuDashboard.BackColor = System.Drawing.Color.FromArgb(61, 19, 19);
            this.pnlMenuDashboard.Name = "pnlMenuDashboard";
            this.pnlMenuDashboard.Padding = new System.Windows.Forms.Padding(20, 0, 0, 0);
            this.pnlMenuDashboard.Cursor = System.Windows.Forms.Cursors.Hand;
            //
            // lblMenuDashboard
            //
            this.lblMenuDashboard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblMenuDashboard.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblMenuDashboard.ForeColor = System.Drawing.Color.FromArgb(220, 195, 170);
            this.lblMenuDashboard.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblMenuDashboard.Name = "lblMenuDashboard";
            this.lblMenuDashboard.Text = "\U0001F4CA  Dashboard";
            this.lblMenuDashboard.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblMenuDashboard.Click += new System.EventHandler(this.lblMenuDashboard_Click);
            this.lblMenuDashboard.MouseEnter += new System.EventHandler(this.SidebarMenu_MouseEnter);
            this.lblMenuDashboard.MouseLeave += new System.EventHandler(this.SidebarMenu_MouseLeave);
            this.pnlMenuDashboard.Controls.Add(this.lblMenuDashboard);
            this.pnlMenuDashboard.Click += new System.EventHandler(this.lblMenuDashboard_Click);
            this.pnlMenuDashboard.MouseEnter += new System.EventHandler(this.SidebarMenu_MouseEnter);
            this.pnlMenuDashboard.MouseLeave += new System.EventHandler(this.SidebarMenu_MouseLeave);

            //
            // pnlMenuArsip
            //
            this.pnlMenuArsip.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlMenuArsip.Size = new System.Drawing.Size(240, 40);
            this.pnlMenuArsip.BackColor = System.Drawing.Color.FromArgb(61, 19, 19);
            this.pnlMenuArsip.Name = "pnlMenuArsip";
            this.pnlMenuArsip.Padding = new System.Windows.Forms.Padding(20, 0, 0, 0);
            this.pnlMenuArsip.Cursor = System.Windows.Forms.Cursors.Hand;
            //
            // lblMenuArsip
            //
            this.lblMenuArsip.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblMenuArsip.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblMenuArsip.ForeColor = System.Drawing.Color.FromArgb(220, 195, 170);
            this.lblMenuArsip.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblMenuArsip.Name = "lblMenuArsip";
            this.lblMenuArsip.Text = "\U0001F4C1  Arsip Surat";
            this.lblMenuArsip.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblMenuArsip.Click += new System.EventHandler(this.lblMenuArsip_Click);
            this.lblMenuArsip.MouseEnter += new System.EventHandler(this.SidebarMenu_MouseEnter);
            this.lblMenuArsip.MouseLeave += new System.EventHandler(this.SidebarMenu_MouseLeave);
            this.pnlMenuArsip.Controls.Add(this.lblMenuArsip);
            this.pnlMenuArsip.Click += new System.EventHandler(this.lblMenuArsip_Click);
            this.pnlMenuArsip.MouseEnter += new System.EventHandler(this.SidebarMenu_MouseEnter);
            this.pnlMenuArsip.MouseLeave += new System.EventHandler(this.SidebarMenu_MouseLeave);

            //
            // pnlMenuLaporan
            //
            this.pnlMenuLaporan.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlMenuLaporan.Size = new System.Drawing.Size(240, 40);
            this.pnlMenuLaporan.BackColor = System.Drawing.Color.FromArgb(61, 19, 19);
            this.pnlMenuLaporan.Name = "pnlMenuLaporan";
            this.pnlMenuLaporan.Padding = new System.Windows.Forms.Padding(20, 0, 0, 0);
            this.pnlMenuLaporan.Cursor = System.Windows.Forms.Cursors.Hand;
            //
            // lblMenuLaporan
            //
            this.lblMenuLaporan.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblMenuLaporan.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblMenuLaporan.ForeColor = System.Drawing.Color.FromArgb(220, 195, 170);
            this.lblMenuLaporan.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblMenuLaporan.Name = "lblMenuLaporan";
            this.lblMenuLaporan.Text = "\U0001F4DD  Nomor Surat";
            this.lblMenuLaporan.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblMenuLaporan.Click += new System.EventHandler(this.lblMenuLaporan_Click);
            this.lblMenuLaporan.MouseEnter += new System.EventHandler(this.SidebarMenu_MouseEnter);
            this.lblMenuLaporan.MouseLeave += new System.EventHandler(this.SidebarMenu_MouseLeave);
            this.pnlMenuLaporan.Controls.Add(this.lblMenuLaporan);
            this.pnlMenuLaporan.Click += new System.EventHandler(this.lblMenuLaporan_Click);
            this.pnlMenuLaporan.MouseEnter += new System.EventHandler(this.SidebarMenu_MouseEnter);
            this.pnlMenuLaporan.MouseLeave += new System.EventHandler(this.SidebarMenu_MouseLeave);

            //
            // pnlMenuMasterData
            //
            this.pnlMenuMasterData.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlMenuMasterData.Size = new System.Drawing.Size(240, 40);
            this.pnlMenuMasterData.BackColor = System.Drawing.Color.FromArgb(61, 19, 19);
            this.pnlMenuMasterData.Name = "pnlMenuMasterData";
            this.pnlMenuMasterData.Padding = new System.Windows.Forms.Padding(20, 0, 0, 0);
            this.pnlMenuMasterData.Cursor = System.Windows.Forms.Cursors.Hand;
            //
            // lblMenuMasterData
            //
            this.lblMenuMasterData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblMenuMasterData.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblMenuMasterData.ForeColor = System.Drawing.Color.FromArgb(220, 195, 170);
            this.lblMenuMasterData.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblMenuMasterData.Name = "lblMenuMasterData";
            this.lblMenuMasterData.Text = "\U0001F4CB  Master Data";
            this.lblMenuMasterData.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblMenuMasterData.Click += new System.EventHandler(this.lblMenuMasterData_Click);
            this.lblMenuMasterData.MouseEnter += new System.EventHandler(this.SidebarMenu_MouseEnter);
            this.lblMenuMasterData.MouseLeave += new System.EventHandler(this.SidebarMenu_MouseLeave);
            this.pnlMenuMasterData.Controls.Add(this.lblMenuMasterData);
            this.pnlMenuMasterData.Click += new System.EventHandler(this.lblMenuMasterData_Click);
            this.pnlMenuMasterData.MouseEnter += new System.EventHandler(this.SidebarMenu_MouseEnter);
            this.pnlMenuMasterData.MouseLeave += new System.EventHandler(this.SidebarMenu_MouseLeave);

            //
            // pnlMenuPengaturan
            //
            this.pnlMenuPengaturan.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlMenuPengaturan.Size = new System.Drawing.Size(240, 40);
            this.pnlMenuPengaturan.BackColor = System.Drawing.Color.FromArgb(61, 19, 19);
            this.pnlMenuPengaturan.Name = "pnlMenuPengaturan";
            this.pnlMenuPengaturan.Padding = new System.Windows.Forms.Padding(20, 0, 0, 0);
            this.pnlMenuPengaturan.Cursor = System.Windows.Forms.Cursors.Hand;
            //
            // lblMenuPengaturan
            //
            this.lblMenuPengaturan.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblMenuPengaturan.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblMenuPengaturan.ForeColor = System.Drawing.Color.FromArgb(220, 195, 170);
            this.lblMenuPengaturan.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblMenuPengaturan.Name = "lblMenuPengaturan";
            this.lblMenuPengaturan.Text = "\u2699  Pengaturan";
            this.lblMenuPengaturan.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblMenuPengaturan.Click += new System.EventHandler(this.lblMenuPengaturan_Click);
            this.lblMenuPengaturan.MouseEnter += new System.EventHandler(this.SidebarMenu_MouseEnter);
            this.lblMenuPengaturan.MouseLeave += new System.EventHandler(this.SidebarMenu_MouseLeave);
            this.pnlMenuPengaturan.Controls.Add(this.lblMenuPengaturan);
            this.pnlMenuPengaturan.Click += new System.EventHandler(this.lblMenuPengaturan_Click);
            this.pnlMenuPengaturan.MouseEnter += new System.EventHandler(this.SidebarMenu_MouseEnter);
            this.pnlMenuPengaturan.MouseLeave += new System.EventHandler(this.SidebarMenu_MouseLeave);

            //
            // pnlMenuLogout — Logout di paling bawah sidebar
            //
            this.pnlMenuLogout.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlMenuLogout.Size = new System.Drawing.Size(240, 45);
            this.pnlMenuLogout.BackColor = System.Drawing.Color.FromArgb(61, 19, 19);
            this.pnlMenuLogout.Name = "pnlMenuLogout";
            this.pnlMenuLogout.Padding = new System.Windows.Forms.Padding(20, 0, 0, 0);
            this.pnlMenuLogout.Cursor = System.Windows.Forms.Cursors.Hand;
            //
            // lblMenuLogout
            //
            this.lblMenuLogout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblMenuLogout.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblMenuLogout.ForeColor = System.Drawing.Color.FromArgb(229, 115, 115); // #E57373 merah terang
            this.lblMenuLogout.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblMenuLogout.Name = "lblMenuLogout";
            this.lblMenuLogout.Text = "\U0001F6AA  Keluar";
            this.lblMenuLogout.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblMenuLogout.Paint += new System.Windows.Forms.PaintEventHandler(this.lblMenuLogout_Paint);
            this.lblMenuLogout.Click += new System.EventHandler(this.lblNavLogout_Click);
            this.lblMenuLogout.MouseEnter += new System.EventHandler(this.SidebarMenu_MouseEnter);
            this.lblMenuLogout.MouseLeave += new System.EventHandler(this.SidebarMenu_MouseLeave);
            this.pnlMenuLogout.Controls.Add(this.lblMenuLogout);
            this.pnlMenuLogout.Click += new System.EventHandler(this.lblNavLogout_Click);
            this.pnlMenuLogout.MouseEnter += new System.EventHandler(this.SidebarMenu_MouseEnter);
            this.pnlMenuLogout.MouseLeave += new System.EventHandler(this.SidebarMenu_MouseLeave);

            // Add menu items in reverse order (last added = docked first from bottom)
            // pnlMenuLogout Dock=Bottom added FIRST so it occupies bottom area before Top-docked items
            this.SidebarPanel.Controls.Add(this.pnlMenuLogout);
            this.SidebarPanel.Controls.Add(this.pnlMenuPengaturan);
            this.SidebarPanel.Controls.Add(this.pnlMenuMasterData);
            this.SidebarPanel.Controls.Add(this.pnlMenuLaporan);
            this.SidebarPanel.Controls.Add(this.pnlMenuArsip);
            this.SidebarPanel.Controls.Add(this.pnlMenuDashboard);
            this.SidebarPanel.Controls.Add(this.lblSidebarSubtitle);
            this.SidebarPanel.Controls.Add(this.lblSidebarTitle);

            // ================================================================
            // NavbarPanel — top navigation bar
            // ================================================================
            this.NavbarPanel.BackColor = System.Drawing.Color.FromArgb(84, 26, 26);
            this.NavbarPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.NavbarPanel.Size = new System.Drawing.Size(784, 50);
            this.NavbarPanel.Name = "NavbarPanel";
            this.NavbarPanel.TabIndex = 26;

            //
            // picLogo
            //
            this.picLogo.Location = new System.Drawing.Point(12, 8);
            this.picLogo.Name = "picLogo";
            this.picLogo.Size = new System.Drawing.Size(34, 34);
            this.picLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picLogo.TabStop = false;
            //
            // lblNavTitle
            //
            this.lblNavTitle.AutoSize = true;
            this.lblNavTitle.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblNavTitle.ForeColor = System.Drawing.Color.White;
            this.lblNavTitle.Location = new System.Drawing.Point(48, 14);
            this.lblNavTitle.Name = "lblNavTitle";
            this.lblNavTitle.Size = new System.Drawing.Size(100, 20);
            this.lblNavTitle.Text = "Arsip Surat";
            //
            // lblNavClock
            //
            this.lblNavClock.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblNavClock.AutoSize = true;
            this.lblNavClock.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblNavClock.ForeColor = System.Drawing.Color.FromArgb(220, 195, 170);
            this.lblNavClock.Location = new System.Drawing.Point(650, 17);
            this.lblNavClock.Name = "lblNavClock";
            this.lblNavClock.Size = new System.Drawing.Size(120, 15);
            this.lblNavClock.Text = "01/07/2026 00:00:00";
            //
            // lblNavUser
            //
            this.lblNavUser.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblNavUser.AutoSize = true;
            this.lblNavUser.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblNavUser.ForeColor = System.Drawing.Color.White;
            this.lblNavUser.Location = new System.Drawing.Point(520, 17);
            this.lblNavUser.Name = "lblNavUser";
            this.lblNavUser.Size = new System.Drawing.Size(50, 15);
            this.lblNavUser.Text = "admin";
            //
            // lblNavLogout
            //
            this.lblNavLogout.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblNavLogout.AutoSize = true;
            this.lblNavLogout.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.lblNavLogout.ForeColor = System.Drawing.Color.FromArgb(220, 195, 170);
            this.lblNavLogout.Location = new System.Drawing.Point(520, 2);
            this.lblNavLogout.Name = "lblNavLogout";
            this.lblNavLogout.Size = new System.Drawing.Size(40, 13);
            this.lblNavLogout.Text = "Logout";
            this.lblNavLogout.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblNavLogout.Click += new System.EventHandler(this.lblNavLogout_Click);

            this.NavbarPanel.Controls.Add(this.lblNavUser);
            this.NavbarPanel.Controls.Add(this.lblNavClock);
            this.NavbarPanel.Controls.Add(this.lblNavTitle);
            this.NavbarPanel.Controls.Add(this.picLogo);

            // ================================================================
            // FooterPanel — bottom bar
            // ================================================================
            this.FooterPanel.BackColor = System.Drawing.Color.FromArgb(220, 195, 170);
            this.FooterPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.FooterPanel.Size = new System.Drawing.Size(784, 30);
            this.FooterPanel.Name = "FooterPanel";
            this.FooterPanel.TabIndex = 32;

            //
            // lblFooterLeft
            //
            this.lblFooterLeft.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblFooterLeft.AutoSize = true;
            this.lblFooterLeft.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblFooterLeft.ForeColor = System.Drawing.Color.FromArgb(84, 26, 26);
            this.lblFooterLeft.Location = new System.Drawing.Point(12, 8);
            this.lblFooterLeft.Name = "lblFooterLeft";
            this.lblFooterLeft.Size = new System.Drawing.Size(130, 15);
            this.lblFooterLeft.Text = "\u00A9 2026 Arsip Surat";
            //
            // lblFooterRight
            //
            this.lblFooterRight.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblFooterRight.AutoSize = true;
            this.lblFooterRight.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblFooterRight.ForeColor = System.Drawing.Color.FromArgb(84, 26, 26);
            this.lblFooterRight.Location = new System.Drawing.Point(720, 8);
            this.lblFooterRight.Name = "lblFooterRight";
            this.lblFooterRight.Size = new System.Drawing.Size(50, 15);
            this.lblFooterRight.Text = "v1.0.0";

            this.FooterPanel.Controls.Add(this.lblFooterRight);
            this.FooterPanel.Controls.Add(this.lblFooterLeft);

            // ================================================================
            // MainForm
            // ================================================================
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(241, 226, 209);
            this.ClientSize = new System.Drawing.Size(1100, 700);
            this.MinimumSize = new System.Drawing.Size(1100, 700);

            // Controls.Add order matters for docking!
            // Processing order is reverse: Footer→bottom, Navbar→top, Sidebar→left, Content→fill
            this.Controls.Add(this.ContentPanel);
            this.Controls.Add(this.SidebarPanel);
            this.Controls.Add(this.NavbarPanel);
            this.Controls.Add(this.FooterPanel);

            this.Name = "MainForm";
            this.Text = "Arsip Surat";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.dgvSurat)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        // ── Original controls (reparented to CardPanel) ──
        private System.Windows.Forms.DataGridView dgvSurat;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Button btnNew;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.CheckBox chkShowDeleted;
        private System.Windows.Forms.Button btnRestore;
        private System.Windows.Forms.Button btnPermanentDelete;
        private System.Windows.Forms.Label labelFilterFrom;
        private System.Windows.Forms.DateTimePicker dtpFilterFrom;
        private System.Windows.Forms.Label labelFilterTo;
        private System.Windows.Forms.DateTimePicker dtpFilterTo;
        private System.Windows.Forms.Label labelFilterJenis;
        private System.Windows.Forms.ComboBox cmbFilterJenis;
        private System.Windows.Forms.Label labelFilterStatus;
        private System.Windows.Forms.ComboBox cmbFilterStatus;
        private System.Windows.Forms.Button btnResetFilter;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.FlowLayoutPanel toolbarPanel;

        // ── Layout panels ──
        private System.Windows.Forms.Panel NavbarPanel;
        private System.Windows.Forms.Panel SidebarPanel;
        private System.Windows.Forms.Panel ContentPanel;
        private System.Windows.Forms.Panel CardPanel;
        private System.Windows.Forms.Panel filterPanel;
        private System.Windows.Forms.Panel BreadcrumbPanel;
        private System.Windows.Forms.Panel FooterPanel;

        // ── Navbar controls ──
        private System.Windows.Forms.PictureBox picLogo;
        private System.Windows.Forms.Label lblNavTitle;
        private System.Windows.Forms.Label lblNavClock;
        private System.Windows.Forms.Label lblNavUser;
        private System.Windows.Forms.Label lblNavLogout;

        // ── Sidebar controls ──
        private System.Windows.Forms.Label lblSidebarTitle;
        private System.Windows.Forms.Label lblSidebarSubtitle;
        private System.Windows.Forms.Panel pnlMenuDashboard;
        private System.Windows.Forms.Label lblMenuDashboard;
        private System.Windows.Forms.Panel pnlMenuArsip;
        private System.Windows.Forms.Label lblMenuArsip;
        private System.Windows.Forms.Panel pnlMenuLaporan;
        private System.Windows.Forms.Label lblMenuLaporan;
        private System.Windows.Forms.Panel pnlMenuMasterData;
        private System.Windows.Forms.Label lblMenuMasterData;
        private System.Windows.Forms.Panel pnlMenuPengaturan;
        private System.Windows.Forms.Label lblMenuPengaturan;
        private System.Windows.Forms.Panel pnlMenuLogout;
        private System.Windows.Forms.Label lblMenuLogout;

        // ── Breadcrumb controls ──
        private System.Windows.Forms.Label lblBreadcrumbHome;
        private System.Windows.Forms.Label lblBreadcrumbPage;

        // ── Footer controls ──
        private System.Windows.Forms.Label lblFooterLeft;
        private System.Windows.Forms.Label lblFooterRight;
    }
}
