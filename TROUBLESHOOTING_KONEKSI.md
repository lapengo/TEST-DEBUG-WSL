# Troubleshooting Koneksi SOAP Service

## Error: "There was no endpoint listening..."

### Gejala
```
Error saat memanggil GetWebServiceInformation: There was no endpoint listening at http://beitvmpme01.beitm.id/EWS/DataExchange.svc
```

### Penyebab Umum

#### 1. Server Tidak Accessible (DNS/Network Issue)
**Indikasi:**
- Error message menyebutkan "could not accept the message"
- Server name tidak bisa di-resolve

**Solusi:**
```bash
# Test DNS resolution
ping beitvmpme01.beitm.id

# Test HTTP connectivity
curl -I http://beitvmpme01.beitm.id/EWS/DataExchange.svc

# Atau gunakan browser untuk test
```

**Jika ping/curl gagal:**
- Server adalah internal server, pastikan terhubung ke VPN atau network yang benar
- Cek apakah server name benar di appsettings.json
- Hubungi network admin untuk memastikan server running

#### 2. URL Salah atau Service Port Berbeda
**Solusi:**
- Verifikasi URL yang benar dari team yang maintain service
- Cek apakah menggunakan HTTP atau HTTPS
- Cek port number jika ada (contoh: :8080)

Edit `appsettings.json`:
```json
{
  "PmeSettings": {
    "ServiceUrl": "https://beitvmpme01.beitm.id/EWS/DataExchange.svc",  // Coba HTTPS
    ...
  }
}
```

#### 3. Firewall atau Proxy Memblokir Koneksi
**Solusi:**
- Cek Windows Firewall atau antivirus
- Cek corporate proxy settings
- Tambahkan exception untuk aplikasi ini

#### 4. Service Tidak Running
**Solusi:**
- Hubungi server administrator
- Cek status service di server
- Verifikasi bahwa IIS/service host sedang running

#### 5. Binding/Endpoint Configuration Salah di Server
**Solusi:**
- Verifikasi bahwa service di-host di URL yang benar
- Cek IIS bindings jika menggunakan IIS
- Verifikasi WSDL accessible di: http://beitvmpme01.beitm.id/EWS/DataExchange.svc?wsdl

---

## Testing Koneksi

### 1. Test DNS Resolution
```bash
# Windows
nslookup beitvmpme01.beitm.id

# Linux/Mac
dig beitvmpme01.beitm.id
```

### 2. Test HTTP Connectivity
```bash
# Test dengan curl
curl -v http://beitvmpme01.beitm.id/EWS/DataExchange.svc

# Test WSDL
curl http://beitvmpme01.beitm.id/EWS/DataExchange.svc?wsdl
```

### 3. Test dengan Browser
Buka di browser:
- `http://beitvmpme01.beitm.id/EWS/DataExchange.svc` - Harus menampilkan service page
- `http://beitvmpme01.beitm.id/EWS/DataExchange.svc?wsdl` - Harus menampilkan WSDL XML

---

## Checklist Troubleshooting

- [ ] Server name bisa di-resolve (ping/nslookup berhasil)
- [ ] Port terbuka dan accessible (curl berhasil)
- [ ] URL di appsettings.json benar (tidak ada typo)
- [ ] Protocol benar (HTTP vs HTTPS)
- [ ] Connected ke network/VPN yang benar (jika internal server)
- [ ] Firewall tidak memblokir koneksi
- [ ] Service sedang running di server
- [ ] Credentials di appsettings.json benar
- [ ] WSDL accessible via browser

---

## Solusi untuk Development/Testing

Jika server internal tidak accessible dari environment saat ini:

### Option 1: Gunakan Mock Service
Buat local mock SOAP service untuk testing

### Option 2: Update hosts file (temporary)
Jika IP address diketahui:

**Windows:** `C:\Windows\System32\drivers\etc\hosts`
**Linux/Mac:** `/etc/hosts`

Tambahkan:
```
192.168.1.100  beitvmpme01.beitm.id
```

### Option 3: VPN/Network Access
Hubungkan ke VPN atau network yang sama dengan server

### Option 4: Contact Server Admin
Minta akses atau alternative endpoint yang accessible

---

## Error Messages dan Solusinya

| Error Message | Penyebab | Solusi |
|---------------|----------|--------|
| Could not resolve host | DNS tidak bisa resolve hostname | Cek DNS, hosts file, atau VPN |
| Connection refused | Service tidak listening di port | Cek service running, cek port |
| Connection timeout | Network tidak bisa reach server | Cek firewall, network, proxy |
| 404 Not Found | URL endpoint salah | Verifikasi URL yang benar |
| 401 Unauthorized | Credentials salah | Cek username/password |
| 500 Internal Server Error | Server-side error | Hubungi server admin |

---

## Untuk Production Deployment

1. **Gunakan environment-specific config:**
   ```json
   // appsettings.Production.json
   {
     "PmeSettings": {
       "ServiceUrl": "https://production-server.company.com/EWS/DataExchange.svc",
       ...
     }
   }
   ```

2. **Health Check:**
   Implementasi health check sebelum memanggil service

3. **Retry Logic:**
   Tambahkan retry mechanism untuk transient failures

4. **Logging:**
   Log semua connection attempts untuk troubleshooting

---

## Kontak Support

Jika masalah masih berlanjut:
1. Screenshot error message lengkap
2. Hasil test ping/curl
3. Network configuration (VPN status, etc)
4. Contact: [Your IT Support Contact]
