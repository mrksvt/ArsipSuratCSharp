namespace ArsipSurat
{
    partial class SuratInputForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.txtNomor = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.dtpTanggal = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbJenis = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtPengirim = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtPenerima = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtPerihal = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.cmbStatus = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtKeterangan = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.lstAttachments = new System.Windows.Forms.ListBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.btnScan = new System.Windows.Forms.Button();
            this.btnRemoveFile = new System.Windows.Forms.Button();
            this.picPreview = new System.Windows.Forms.PictureBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.chkOcrMode = new System.Windows.Forms.CheckBox();
            this.btnAnalyzeOcr = new System.Windows.Forms.Button();
            this.lblOcrConfidence = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.picPreview)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Nomor Surat:";
            // 
            // txtNomor
            // 
            this.txtNomor.Location = new System.Drawing.Point(100, 12);
            this.txtNomor.Name = "txtNomor";
            this.txtNomor.Size = new System.Drawing.Size(200, 20);
            this.txtNomor.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(49, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Tanggal:";
            // 
            // dtpTanggal
            // 
            this.dtpTanggal.Location = new System.Drawing.Point(100, 38);
            this.dtpTanggal.Name = "dtpTanggal";
            this.dtpTanggal.Size = new System.Drawing.Size(200, 20);
            this.dtpTanggal.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 67);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(66, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Jenis Surat:";
            // 
            // cmbJenis
            // 
            this.cmbJenis.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbJenis.FormattingEnabled = true;
            this.cmbJenis.Location = new System.Drawing.Point(100, 64);
            this.cmbJenis.Name = "cmbJenis";
            this.cmbJenis.Size = new System.Drawing.Size(120, 21);
            this.cmbJenis.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 93);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(54, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Pengirim:";
            // 
            // txtPengirim
            // 
            this.txtPengirim.Location = new System.Drawing.Point(100, 90);
            this.txtPengirim.Name = "txtPengirim";
            this.txtPengirim.Size = new System.Drawing.Size(200, 20);
            this.txtPengirim.TabIndex = 7;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 119);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(54, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "Penerima:";
            // 
            // txtPenerima
            // 
            this.txtPenerima.Location = new System.Drawing.Point(100, 116);
            this.txtPenerima.Name = "txtPenerima";
            this.txtPenerima.Size = new System.Drawing.Size(200, 20);
            this.txtPenerima.TabIndex = 9;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 145);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(49, 13);
            this.label6.TabIndex = 10;
            this.label6.Text = "Perihal:";
            // 
            // txtPerihal
            // 
            this.txtPerihal.Location = new System.Drawing.Point(100, 142);
            this.txtPerihal.Name = "txtPerihal";
            this.txtPerihal.Size = new System.Drawing.Size(300, 20);
            this.txtPerihal.TabIndex = 11;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, 171);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(40, 13);
            this.label7.TabIndex = 12;
            this.label7.Text = "Status:";
            // 
            // cmbStatus
            // 
            this.cmbStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbStatus.FormattingEnabled = true;
            this.cmbStatus.Location = new System.Drawing.Point(100, 168);
            this.cmbStatus.Name = "cmbStatus";
            this.cmbStatus.Size = new System.Drawing.Size(120, 21);
            this.cmbStatus.TabIndex = 13;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(12, 197);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(65, 13);
            this.label8.TabIndex = 14;
            this.label8.Text = "Keterangan:";
            // 
            // txtKeterangan
            // 
            this.txtKeterangan.Location = new System.Drawing.Point(100, 194);
            this.txtKeterangan.Multiline = true;
            this.txtKeterangan.Name = "txtKeterangan";
            this.txtKeterangan.Size = new System.Drawing.Size(300, 60);
            this.txtKeterangan.TabIndex = 15;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(12, 268);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(58, 13);
            this.label9.TabIndex = 16;
            this.label9.Text = "Lampiran:";
            // 
            // lstAttachments
            // 
            this.lstAttachments.FormattingEnabled = true;
            this.lstAttachments.Location = new System.Drawing.Point(100, 265);
            this.lstAttachments.Name = "lstAttachments";
            this.lstAttachments.Size = new System.Drawing.Size(200, 69);
            this.lstAttachments.TabIndex = 17;
            this.lstAttachments.SelectedIndexChanged += new System.EventHandler(this.lstAttachments_SelectedIndexChanged);
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(306, 265);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(50, 23);
            this.btnBrowse.TabIndex = 18;
            this.btnBrowse.Text = "...";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // btnScan
            // 
            this.btnScan.Location = new System.Drawing.Point(360, 265);
            this.btnScan.Name = "btnScan";
            this.btnScan.Size = new System.Drawing.Size(50, 23);
            this.btnScan.TabIndex = 23;
            this.btnScan.Text = "Scan";
            this.btnScan.UseVisualStyleBackColor = true;
            this.btnScan.Click += new System.EventHandler(this.btnScan_Click);
            // 
            // btnRemoveFile
            // 
            this.btnRemoveFile.Location = new System.Drawing.Point(306, 294);
            this.btnRemoveFile.Name = "btnRemoveFile";
            this.btnRemoveFile.Size = new System.Drawing.Size(50, 23);
            this.btnRemoveFile.TabIndex = 19;
            this.btnRemoveFile.Text = "X";
            this.btnRemoveFile.UseVisualStyleBackColor = true;
            this.btnRemoveFile.Click += new System.EventHandler(this.btnRemoveFile_Click);
            // 
            // picPreview
            // 
            this.picPreview.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picPreview.Location = new System.Drawing.Point(365, 265);
            this.picPreview.Name = "picPreview";
            this.picPreview.Size = new System.Drawing.Size(120, 120);
            this.picPreview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picPreview.TabIndex = 20;
            this.picPreview.TabStop = false;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(100, 400);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 21;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(185, 400);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 22;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // chkOcrMode
            // 
            this.chkOcrMode.AutoSize = true;
            this.chkOcrMode.Location = new System.Drawing.Point(100, 260);
            this.chkOcrMode.Name = "chkOcrMode";
            this.chkOcrMode.Size = new System.Drawing.Size(100, 17);
            this.chkOcrMode.TabIndex = 24;
            this.chkOcrMode.Text = "OCR Mode ON";
            this.chkOcrMode.UseVisualStyleBackColor = true;
            this.chkOcrMode.CheckedChanged += new System.EventHandler(this.chkOcrMode_CheckedChanged);
            // 
            // btnAnalyzeOcr
            // 
            this.btnAnalyzeOcr.Location = new System.Drawing.Point(210, 258);
            this.btnAnalyzeOcr.Name = "btnAnalyzeOcr";
            this.btnAnalyzeOcr.Size = new System.Drawing.Size(80, 23);
            this.btnAnalyzeOcr.TabIndex = 25;
            this.btnAnalyzeOcr.Text = "Analyze OCR";
            this.btnAnalyzeOcr.UseVisualStyleBackColor = true;
            this.btnAnalyzeOcr.Visible = false;
            this.btnAnalyzeOcr.Click += new System.EventHandler(this.btnAnalyzeOcr_Click);
            // 
            // lblOcrConfidence
            // 
            this.lblOcrConfidence.AutoSize = true;
            this.lblOcrConfidence.Location = new System.Drawing.Point(300, 262);
            this.lblOcrConfidence.Name = "lblOcrConfidence";
            this.lblOcrConfidence.Size = new System.Drawing.Size(0, 13);
            this.lblOcrConfidence.TabIndex = 26;
            // 
            // SuratInputForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(500, 440);
            this.Controls.Add(this.lblOcrConfidence);
            this.Controls.Add(this.btnAnalyzeOcr);
            this.Controls.Add(this.chkOcrMode);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.picPreview);
            this.Controls.Add(this.btnScan);
            this.Controls.Add(this.btnRemoveFile);
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.lstAttachments);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.txtKeterangan);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.cmbStatus);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.txtPerihal);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtPenerima);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtPengirim);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.cmbJenis);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.dtpTanggal);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtNomor);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SuratInputForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            ((System.ComponentModel.ISupportInitialize)(this.picPreview)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtNomor;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker dtpTanggal;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cmbJenis;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtPengirim;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtPenerima;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtPerihal;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox cmbStatus;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtKeterangan;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ListBox lstAttachments;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.Button btnScan;
        private System.Windows.Forms.Button btnRemoveFile;
        private System.Windows.Forms.PictureBox picPreview;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.CheckBox chkOcrMode;
        private System.Windows.Forms.Button btnAnalyzeOcr;
        private System.Windows.Forms.Label lblOcrConfidence;
    }
}
