using System;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace ArsipSurat
{
    public partial class SuratDetailForm : Form
    {
        private int suratId;
        private bool includeDeleted;
        private SuratRepository repository = new SuratRepository();
        private LampiranRepository lampiranRepo = new LampiranRepository();

        public SuratDetailForm(int id, bool showDeleted = false)
        {
            InitializeComponent();
            suratId = id;
            includeDeleted = showDeleted;
            ApplyTheme();
            if (showDeleted)
            {
                btnEdit.Visible = false;
                btnDelete.Text = "Restore";
                btnDelete.Click -= btnDelete_Click;
                btnDelete.Click += btnRestore_Click;
            }
            LoadData();
            LoadAttachments();
        }

        private void ApplyTheme()
        {
            this.BackColor = Color.FromArgb(248, 249, 250);
            this.Font = new Font("Segoe UI", 9F);
            
            StyleButton(btnEdit, Color.FromArgb(41, 98, 255), Color.FromArgb(30, 80, 220));
            StyleButton(btnDelete, Color.FromArgb(220, 53, 69), Color.FromArgb(180, 40, 55));
            StyleButton(btnClose, Color.FromArgb(108, 117, 125), Color.FromArgb(90, 98, 105));
            StyleButton(btnOpenFile, Color.FromArgb(23, 162, 184), Color.FromArgb(18, 130, 150));
            
            foreach (Control c in this.Controls)
            {
                if (c is Label && c.Name.StartsWith("label"))
                {
                    c.ForeColor = Color.FromArgb(73, 80, 87);
                    c.Font = new Font("Segoe UI", 9F);
                }
            }
            
            lblNomor.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblNomor.ForeColor = Color.FromArgb(33, 37, 41);

            lstAttachments.Font = new Font("Segoe UI", 9F);
            lblKeterangan.Font = new Font("Segoe UI", 9F);
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
        }

        private void LoadData()
        {
            var surat = repository.GetById(suratId, includeDeleted);
            if (surat == null)
            {
                MessageBox.Show("Surat tidak ditemukan", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }
            lblNomor.Text = surat.NomorSurat;
            lblTanggal.Text = surat.TanggalSurat.ToString("dd/MM/yyyy");
            lblJenis.Text = surat.JenisSurat;
            lblPengirim.Text = surat.Pengirim ?? "-";
            lblPenerima.Text = surat.Penerima ?? "-";
            lblPerihal.Text = surat.Perihal;
            lblStatus.Text = surat.Status;
            lblKeterangan.Text = surat.Keterangan ?? "-";
        }

        private void LoadAttachments()
        {
            lstAttachments.Items.Clear();
            DataTable dt = lampiranRepo.GetBySuratId(suratId);
            foreach (DataRow row in dt.Rows)
            {
                lstAttachments.Items.Add(new AttachmentItem
                {
                    Id = (int)row["id"],
                    FileName = row["nama_file"].ToString(),
                    FilePath = row["file_path"].ToString(),
                    IsNew = false
                });
            }
        }

        private void lstAttachments_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstAttachments.SelectedItem == null)
            {
                picPreview.Image = null;
                return;
            }
            var item = (AttachmentItem)lstAttachments.SelectedItem;
            string fullPath = FileStorage.GetFullPath(item.FilePath);
            ShowPreview(fullPath);
        }

        private void ShowPreview(string filePath)
        {
            picPreview.Image = null;
            if (!File.Exists(filePath)) return;
            string ext = Path.GetExtension(filePath).ToLower();
            if (ext == ".jpg" || ext == ".jpeg" || ext == ".png")
            {
                try
                {
                    using (var img = Image.FromFile(filePath))
                    {
                        picPreview.Image = new Bitmap(img);
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Preview error: " + ex.Message);
                }
            }
        }

        private void btnOpenFile_Click(object sender, EventArgs e)
        {
            if (lstAttachments.SelectedItem == null) return;
            var item = (AttachmentItem)lstAttachments.SelectedItem;
            string fullPath = FileStorage.GetFullPath(item.FilePath);
            if (File.Exists(fullPath))
            {
                Process.Start(fullPath);
            }
            else
            {
                MessageBox.Show("File tidak ditemukan", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            using (var form = new SuratInputForm(suratId))
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    LoadData();
                    LoadAttachments();
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            var surat = repository.GetById(suratId, includeDeleted);
            if (surat == null)
            {
                MessageBox.Show("Surat tidak ditemukan", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.DialogResult = DialogResult.OK;
                this.Close();
                return;
            }
            if (MessageBox.Show(string.Format("Yakin hapus surat {0}?", surat.NomorSurat), "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                repository.SoftDelete(suratId);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnRestore_Click(object sender, EventArgs e)
        {
            repository.Restore(suratId);
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
