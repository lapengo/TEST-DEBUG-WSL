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
- Username dan Password untuk autentikasi Digest

### Installation & Run

```bash
# Clone repository
git clone https://github.com/lapengo/TEST-DEBUG-WSL.git
cd TEST-DEBUG-WSL/PME

# Build project
dotnet build

# Run aplikasi (akan meminta username & password)
dotnet run

# Atau set environment variables untuk credentials
export PME_USERNAME="your_username"
export PME_PASSWORD="your_password"
dotnet run
```

### Authentication

Aplikasi ini menggunakan **HTTP Digest Authentication** untuk mengakses SOAP service. Ada 2 cara untuk menyediakan credentials:

1. **Interactive Input** - Aplikasi akan meminta username dan password saat dijalankan
2. **Environment Variables** - Set `PME_USERNAME` dan `PME_PASSWORD` sebelum menjalankan aplikasi

```bash
# Windows (PowerShell)
$env:PME_USERNAME="username"
$env:PME_PASSWORD="password"
dotnet run

# Linux/Mac
export PME_USERNAME="username"
export PME_PASSWORD="password"
dotnet run
```

## Output Example

```
================================================================================
PME DataExchange SOAP Client - GetWebServiceInformation Demo
================================================================================

Masukkan Username: admin
Masukkan Password: ********

Menghubungkan ke SOAP service: http://beitvmpme01.beitm.id/EWS/DataExchange.svc
Username: admin

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
