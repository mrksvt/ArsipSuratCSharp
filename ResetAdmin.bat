@echo off
chcp 65001 > nul
echo ==============================================
echo   Arsip Surat - Reset Password ^& Email Admin
echo ==============================================
echo.

REM Cari csc.exe di lokasi standar .NET Framework
set CSC_PATH=
if exist "C:\Windows\Microsoft.NET\Framework\v4.0.30319\csc.exe" (
    set CSC_PATH=C:\Windows\Microsoft.NET\Framework\v4.0.30319\csc.exe
)
if exist "C:\Windows\Microsoft.NET\Framework64\v4.0.30319\csc.exe" (
    set CSC_PATH=C:\Windows\Microsoft.NET\Framework64\v4.0.30319\csc.exe
)

if "%CSC_PATH%"=="" (
    echo [ERROR] C# Compiler (csc.exe) tidak ditemukan.
    echo Pastikan .NET Framework 4.x terinstall.
    echo.
    pause
    exit /b 1
)

echo [INFO] Compiler ditemukan: %CSC_PATH%
echo [INFO] Kompilasi ResetAdmin.cs...
echo.

REM Compile ResetAdmin.cs
"%CSC_PATH%" /nologo /out:ResetAdmin.exe ResetAdmin.cs

if errorlevel 1 (
    echo.
    echo [ERROR] Kompilasi gagal. Periksa ResetAdmin.cs.
    echo.
    pause
    exit /b 1
)

echo [INFO] Kompilasi berhasil.
echo [INFO] Menjalankan ResetAdmin.exe...
echo.

REM Run ResetAdmin.exe
ResetAdmin.exe

if errorlevel 1 (
    echo.
    echo [ERROR] Eksekusi gagal.
    echo.
    pause
    exit /b 1
)

echo.
echo [INFO] Selesai. File reset_admin.sql telah dibuat.
echo.
echo LANGKAH SELANJUTNYA:
echo 1. Pastikan MySQL Server sedang berjalan
echo 2. Jalankan: mysql -u root -p ^< reset_admin.sql
echo 3. Login dengan username: admin, password: admin123
echo.
pause
