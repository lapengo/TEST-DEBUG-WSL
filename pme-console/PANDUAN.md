# Panduan Penggunaan PME SOAP Service Client

## Cara Tercepat (Rekomendasi)

### 1. Buat file .env

Buka terminal dan jalankan:

```bash
cd pme-console/PME/PME
cp ../../../.env.example .env
```

### 2. Edit file .env (opsional)

File `.env` sudah berisi konfigurasi default yang bisa langsung digunakan:

```
PME_ENDPOINT_URL=http://beitvmpme01.beitm.id/EWS/DataExchange.svc
PME_USERNAME=supervisor
PME_PASSWORD=P@ssw0rdpme
```

Jika perlu mengubah, edit file `.env` dengan text editor:
- Windows: `notepad .env`
- Linux/Mac: `nano .env` atau `vim .env`

### 3. Jalankan aplikasi

```bash
dotnet run
```

**Selesai!** Tidak perlu lagi menulis username, password, atau URL secara manual setiap kali.

## Keuntungan Menggunakan .env File

✅ **Tidak perlu export/set environment variable manual**  
✅ **Konfigurasi tersimpan, tinggal jalankan `dotnet run`**  
✅ **File .env tidak akan di-commit ke git (aman)**  
✅ **Mudah berganti konfigurasi (tinggal edit file .env)**

## Output Aplikasi

Ketika berhasil load konfigurasi dari .env file:

```
PME SOAP Service Client
=======================

Configuration loaded:
  Endpoint: Environment variable (PME_ENDPOINT_URL)
  Username: Environment variable (PME_USERNAME=supervisor)
  Password: Environment variable (PME_PASSWORD=***)

Using endpoint: http://beitvmpme01.beitm.id/EWS/DataExchange.svc
Using authentication: supervisor
```

## Troubleshooting

### Jika masih perlu input manual:

Pastikan file `.env` ada di folder yang benar:
```bash
# Harus di folder ini:
ls pme-console/PME/PME/.env

# Jika tidak ada, copy dari .env.example:
cp pme-console/.env.example pme-console/PME/PME/.env
```

### Jika credential tidak ter-load:

Cek isi file `.env`:
```bash
cat pme-console/PME/PME/.env
```

Pastikan format nya benar (tanpa tanda # di depan):
```
PME_USERNAME=supervisor
PME_PASSWORD=P@ssw0rdpme
```

### Error koneksi "No such host is known":

Error ini normal jika server `beitvmpme01.beitm.id` tidak bisa diakses dari jaringan Anda.
Solusi:
1. Pastikan VPN terhubung (jika diperlukan)
2. Atau ganti endpoint di file `.env` dengan server yang bisa diakses

## Cara Lain (Opsional)

### Menggunakan environment variable manual (cara lama):

```bash
# Windows PowerShell
$env:PME_USERNAME="supervisor"
$env:PME_PASSWORD="P@ssw0rdpme"
dotnet run

# Linux/Mac
export PME_USERNAME=supervisor
export PME_PASSWORD='P@ssw0rdpme'
dotnet run
```

### Menggunakan command line argument (untuk endpoint saja):

```bash
dotnet run -- http://server-lain.com/EWS/DataExchange.svc
```

## Prioritas Konfigurasi

1. Command-line argument (paling tinggi)
2. Environment variable manual
3. File .env (otomatis)
4. Default hardcoded (paling rendah)
