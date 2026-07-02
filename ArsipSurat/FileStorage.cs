using System;
using System.IO;

namespace ArsipSurat
{
    public static class FileStorage
    {
        private static readonly string[] AllowedExtensions = { ".pdf", ".jpg", ".jpeg", ".png" };
        private const long MaxFileSize = 10 * 1024 * 1024; // 10MB

        public static string GetBasePath()
        {
            string basePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Arsip");
            if (!Directory.Exists(basePath))
            {
                Directory.CreateDirectory(basePath);
            }
            return basePath;
        }

        public static string GetMonthFolder(DateTime date)
        {
            string folder = Path.Combine(GetBasePath(), date.ToString("yyyy"), date.ToString("MM"));
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            return folder;
        }

        public static bool IsValidFile(string filePath)
        {
            if (!File.Exists(filePath)) return false;
            string ext = Path.GetExtension(filePath).ToLower();
            if (Array.IndexOf(AllowedExtensions, ext) < 0) return false;
            if (new FileInfo(filePath).Length > MaxFileSize) return false;
            return true;
        }

        public static string GetValidationError(string filePath)
        {
            if (!File.Exists(filePath)) return "File tidak ditemukan";
            string ext = Path.GetExtension(filePath).ToLower();
            if (Array.IndexOf(AllowedExtensions, ext) < 0)
                return "Format file tidak didukung. Gunakan: PDF, JPG, PNG";
            if (new FileInfo(filePath).Length > MaxFileSize)
                return "Ukuran file melebihi 10MB";
            return null;
        }

        public static string CopyToStorage(string sourcePath, string nomorSurat, DateTime tanggal)
        {
            string folder = GetMonthFolder(tanggal);
            string ext = Path.GetExtension(sourcePath);
            string fileName = string.Format("{0}_{1}{2}", 
                nomorSurat.Replace("/", "_"), 
                DateTime.Now.ToString("yyyyMMddHHmmss"), 
                ext);
            string destPath = Path.Combine(folder, fileName);
            File.Copy(sourcePath, destPath);
            // Return relative path from BasePath
            return destPath.Substring(GetBasePath().Length).TrimStart(Path.DirectorySeparatorChar);
        }

        public static string GetFullPath(string relativePath)
        {
            return Path.Combine(GetBasePath(), relativePath);
        }

        public static void DeleteFile(string relativePath)
        {
            string fullPath = GetFullPath(relativePath);
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }
        }
    }
}
