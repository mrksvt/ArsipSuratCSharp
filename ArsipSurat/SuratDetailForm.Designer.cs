namespace ArsipSurat
{
    partial class SuratDetailForm
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
            this.lblNomor = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblTanggal = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblJenis = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lblPengirim = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.lblPenerima = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.lblPerihal = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.lblKeterangan = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.lstAttachments = new System.Windows.Forms.ListBox();
            this.btnOpenFile = new System.Windows.Forms.Button();
            this.picPreview = new System.Windows.Forms.PictureBox();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
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
            // lblNomor
            // 
            this.lblNomor.AutoSize = true;
            this.lblNomor.Location = new System.Drawing.Point(100, 15);
            this.lblNomor.Name = "lblNomor";
            this.lblNomor.Size = new System.Drawing.Size(10, 13);
            this.lblNomor.TabIndex = 1;
            this.lblNomor.Text = "-";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 38);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(49, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Tanggal:";
            // 
            // lblTanggal
            // 
            this.lblTanggal.AutoSize = true;
            this.lblTanggal.Location = new System.Drawing.Point(100, 38);
            this.lblTanggal.Name = "lblTanggal";
            this.lblTanggal.Size = new System.Drawing.Size(10, 13);
            this.lblTanggal.TabIndex = 3;
            this.lblTanggal.Text = "-";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 61);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(66, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Jenis Surat:";
            // 
            // lblJenis
            // 
            this.lblJenis.AutoSize = true;
            this.lblJenis.Location = new System.Drawing.Point(100, 61);
            this.lblJenis.Name = "lblJenis";
            this.lblJenis.Size = new System.Drawing.Size(10, 13);
            this.lblJenis.TabIndex = 5;
            this.lblJenis.Text = "-";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 84);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(54, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Pengirim:";
            // 
            // lblPengirim
            // 
            this.lblPengirim.AutoSize = true;
            this.lblPengirim.Location = new System.Drawing.Point(100, 84);
            this.lblPengirim.Name = "lblPengirim";
            this.lblPengirim.Size = new System.Drawing.Size(10, 13);
            this.lblPengirim.TabIndex = 7;
            this.lblPengirim.Text = "-";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 107);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(54, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "Penerima:";
            // 
            // lblPenerima
            // 
            this.lblPenerima.AutoSize = true;
            this.lblPenerima.Location = new System.Drawing.Point(100, 107);
            this.lblPenerima.Name = "lblPenerima";
            this.lblPenerima.Size = new System.Drawing.Size(10, 13);
            this.lblPenerima.TabIndex = 9;
            this.lblPenerima.Text = "-";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 130);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(49, 13);
            this.label6.TabIndex = 10;
            this.label6.Text = "Perihal:";
            // 
            // lblPerihal
            // 
            this.lblPerihal.AutoSize = true;
            this.lblPerihal.Location = new System.Drawing.Point(100, 130);
            this.lblPerihal.Name = "lblPerihal";
            this.lblPerihal.Size = new System.Drawing.Size(10, 13);
            this.lblPerihal.TabIndex = 11;
            this.lblPerihal.Text = "-";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, 153);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(40, 13);
            this.label7.TabIndex = 12;
            this.label7.Text = "Status:";
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(100, 153);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(10, 13);
            this.lblStatus.TabIndex = 13;
            this.lblStatus.Text = "-";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(12, 176);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(65, 13);
            this.label8.TabIndex = 14;
            this.label8.Text = "Keterangan:";
            // 
            // lblKeterangan
            // 
            this.lblKeterangan.Location = new System.Drawing.Point(100, 176);
            this.lblKeterangan.Name = "lblKeterangan";
            this.lblKeterangan.Size = new System.Drawing.Size(300, 40);
            this.lblKeterangan.TabIndex = 15;
            this.lblKeterangan.Text = "-";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(12, 230);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(58, 13);
            this.label9.TabIndex = 16;
            this.label9.Text = "Lampiran:";
            // 
            // lstAttachments
            // 
            this.lstAttachments.FormattingEnabled = true;
            this.lstAttachments.Location = new System.Drawing.Point(100, 227);
            this.lstAttachments.Name = "lstAttachments";
            this.lstAttachments.Size = new System.Drawing.Size(200, 69);
            this.lstAttachments.TabIndex = 17;
            this.lstAttachments.SelectedIndexChanged += new System.EventHandler(this.lstAttachments_SelectedIndexChanged);
            // 
            // btnOpenFile
            // 
            this.btnOpenFile.Location = new System.Drawing.Point(306, 227);
            this.btnOpenFile.Name = "btnOpenFile";
            this.btnOpenFile.Size = new System.Drawing.Size(75, 23);
            this.btnOpenFile.TabIndex = 18;
            this.btnOpenFile.Text = "Open";
            this.btnOpenFile.UseVisualStyleBackColor = true;
            this.btnOpenFile.Click += new System.EventHandler(this.btnOpenFile_Click);
            // 
            // picPreview
            // 
            this.picPreview.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picPreview.Location = new System.Drawing.Point(306, 260);
            this.picPreview.Name = "picPreview";
            this.picPreview.Size = new System.Drawing.Size(150, 150);
            this.picPreview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picPreview.TabIndex = 19;
            this.picPreview.TabStop = false;
            // 
            // btnEdit
            // 
            this.btnEdit.Location = new System.Drawing.Point(100, 420);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(75, 23);
            this.btnEdit.TabIndex = 20;
            this.btnEdit.Text = "Edit";
            this.btnEdit.UseVisualStyleBackColor = true;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(185, 420);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 21;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(270, 420);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 22;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // SuratDetailForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(470, 460);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnEdit);
            this.Controls.Add(this.picPreview);
            this.Controls.Add(this.btnOpenFile);
            this.Controls.Add(this.lstAttachments);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.lblKeterangan);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.lblPerihal);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.lblPenerima);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.lblPengirim);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.lblJenis);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lblTanggal);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblNomor);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SuratDetailForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            ((System.ComponentModel.ISupportInitialize)(this.picPreview)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblNomor;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblTanggal;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblJenis;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblPengirim;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lblPenerima;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lblPerihal;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label lblKeterangan;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ListBox lstAttachments;
        private System.Windows.Forms.Button btnOpenFile;
        private System.Windows.Forms.PictureBox picPreview;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnClose;
    }
}
