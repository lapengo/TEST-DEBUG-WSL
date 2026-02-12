# PME DataExchange SOAP Client

Aplikasi console .NET 10 untuk berkomunikasi dengan Schneider Electric Power Monitoring Expert (PME) DataExchange SOAP Web Service.

## Fitur Utama

✅ **GetWebServiceInformation** - Mendapatkan informasi tentang web service meliputi:
- Versi web service (Major & Minor version)
- Daftar operasi yang didukung
- Daftar profil yang didukung  
- Informasi sistem (Name, ID, Version)

## Quick Start

### Prerequisites
- .NET 10 SDK
- Akses ke PME server: `http://beitvmpme01.beitm.id/EWS/DataExchange.svc`

### Installation & Run

```bash
# Clone repository
git clone https://github.com/lapengo/TEST-DEBUG-WSL.git
cd TEST-DEBUG-WSL/PME

# Build project
dotnet build

# Run aplikasi
dotnet run
```

### Configuration

Aplikasi ini menggunakan **appsettings.json** untuk konfigurasi. Semua settings sudah dikonfigurasi di file tersebut:

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

**Untuk mengubah konfigurasi:**
1. Edit file `appsettings.json`
2. Ubah nilai ServiceUrl, Username, Password, atau Version sesuai kebutuhan
3. Save file dan run aplikasi

**Keuntungan menggunakan appsettings.json:**
- ✅ Mudah di-maintenance - semua konfigurasi di satu tempat
- ✅ Tidak perlu input manual setiap kali run aplikasi
- ✅ Bisa di-version control atau di-customize per environment
- ✅ Lebih professional dan standard untuk .NET applications

## Output Example

```
================================================================================
PME DataExchange SOAP Client - GetWebServiceInformation Demo
================================================================================

Konfigurasi dimuat dari appsettings.json

Menghubungkan ke SOAP service: http://beitvmpme01.beitm.id/EWS/DataExchange.svc
Username: supervisor
Version: 2

Memanggil GetWebServiceInformation...

================================================================================
HASIL RESPONSE:
================================================================================

INFORMASI VERSI WEB SERVICE:
  Major Version: 2
  Minor Version: 0

Response Version: 1.0

OPERASI YANG DIDUKUNG:
  - GetWebServiceInformation
  - GetContainerItems
  - GetItems
  - GetValues
  - SetValues
  - GetHistory
  - GetAlarmEvents

PROFIL YANG DIDUKUNG:
  - Profile1
  - Profile2

INFORMASI SISTEM:
  Nama   : PME System
  ID     : PME-001
  Versi  : 9.2.0

================================================================================
Berhasil mendapatkan informasi web service!
================================================================================
```

## Struktur Code

```
PME/
├── Models/                    # Data Transfer Objects (DTOs)
│   ├── WebServiceInfoRequestDto.cs
│   └── WebServiceInfoResponseDto.cs
├── Services/                  # Business Logic Layer
│   └── DataExchangeService.cs
├── Connected Services/        # Auto-generated SOAP Client
│   └── wsdl/Reference.cs
└── Program.cs                 # Console Application Entry Point
```

## Clean Architecture

Project ini menggunakan clean architecture dengan pemisahan concerns:

- **Models**: DTOs yang clean dan simple
- **Services**: Business logic dan SOAP communication
- **Presentation**: Console UI untuk display results

## Dokumentasi Lengkap

Lihat [GIT_INSTRUCTIONS.md](./GIT_INSTRUCTIONS.md) untuk:
- Panduan development lengkap
- Git workflow guidelines
- Best practices
- Troubleshooting

## Technologies

- .NET 10
- System.ServiceModel (SOAP/WCF)
- Connected Services (WSDL to C# code generation)

## License

Internal use only - Schneider Electric PME Integration

---

**Author:** Development Team  
**Last Updated:** 2026-02-12
