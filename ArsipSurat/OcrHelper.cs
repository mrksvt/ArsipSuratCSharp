using System;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;

namespace ArsipSurat
{
    public static class OcrHelper
    {
        public static OcrResult ExtractText(string filePath)
        {
            try
            {
                string tessDataPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "tessdata");
                if (!Directory.Exists(tessDataPath))
                {
                    return new OcrResult { Success = false, Error = "Tessdata folder not found. Place tessdata folder in application directory." };
                }

                using (var engine = new Tesseract.TesseractEngine(tessDataPath, "ind+eng", Tesseract.EngineMode.Default))
                {
                    using (var img = Tesseract.Pix.LoadFromFile(filePath))
                    {
                        using (var page = engine.Process(img))
                        {
                            string text = page.GetText();
                            float confidence = page.GetMeanConfidence();
                            
                            var result = new OcrResult
                            {
                                Success = true,
                                RawText = text,
                                Confidence = confidence * 100
                            };

                            ParseFields(result);
                            return result;
                        }
                    }
                }
            }
            catch (TypeLoadException)
            {
                return new OcrResult { Success = false, Error = "Tesseract library not installed. Install via NuGet: Install-Package Tesseract" };
            }
            catch (Exception ex)
            {
                return new OcrResult { Success = false, Error = "OCR error: " + ex.Message };
            }
        }

        private static void ParseFields(OcrResult result)
        {
            string text = result.RawText;
            
            var nomorMatch = Regex.Match(text, @"(\d{3,})/([A-Z]{2,})/(?:([IVX]{1,4})/)?(\d{4})", RegexOptions.IgnoreCase);
            if (nomorMatch.Success)
            {
                result.NomorSurat = nomorMatch.Value;
            }

            var tanggalMatch = Regex.Match(text, @"(\d{1,2})[\s\-/](Januari|Februari|Maret|April|Mei|Juni|Juli|Agustus|September|Oktober|November|Desember|\d{1,2})[\s\-/](\d{4})", RegexOptions.IgnoreCase);
            if (tanggalMatch.Success)
            {
                result.Tanggal = tanggalMatch.Value;
            }

            var perihalMatch = Regex.Match(text, @"(?:Perihal|Hal|Perihal|HAL)\s*:\s*(.+?)(?:\r?\n|$)", RegexOptions.IgnoreCase);
            if (perihalMatch.Success)
            {
                result.Perihal = perihalMatch.Groups[1].Value.Trim();
            }

            string[] masukKeywords = { "diterima", "masuk", "dari" };
            string[] keluarKeywords = { "keluar", "kepada", "kepada Yth" };
            
            foreach (var keyword in masukKeywords)
            {
                if (text.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    result.JenisSurat = "Masuk";
                    break;
                }
            }
            
            if (result.JenisSurat == null)
            {
                foreach (var keyword in keluarKeywords)
                {
                    if (text.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        result.JenisSurat = "Keluar";
                        break;
                    }
                }
            }
        }

        public static Color GetConfidenceColor(float confidence)
        {
            if (confidence >= 90) return Color.LightGreen;
            if (confidence >= 70) return Color.Yellow;
            return Color.LightCoral;
        }
    }

    public class OcrResult
    {
        public bool Success { get; set; }
        public string Error { get; set; }
        public string RawText { get; set; }
        public float Confidence { get; set; }
        public string NomorSurat { get; set; }
        public string Tanggal { get; set; }
        public string Perihal { get; set; }
        public string JenisSurat { get; set; }
    }
}
