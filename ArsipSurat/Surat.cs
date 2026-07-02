using System;

namespace ArsipSurat
{
    public class Surat
    {
        public int Id { get; set; }
        public string NomorSurat { get; set; }
        public DateTime TanggalSurat { get; set; }
        public string JenisSurat { get; set; }
        public string Pengirim { get; set; }
        public string Penerima { get; set; }
        public string Perihal { get; set; }
        public string Status { get; set; }
        public string Keterangan { get; set; }
        public bool IsOcrProcessed { get; set; }
        public decimal? OcrConfidence { get; set; }
        public string OcrRawText { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool IsDeleted { get; set; }
    }
}
