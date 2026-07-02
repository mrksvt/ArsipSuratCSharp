# Panduan Screenshot untuk README.md

Folder ini berisi screenshot aplikasi Arsip Surat yang digunakan di README.md.

## Screenshot yang Diperlukan

Ambil screenshot berikut dengan resolusi minimal 1280x720px, format PNG:

1. **login.png** — LoginForm dengan field username/password
2. **dashboard.png** — Dashboard dengan 4 kartu statistik + tabel 10 surat terakhir
3. **arsip-list.png** — Arsip Surat dengan DataGridView, toolbar, search, filters
4. **arsip-input.png** — SuratInputForm dengan semua field + OCR checkbox
5. **nomor-surat.png** — Nomor Surat panel dengan form generator
6. **master-data.png** — Master Data Departemen dengan grid + toolbar
7. **pengaturan-password.png** — Pengaturan panel bagian Ganti Password
8. **pengaturan-2fa.png** — Pengaturan panel bagian 2FA dengan TOTP setup/status
9. **2fa-totp-setup.png** — QR code + secret key saat setup TOTP
10. **2fa-totp-active.png** — Status "✓ AKTIF" setelah TOTP diaktifkan

## Cara Mengambil Screenshot

1. Jalankan aplikasi
2. Login dengan akun admin
3. Navigasi ke setiap menu/panel
4. Tekan `Alt + PrtScr` untuk screenshot window aktif
5. Paste di Paint/editor gambar
6. Save dengan nama sesuai list di atas ke folder `screenshots\`
7. Pastikan tidak ada data sensitif/personal dalam screenshot

## Catatan

- File .gitignore sudah mengexclude folder screenshots (tidak di-commit)
- Gunakan data dummy/contoh saat screenshot
- Semua screenshot harus dalam kondisi window maximized untuk konsistensi
