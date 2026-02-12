# Git Instructions untuk PME DataExchange Project

## Deskripsi Project
Project PME adalah aplikasi console .NET 10 yang berkomunikasi dengan Schneider Electric SOAP Web Service (DataExchange) untuk mendapatkan informasi dari Power Monitoring Expert (PME).

## Struktur Project

```
PME/
├── Connected Services/
│   └── wsdl/
│       ├── Reference.cs          # Auto-generated SOAP client code
│       └── ConnectedService.json # SOAP service configuration
├── Models/
│   ├── WebServiceInfoRequestDto.cs   # DTO untuk request
│   └── WebServiceInfoResponseDto.cs  # DTO untuk response
├── Services/
│   └── DataExchangeService.cs    # Service class untuk SOAP communication
├── Program.cs                     # Main console application
└── PME.csproj                     # Project file

```

## Teknologi yang Digunakan
- **.NET 10** - Framework utama
- **System.ServiceModel** - Untuk SOAP web service communication
- **Connected Services** - Auto-generated SOAP client dari WSDL

## SOAP Service Endpoint
- URL: `http://beitvmpme01.beitm.id/EWS/DataExchange.svc`
- WSDL: `http://beitvmpme01.beitm.id/EWS/DataExchange.svc?singleWsdl`

## Panduan Development

### 1. Clone Repository
```bash
git clone https://github.com/lapengo/TEST-DEBUG-WSL.git
cd TEST-DEBUG-WSL/PME
```

### 2. Build Project
```bash
dotnet build
```

### 3. Run Application
```bash
dotnet run
```

### 4. Clean Build Artifacts
```bash
dotnet clean
```

## Struktur Clean Architecture

### Models Layer
- **WebServiceInfoRequestDto**: DTO untuk request ke SOAP service
- **WebServiceInfoResponseDto**: DTO untuk response dari SOAP service
- **VersionInfo**: Model untuk informasi versi web service
- **SystemInfo**: Model untuk informasi sistem PME

### Services Layer
- **DataExchangeService**: 
  - Menangani komunikasi dengan SOAP service
  - Mendukung HTTP Digest Authentication
  - Mapping dari SOAP objects ke DTOs
  - Implementasi IDisposable untuk proper resource management

### Presentation Layer
- **Program.cs**: Console application yang menampilkan output GetWebServiceInformation

## Configuration

Aplikasi ini menggunakan **appsettings.json** untuk menyimpan semua konfigurasi.

### File appsettings.json

Lokasi: `PME/appsettings.json`

```json
{
  "PmeSettings": {
    "ServiceUrl": "http://beitvmpme01.beitm.id/EWS/DataExchange.svc",
    "Username": "supervisor",
    "Password": "P@ssw0rdpme",
    "Version": "2"
  }
}
```

### Configuration Properties:

| Property | Deskripsi | Default Value |
|----------|-----------|---------------|
| ServiceUrl | URL endpoint SOAP service | http://beitvmpme01.beitm.id/EWS/DataExchange.svc |
| Username | Username untuk autentikasi Digest | supervisor |
| Password | Password untuk autentikasi Digest | P@ssw0rdpme |
| Version | Versi API yang diminta | 2 |

### Cara Mengubah Konfigurasi:

1. Buka file `appsettings.json`
2. Edit nilai yang ingin diubah
3. Save file
4. Run aplikasi - konfigurasi baru akan langsung digunakan

### Keuntungan appsettings.json:
- ✅ **Mudah di-maintenance** - Semua konfigurasi di satu file
- ✅ **Tidak perlu recompile** - Ubah konfigurasi tanpa rebuild aplikasi
- ✅ **Standard .NET approach** - Mengikuti best practice .NET
- ✅ **Environment-specific** - Bisa buat appsettings.Development.json, appsettings.Production.json, dll
- ✅ **Version control friendly** - Mudah track perubahan konfigurasi

### Security Notes:
- ⚠️ Jangan commit appsettings.json yang berisi password production ke Git public repository
- 🔒 Untuk production, gunakan appsettings.Production.json yang di-gitignore
- 🔐 Atau gunakan Azure Key Vault / environment variables untuk sensitive data

## Authentication

Aplikasi ini menggunakan **HTTP Digest Authentication** untuk mengakses SOAP service PME. Credentials dan URL dikonfigurasi di **appsettings.json**.

## Method yang Tersedia

### GetWebServiceInformation
Method ini digunakan untuk mendapatkan informasi tentang web service, termasuk:
- Versi web service (Major & Minor version)
- Daftar operasi yang didukung
- Daftar profil yang didukung
- Informasi sistem (Name, ID, Version)

**Usage:**
```csharp
// Load configuration
var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();

var settings = new PmeSettings();
configuration.GetSection("PmeSettings").Bind(settings);

// Use configuration
using var service = new DataExchangeService(settings.ServiceUrl, settings.Username, settings.Password);
var request = new WebServiceInfoRequestDto { Version = settings.Version };
var response = await service.GetWebServiceInformationAsync(request);
```

## Git Workflow Guidelines

### Branch Naming Convention
- `feature/nama-fitur` - Untuk fitur baru
- `bugfix/nama-bug` - Untuk perbaikan bug
- `hotfix/nama-hotfix` - Untuk perbaikan urgent di production
- `refactor/nama-refactor` - Untuk refactoring code

### Commit Message Convention
Gunakan format yang jelas dan deskriptif:
```
[Type] Short description

Detailed description (optional)

- Change 1
- Change 2
```

**Types:**
- `[FEAT]` - Fitur baru
- `[FIX]` - Bug fix
- `[REFACTOR]` - Code refactoring
- `[DOCS]` - Dokumentasi
- `[TEST]` - Testing
- `[CHORE]` - Maintenance/chores

**Contoh:**
```bash
git commit -m "[FEAT] Implementasi GetWebServiceInformation dengan clean DTO"
git commit -m "[FIX] Perbaikan error handling pada SOAP service call"
git commit -m "[REFACTOR] Simplify mapping logic di DataExchangeService"
```

### Pull Request Guidelines
1. Pastikan code sudah di-build tanpa error
2. Test aplikasi sebelum create PR
3. Berikan deskripsi yang jelas tentang perubahan
4. Reference issue number jika ada

## Best Practices

### 1. Jangan Commit Build Artifacts
File-file berikut sudah ada di `.gitignore`:
- `bin/` folder
- `obj/` folder
- `.vs/` folder
- `*.user` files

### 2. Keep DTOs Clean
- DTOs harus simple dan hanya berisi properties
- Jangan tambahkan business logic di DTOs
- Gunakan nullable types untuk optional fields

### 3. Service Layer Pattern
- Semua komunikasi SOAP harus melalui service class
- Service class harus implement IDisposable
- Gunakan async/await untuk semua SOAP calls

### 4. Error Handling
- Selalu gunakan try-catch untuk SOAP calls
- Berikan error message yang informatif
- Log error untuk debugging

### 5. Code Documentation
- Gunakan XML comments untuk public methods
- Dokumentasikan parameter dan return values
- Berikan contoh usage jika perlu

## Troubleshooting

### Error: "appsettings.json not found"
**Penyebab:** File appsettings.json tidak ada di output directory.

**Solusi:**
1. Pastikan file `appsettings.json` ada di project folder
2. Cek PME.csproj bahwa appsettings.json di-set untuk copy ke output:
   ```xml
   <None Update="appsettings.json">
     <CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>
   </None>
   ```
3. Rebuild project dengan `dotnet build`

### Error: "The HTTP request is unauthorized with client authentication scheme 'Anonymous'"
**Penyebab:** Credentials di appsettings.json tidak valid atau salah.

**Solusi:**
1. Buka `appsettings.json` dan verifikasi username dan password
2. Pastikan credentials yang digunakan memiliki akses ke service
3. Test credentials secara manual jika memungkinkan

### Error: "Could not connect to SOAP service"
- Cek URL di `appsettings.json` sudah benar
- Cek koneksi network ke server yang dikonfigurasi
- Pastikan service sedang running
- Cek firewall settings

### Error: "Build failed"
- Run `dotnet clean`
- Run `dotnet restore`
- Run `dotnet build` lagi

### Error: "The type or namespace name 'wsdl' could not be found"
- Pastikan Reference.cs ada di folder Connected Services
- Rebuild project dengan `dotnet build`

## Catatan Penting

1. **Reference.cs adalah auto-generated file** - Jangan edit manual file ini. Jika perlu update WSDL, regenerate melalui Connected Services di Visual Studio.

2. **Configuration via appsettings.json** - Semua konfigurasi (URL, credentials, version) ada di `appsettings.json`. Edit file tersebut untuk mengubah konfigurasi.

3. **appsettings.json Security**:
   - File ini berisi credentials - jangan commit ke public repository jika berisi password production
   - Untuk production, buat `appsettings.Production.json` dan tambahkan ke `.gitignore`
   - Alternatif: Gunakan environment variables atau Azure Key Vault untuk production

4. **Security Best Practices**:
   - Jangan commit credentials production ke Git
   - Gunakan different appsettings files per environment
   - Untuk CI/CD, inject credentials via pipeline variables

5. **Timeout Settings** - Default timeout bisa di-configure di `DataExchangeClient` jika diperlukan untuk operasi yang lama.

## Kontak & Support

Untuk pertanyaan atau issue, silakan buat issue di GitHub repository atau hubungi team developer.

---
**Last Updated:** 2026-02-12  
**Version:** 1.1 (with Authentication)
