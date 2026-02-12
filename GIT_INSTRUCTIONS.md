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
  - Mapping dari SOAP objects ke DTOs
  - Implementasi IDisposable untuk proper resource management

### Presentation Layer
- **Program.cs**: Console application yang menampilkan output GetWebServiceInformation

## Method yang Tersedia

### GetWebServiceInformation
Method ini digunakan untuk mendapatkan informasi tentang web service, termasuk:
- Versi web service (Major & Minor version)
- Daftar operasi yang didukung
- Daftar profil yang didukung
- Informasi sistem (Name, ID, Version)

**Usage:**
```csharp
using var service = new DataExchangeService(serviceUrl);
var request = new WebServiceInfoRequestDto { Version = null };
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

### Error: "Could not connect to SOAP service"
- Cek koneksi network ke `beitvmpme01.beitm.id`
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

2. **Endpoint Configuration** - Endpoint SOAP service di-hardcode di `Program.cs`. Untuk production, sebaiknya dipindahkan ke configuration file.

3. **Authentication** - Saat ini tidak ada authentication. Jika service memerlukan credentials, tambahkan di `DataExchangeService` constructor.

4. **Timeout Settings** - Default timeout bisa di-configure di `DataExchangeClient` jika diperlukan untuk operasi yang lama.

## Kontak & Support

Untuk pertanyaan atau issue, silakan buat issue di GitHub repository atau hubungi team developer.

---
**Last Updated:** 2026-02-12
**Version:** 1.0
