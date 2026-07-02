using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace ArsipSurat
{
    public partial class SuratInputForm : Form
    {
        private int? editingId;
        private SuratRepository repository = new SuratRepository();
        private LampiranRepository lampiranRepo = new LampiranRepository();
        private List<string> pendingFiles = new List<string>();
        private string currentOcrFile;

        public SuratInputForm(int? id = null)
        {
            InitializeComponent();
            editingId = id;
            cmbJenis.Items.AddRange(new object[] { "Masuk", "Keluar" });
            cmbStatus.Items.AddRange(new object[] { "Draft", "Diterima", "Diproses", "Selesai" });
            ApplyTheme();
            if (id.HasValue)
            {
                LoadData(id.Value);
                this.Text = "Edit Surat";
            }
            else
            {
                this.Text = "New Surat";
                dtpTanggal.Value = DateTime.Today;
                cmbJenis.SelectedIndex = 0;
                cmbStatus.SelectedIndex = 0;
            }
        }

        private void ApplyTheme()
        {
            this.BackColor = Color.FromArgb(248, 249, 250);
            this.Font = new Font("Segoe UI", 9F);

            StyleButton(btnSave, Color.FromArgb(41, 98, 255), Color.FromArgb(30, 80, 220));
            StyleButton(btnCancel, Color.FromArgb(108, 117, 125), Color.FromArgb(90, 98, 105));
            StyleButton(btnBrowse, Color.FromArgb(23, 162, 184), Color.FromArgb(18, 130, 150));
            StyleButton(btnScan, Color.FromArgb(108, 117, 125), Color.FromArgb(90, 98, 105));
            StyleButton(btnRemoveFile, Color.FromArgb(220, 53, 69), Color.FromArgb(180, 40, 55));
            StyleButton(btnAnalyzeOcr, Color.FromArgb(255, 193, 7), Color.FromArgb(200, 150, 0));

            foreach (Control c in this.Controls)
            {
                if (c is Label && c.Name.StartsWith("label"))
                {
                    c.ForeColor = Color.FromArgb(73, 80, 87);
                    c.Font = new Font("Segoe UI", 9F);
                }
                if (c is ComboBox)
                {
                    var cmb = (ComboBox)c;
                    cmb.FlatStyle = FlatStyle.Flat;
                    cmb.Font = new Font("Segoe UI", 9F);
                    cmb.BackColor = Color.White;
                    cmb.ForeColor = Color.FromArgb(33, 37, 41);
                }
                if (c is TextBox)
                {
                    var txt = (TextBox)c;
                    txt.Font = new Font("Segoe UI", 9F);
                }
                if (c is DateTimePicker)
                {
                    var dtp = (DateTimePicker)c;
                    dtp.Font = new Font("Segoe UI", 9F);
                }
            }

            lstAttachments.Font = new Font("Segoe UI", 9F);
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

        private void LoadData(int id)
        {
            var surat = repository.GetById(id);
            if (surat == null) return;
            txtNomor.Text = surat.NomorSurat;
            dtpTanggal.Value = surat.TanggalSurat;
            cmbJenis.SelectedItem = surat.JenisSurat;
            txtPengirim.Text = surat.Pengirim;
            txtPenerima.Text = surat.Penerima;
            txtPerihal.Text = surat.Perihal;
            cmbStatus.SelectedItem = surat.Status;
            txtKeterangan.Text = surat.Keterangan;
            LoadAttachments(id);
        }

        private void LoadAttachments(int suratId)
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

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Documents|*.pdf;*.jpg;*.jpeg;*.png";
                ofd.Multiselect = true;
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    foreach (string file in ofd.FileNames)
                    {
                        string error = FileStorage.GetValidationError(file);
                        if (error != null)
                        {
                            MessageBox.Show(error, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            continue;
                        }
                        pendingFiles.Add(file);
                        lstAttachments.Items.Add(new AttachmentItem
                        {
                            FileName = Path.GetFileName(file),
                            FilePath = file,
                            IsNew = true
                        });
                    }
                }
            }
        }

        private void btnScan_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ScannerHelper.IsScannerAvailable())
                {
                    MessageBox.Show("Scanner tidak ditemukan. Pastikan scanner terhubung.", "Scanner", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                
                string scannedFile = ScannerHelper.ScanToFile();
                if (scannedFile != null)
                {
                    pendingFiles.Add(scannedFile);
                    lstAttachments.Items.Add(new AttachmentItem
                    {
                        FileName = Path.GetFileName(scannedFile),
                        FilePath = scannedFile,
                        IsNew = true
                    });
                    currentOcrFile = scannedFile;
                    ShowPreview(scannedFile);
                    if (chkOcrMode.Checked)
                    {
                        AnalyzeOcr(scannedFile);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Scan error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void chkOcrMode_CheckedChanged(object sender, EventArgs e)
        {
            btnAnalyzeOcr.Visible = chkOcrMode.Checked;
        }

        private void btnAnalyzeOcr_Click(object sender, EventArgs e)
        {
            if (lstAttachments.SelectedItem == null)
            {
                MessageBox.Show("Pilih file terlebih dahulu", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            var item = (AttachmentItem)lstAttachments.SelectedItem;
            string fullPath = item.IsNew ? item.FilePath : FileStorage.GetFullPath(item.FilePath);
            AnalyzeOcr(fullPath);
        }

        private void AnalyzeOcr(string filePath)
        {
            if (!File.Exists(filePath))
            {
                MessageBox.Show("File tidak ditemukan", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string ext = Path.GetExtension(filePath).ToLower();
            if (ext != ".jpg" && ext != ".jpeg" && ext != ".png")
            {
                MessageBox.Show("OCR hanya support file gambar (JPG, PNG)", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            Cursor = Cursors.WaitCursor;
            var result = OcrHelper.ExtractText(filePath);
            Cursor = Cursors.Default;

            if (!result.Success)
            {
                MessageBox.Show(result.Error, "OCR Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!string.IsNullOrEmpty(result.NomorSurat))
            {
                txtNomor.Text = result.NomorSurat;
                txtNomor.BackColor = OcrHelper.GetConfidenceColor(result.Confidence);
            }
            if (!string.IsNullOrEmpty(result.Perihal))
            {
                txtPerihal.Text = result.Perihal;
                txtPerihal.BackColor = OcrHelper.GetConfidenceColor(result.Confidence);
            }
            if (!string.IsNullOrEmpty(result.JenisSurat))
            {
                cmbJenis.SelectedItem = result.JenisSurat;
            }

            lblOcrConfidence.Text = string.Format("OCR Confidence: {0:F1}%", result.Confidence);
            lblOcrConfidence.BackColor = OcrHelper.GetConfidenceColor(result.Confidence);
        }

        private void btnRemoveFile_Click(object sender, EventArgs e)
        {
            if (lstAttachments.SelectedItem == null) return;
            var item = (AttachmentItem)lstAttachments.SelectedItem;
            if (item.IsNew)
            {
                pendingFiles.Remove(item.FilePath);
            }
            else
            {
                lampiranRepo.Delete(item.Id);
            }
            lstAttachments.Items.Remove(item);
            picPreview.Image = null;
        }

        private void lstAttachments_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstAttachments.SelectedItem == null)
            {
                picPreview.Image = null;
                return;
            }
            var item = (AttachmentItem)lstAttachments.SelectedItem;
            string fullPath = item.IsNew ? item.FilePath : FileStorage.GetFullPath(item.FilePath);
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

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateInput()) return;
            var surat = new Surat
            {
                NomorSurat = txtNomor.Text.Trim(),
                TanggalSurat = dtpTanggal.Value.Date,
                JenisSurat = cmbJenis.SelectedItem.ToString(),
                Pengirim = txtPengirim.Text.Trim(),
                Penerima = txtPenerima.Text.Trim(),
                Perihal = txtPerihal.Text.Trim(),
                Status = cmbStatus.SelectedItem.ToString(),
                Keterangan = txtKeterangan.Text.Trim()
            };

            try
            {
                if (editingId.HasValue)
                {
                    surat.Id = editingId.Value;
                    repository.Update(surat);
                    SavePendingFiles(editingId.Value, surat.NomorSurat, surat.TanggalSurat);
                }
                else
                {
                    int newId = repository.Insert(surat);
                    SavePendingFiles(newId, surat.NomorSurat, surat.TanggalSurat);
                }

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                pendingFiles.Clear();
                if (ex.Number == 1062)
                {
                    MessageBox.Show("Nomor surat sudah ada. Gunakan nomor lain.", "Duplikat", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtNomor.Focus();
                }
                else
                {
                    MessageBox.Show("Database error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                pendingFiles.Clear();
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SavePendingFiles(int suratId, string nomorSurat, DateTime tanggal)
        {
            foreach (string filePath in pendingFiles)
            {
                string relativePath = FileStorage.CopyToStorage(filePath, nomorSurat, tanggal);
                string fileName = Path.GetFileName(filePath);
                long fileSize = new FileInfo(filePath).Length;
                string fileType = Path.GetExtension(filePath).TrimStart('.');
                lampiranRepo.Insert(suratId, fileName, relativePath, fileSize, fileType);
            }
            pendingFiles.Clear();
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtNomor.Text))
            {
                MessageBox.Show("Nomor surat wajib diisi", "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNomor.Focus();
                return false;
            }
            if (cmbJenis.SelectedIndex < 0)
            {
                MessageBox.Show("Jenis surat wajib dipilih", "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtPerihal.Text))
            {
                MessageBox.Show("Perihal wajib diisi", "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPerihal.Focus();
                return false;
            }
            if (cmbStatus.SelectedIndex < 0)
            {
                MessageBox.Show("Status wajib dipilih", "Validasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }

    public class AttachmentItem
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public bool IsNew { get; set; }
        public override string ToString() { return FileName; }
    }
}
