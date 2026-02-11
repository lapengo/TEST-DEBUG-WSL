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
    "EndpointUrl": "http://beitvmpme01.beitm.id/EWS/DataExchange.svc",
    "Username": "supervisor",
    "Password": "P@ssw0rdpme"
  }
}
```

Jika perlu mengubah, edit file `appsettings.json` dengan text editor:
- Windows: `notepad appsettings.json`
- Linux/Mac: `nano appsettings.json` atau `vim appsettings.json`

**Catatan Penting:** URL endpoint harus URL service saja, JANGAN tambahkan `?singleWsdl`. Parameter `?singleWsdl` hanya digunakan untuk mengambil definisi WSDL saat development, bukan untuk pemanggilan SOAP.

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

### ⚠️ Error: "No such host is known" atau "EndpointNotFoundException"

**Error lengkap:**
```
✗ DNS Resolution: FAILED
  Host: beitvmpme01.beitm.id
  Error: No such host is known.

System.ServiceModel.EndpointNotFoundException: 'There was no endpoint listening at 
http://beitvmpme01.beitm.id/EWS/DataExchange.svc that could accept the message.'
```

**Apa artinya:**
- Server `beitvmpme01.beitm.id` **tidak bisa diakses** dari komputer Anda
- DNS tidak bisa menerjemahkan nama server ke alamat IP
- Ini adalah server **internal** yang hanya bisa diakses dari jaringan tertentu

**Solusi:**

1. **CONNECT KE VPN DULU** (Paling Sering) ⭐
   ```bash
   # 1. Nyalakan VPN BEIT/Perusahaan
   # 2. Pastikan status: Connected
   # 3. Test dengan ping:
   ping beitvmpme01.beitm.id
   # 4. Jika berhasil di-ping, jalankan aplikasi lagi
   dotnet run
   ```

2. **Cek Apakah Anda Di Jaringan yang Tepat**
   - Server internal hanya bisa diakses dari:
     - Komputer di kantor
     - Melalui VPN perusahaan
   - TIDAK bisa diakses dari:
     - Internet umum
     - WiFi rumah (tanpa VPN)
     - Hotspot HP

3. **Test Koneksi Manual**
   ```bash
   # Windows (Command Prompt):
   ping beitvmpme01.beitm.id
   nslookup beitvmpme01.beitm.id
   
   # Linux/Mac (Terminal):
   ping beitvmpme01.beitm.id
   nslookup beitvmpme01.beitm.id
   ```
   
   **Hasil yang diharapkan jika BERHASIL:**
   ```
   Reply from 192.168.x.x: bytes=32 time=5ms TTL=64
   ```
   
   **Jika GAGAL:**
   ```
   Ping request could not find host beitvmpme01.beitm.id
   ```
   → Anda belum di jaringan yang tepat atau VPN belum connect

4. **Gunakan IP Address Langsung (Temporary)**
   
   Jika Anda tahu IP address server PME, edit `appsettings.json`:
   ```json
   {
     "PmeService": {
       "EndpointUrl": "http://192.168.1.100/EWS/DataExchange.svc",
       "Username": "supervisor",
       "Password": "P@ssw0rdpme"
     }
   }
   ```

5. **Hubungi IT Support**
   
   Jika semua cara di atas tidak berhasil, hubungi IT/Network Admin dengan info:
   - Lokasi Anda (kantor/remote)
   - Status VPN (connected/disconnected)
   - Hasil ping dan nslookup
   - Screenshot error

**📖 Penjelasan Lengkap:**

Lihat file `PENJELASAN-ERROR.md` untuk:
- Penjelasan detail kenapa error ini terjadi
- Diagram troubleshooting lengkap
- FAQ dan solusi umum
- Contact support

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
