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

## Output Example

```
================================================================================
PME DataExchange SOAP Client - GetWebServiceInformation Demo
================================================================================

Menghubungkan ke SOAP service: http://beitvmpme01.beitm.id/EWS/DataExchange.svc

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
