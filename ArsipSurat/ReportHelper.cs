using System;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace ArsipSurat
{
    public static class ReportHelper
    {
        public static void ExportToCsv(DataTable dt, string filePath)
        {
            StringBuilder sb = new StringBuilder();
            
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                if (i > 0) sb.Append(",");
                sb.Append(dt.Columns[i].ColumnName);
            }
            sb.AppendLine();
            
            foreach (DataRow row in dt.Rows)
            {
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    if (i > 0) sb.Append(",");
                    string value = row[i].ToString().Replace("\"", "\"\"");
                    if (value.Length > 0 && (value[0] == '=' || value[0] == '+' || value[0] == '-' || value[0] == '@'))
                    {
                        value = "'" + value;
                    }
                    sb.Append(string.Format("\"{0}\"", value));
                }
                sb.AppendLine();
            }
            
            File.WriteAllText(filePath, sb.ToString(), Encoding.UTF8);
        }

        public static void PrintDataGridView(DataGridView dgv, string title)
        {
            PrintDocument printDoc = new PrintDocument();
            printDoc.DocumentName = title;
            
            int currentRow = 0;
            int currentPage = 1;
            
            printDoc.PrintPage += (sender, e) =>
            {
                Graphics g = e.Graphics;
                float lineHeight = 20;
                float x = e.MarginBounds.Left;
                float y = e.MarginBounds.Top;
                
                using (Font titleFont = new Font("Arial", 14, FontStyle.Bold))
                {
                    g.DrawString(title, titleFont, Brushes.Black, x, y);
                    y += lineHeight * 2;
                }
                
                using (Font headerFont = new Font("Arial", 9, FontStyle.Bold))
                using (Font bodyFont = new Font("Arial", 9))
                {
                    float[] colWidths = new float[dgv.Columns.Count];
                    float totalWidth = e.MarginBounds.Width;
                    float colWidth = totalWidth / dgv.Columns.Count;
                    
                    for (int i = 0; i < dgv.Columns.Count; i++)
                    {
                        colWidths[i] = colWidth;
                        g.FillRectangle(Brushes.LightGray, x, y, colWidth, lineHeight);
                        g.DrawRectangle(Pens.Black, x, y, colWidth, lineHeight);
                        g.DrawString(dgv.Columns[i].HeaderText, headerFont, Brushes.Black, x + 2, y + 2);
                        x += colWidth;
                    }
                    y += lineHeight;
                    
                    while (currentRow < dgv.Rows.Count)
                    {
                        if (y + lineHeight > e.MarginBounds.Bottom)
                        {
                            e.HasMorePages = true;
                            currentPage++;
                            return;
                        }
                        
                        x = e.MarginBounds.Left;
                        DataGridViewRow row = dgv.Rows[currentRow];
                        
                        for (int i = 0; i < dgv.Columns.Count; i++)
                        {
                            string value = row.Cells[i].Value?.ToString() ?? "";
                            if (value.Length > 30) value = value.Substring(0, 27) + "...";
                            g.DrawRectangle(Pens.Black, x, y, colWidths[i], lineHeight);
                            g.DrawString(value, bodyFont, Brushes.Black, x + 2, y + 2);
                            x += colWidths[i];
                        }
                        y += lineHeight;
                        currentRow++;
                    }
                    
                    y += lineHeight;
                    g.DrawString(string.Format("Total: {0} records", dgv.Rows.Count), bodyFont, Brushes.Black, x, y);
                }
            };
            
            using (PrintPreviewDialog preview = new PrintPreviewDialog())
            {
                preview.Document = printDoc;
                preview.ShowDialog();
            }
        }

        public static void ShowExportDialog(DataTable dt, string defaultName)
        {
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "CSV File|*.csv|Text File|*.txt";
                sfd.FileName = defaultName + "_" + DateTime.Now.ToString("yyyyMMdd");
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    ExportToCsv(dt, sfd.FileName);
                    MessageBox.Show("Export berhasil: " + sfd.FileName, "Export", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
    }
}
