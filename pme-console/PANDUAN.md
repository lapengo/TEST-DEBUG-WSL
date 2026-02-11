# Panduan Penggunaan PME SOAP Service Client

## Cara Tercepat (Rekomendasi)

### 1. Buat file appsettings.json

Buka terminal dan jalankan:

```bash
cd pme-console/PME/PME
cp appsettings.example.json appsettings.json
```

### 2. Edit file appsettings.json

File `appsettings.json` berisi konfigurasi endpoint dan kredensial:

```json
{
  "PmeService": {
    "EndpointUrl": "http://beitvmpme01.beitm.id/EWS/DataExchange.svc?singleWsdl",
    "Username": "supervisor",
    "Password": "P@ssw0rdpme"
  }
}
```

Jika perlu mengubah, edit file `appsettings.json` dengan text editor:
- Windows: `notepad appsettings.json`
- Linux/Mac: `nano appsettings.json` atau `vim appsettings.json`

### 3. Jalankan aplikasi

```bash
dotnet run
```

**Selesai!** Tidak perlu lagi menulis username, password, atau URL secara manual setiap kali.

## Keuntungan Menggunakan appsettings.json

✅ **Tidak perlu export/set environment variable manual**  
✅ **Konfigurasi tersimpan dalam file JSON yang mudah diedit**  
✅ **File appsettings.json tidak akan di-commit ke git (aman)**  
✅ **Mudah berganti konfigurasi (tinggal edit file appsettings.json)**  
✅ **Menggunakan Digest authentication yang lebih aman**

## Output Aplikasi

Ketika berhasil load konfigurasi dari appsettings.json:

```
PME SOAP Service Client
=======================

Configuration loaded:
  Endpoint: appsettings.json
  Username: supervisor
  Password: ***

Using endpoint: http://beitvmpme01.beitm.id/EWS/DataExchange.svc
Using authentication: supervisor
```

## Troubleshooting

### Jika file appsettings.json tidak ada:

Pastikan file `appsettings.json` ada di folder yang benar:
```bash
# Harus di folder ini:
ls pme-console/PME/PME/appsettings.json

# Jika tidak ada, copy dari appsettings.example.json:
cp pme-console/PME/PME/appsettings.example.json pme-console/PME/PME/appsettings.json
```

### Jika credential tidak ter-load:

Cek isi file `appsettings.json`:
```bash
cat pme-console/PME/PME/appsettings.json
```

Pastikan format nya benar (valid JSON):
```json
{
  "PmeService": {
    "EndpointUrl": "http://beitvmpme01.beitm.id/EWS/DataExchange.svc",
    "Username": "supervisor",
    "Password": "P@ssw0rdpme"
  }
}
```

### Error "The HTTP request is unauthorized with client authentication scheme 'Anonymous'":

Error ini menunjukkan bahwa kredensial tidak dikonfigurasi dengan benar. Aplikasi sekarang menggunakan **Digest authentication** yang lebih aman. Pastikan:
1. File `appsettings.json` ada dan berisi username dan password yang benar
2. Format JSON valid

### Error koneksi "No such host is known":

Error ini normal jika server `beitvmpme01.beitm.id` tidak bisa diakses dari jaringan Anda.
Solusi:
1. Pastikan VPN terhubung (jika diperlukan)
2. Atau ganti endpoint di file `appsettings.json` dengan server yang bisa diakses

## Cara Lain (Opsional)

### Menggunakan command line argument (untuk endpoint saja):

```bash
dotnet run -- http://server-lain.com/EWS/DataExchange.svc
```

## Prioritas Konfigurasi

1. Command-line argument (paling tinggi)
2. File appsettings.json
3. Default hardcoded (paling rendah)

## Keamanan

- File `appsettings.json` **TIDAK** akan di-commit ke git (dilindungi oleh .gitignore)
- Password tidak ditampilkan di output (ditampilkan sebagai ***)
- Aplikasi menggunakan **Digest authentication** (MD5 atau SHA-256) yang lebih aman daripada Basic authentication
- **PERINGATAN**: Endpoint default menggunakan HTTP (bukan HTTPS), data dikirim dalam plaintext
  - Untuk production, pastikan menggunakan HTTPS
  - Atau pastikan server hanya bisa diakses dalam jaringan privat/aman
