# PME DataExchange SOAP Client

Aplikasi console .NET 10 untuk berkomunikasi dengan Schneider Electric Power Monitoring Expert (PME) DataExchange SOAP Web Service.

## Fitur Utama

✅ **GetWebServiceInformation** - Mendapatkan informasi tentang web service meliputi:
- Versi web service (Major & Minor version)
- Daftar operasi yang didukung
- Daftar profil yang didukung  
- Informasi sistem (Name, ID, Version)

✅ **GetAlarmEventTypes** - Mendapatkan daftar alarm event types yang didukung oleh service

✅ **GetEnums** - Mendapatkan enumerations dengan values:
- List of enums dengan ID dan Name
- Enum values (value-text pairs)
- Error results jika ada

✅ **Clean Architecture** - Kode ter-refactor dengan separation of concerns:
- Helper layer untuk reusable utilities
- Service layer untuk business logic
- Model layer untuk DTOs

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

## Menu Interaktif

Aplikasi sekarang memiliki menu interaktif untuk memilih operasi:

```
Pilih operasi yang ingin dijalankan:
1. GetWebServiceInformation
2. GetAlarmEventTypes
3. GetEnums
4. Jalankan semua

Pilihan (1/2/3/4):
```

## Output Example

### GetWebServiceInformation
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

### GetAlarmEventTypes
```
================================================================================
PME DataExchange SOAP Client - Demo
================================================================================

Konfigurasi dimuat dari appsettings.json

Menghubungkan ke SOAP service: http://beitvmpme01.beitm.id/EWS/DataExchange.svc
Username: supervisor
Version: 2

Pilih operasi yang ingin dijalankan:
1. GetWebServiceInformation
2. GetAlarmEventTypes
3. Jalankan semua

Pilihan (1/2/3/4): 2

Memanggil GetAlarmEventTypes...

================================================================================
HASIL RESPONSE:
================================================================================

Response Version: 2

ALARM EVENT TYPES:
  - HighAlarm
  - LowAlarm
  - DeviceFailure
  - CommunicationLost
  - SystemError

================================================================================
Berhasil mendapatkan alarm event types!
================================================================================
```

### GetEnums
```
================================================================================
PME DataExchange SOAP Client - Demo
================================================================================

Konfigurasi dimuat dari appsettings.json

Menghubungkan ke SOAP service: http://beitvmpme01.beitm.id/EWS/DataExchange.svc
Username: supervisor
Version: 2

Pilih operasi yang ingin dijalankan:
1. GetWebServiceInformation
2. GetAlarmEventTypes
3. GetEnums
4. Jalankan semua

Pilihan (1/2/3/4): 3

Memanggil GetEnums...

================================================================================
HASIL RESPONSE:
================================================================================

Response Version: 2

ENUMS (3 enums):

  Enum: AlarmPriority
    ID: alarm-priority-enum
    Values (4):
      • 1 = Low
      • 2 = Medium
      • 3 = High
      • 4 = Critical

  Enum: DeviceStatus
    ID: device-status-enum
    Values (3):
      • 0 = Offline
      • 1 = Online
      • 2 = Error

  Enum: MeasurementUnit
    ID: measurement-unit-enum
    Values (5):
      • kW = Kilowatt
      • kWh = Kilowatt Hour
      • A = Ampere
      • V = Volt
      • Hz = Hertz

================================================================================
Berhasil mendapatkan enums!
================================================================================
```

## Struktur Code (Clean Architecture)

```
PME/
├── Helpers/                    # Helper layer - reusable utilities
│   ├── ConsoleHelper.cs        # Console formatting methods
│   └── DisplayHelper.cs        # Display logic for data types
│
├── Models/                     # Data Transfer Objects (DTOs)
│   ├── PmeSettings.cs
│   ├── WebServiceInfoRequestDto.cs
│   ├── WebServiceInfoResponseDto.cs
│   ├── AlarmEventTypesRequestDto.cs
│   ├── AlarmEventTypesResponseDto.cs
│   ├── GetEnumsRequestDto.cs
│   └── GetEnumsResponseDto.cs
│
├── Services/                   # Business Logic Layer
│   ├── DataExchangeService.cs      # SOAP client wrapper
│   ├── WebServiceInfoService.cs    # GetWebServiceInformation logic
│   ├── AlarmEventTypesService.cs   # GetAlarmEventTypes logic
│   └── GetEnumsService.cs          # GetEnums logic
│
├── Connected Services/         # Auto-generated SOAP Client
│   └── wsdl/Reference.cs
│
└── Program.cs                  # Console Application Entry Point
```

## Clean Architecture

Project ini menggunakan clean architecture dengan pemisahan concerns:

- **Helpers**: Reusable utility methods (console formatting, display)
- **Models**: DTOs yang clean dan simple
- **Services**: Business logic untuk setiap SOAP operation
- **Presentation**: Console UI untuk orchestration dan user interaction

### Adding New SOAP Operation

1. Create Request/Response DTOs in `Models/`
2. (Optional) Add display method in `DisplayHelper`
3. Create service class in `Services/`
4. Add menu option in `Program.cs`

See [ARCHITECTURE_REFACTORING.md](./ARCHITECTURE_REFACTORING.md) for detailed guide.
## Dokumentasi Lengkap

Lihat [GIT_INSTRUCTIONS.md](./GIT_INSTRUCTIONS.md) untuk:
- Panduan development lengkap
- Git workflow guidelines
- Best practices
- Troubleshooting

**Architecture Documentation:**
- [ARCHITECTURE_REFACTORING.md](./ARCHITECTURE_REFACTORING.md) - Panduan lengkap clean architecture dan cara menambah SOAP operation baru

**Troubleshooting Koneksi:**
Jika mengalami error koneksi ke SOAP service, lihat [TROUBLESHOOTING_KONEKSI.md](./TROUBLESHOOTING_KONEKSI.md) untuk panduan lengkap mengatasi masalah network connectivity.

## Technologies

- .NET 10
- System.ServiceModel (SOAP/WCF) dengan HTTP Digest Authentication
- Microsoft.Extensions.Configuration (Configuration management)
- Connected Services (WSDL to C# code generation)
- Clean Architecture (Layered design pattern)

## License

Internal use only - Schneider Electric PME Integration

---

**Author:** Development Team  
**Last Updated:** 2026-02-12  
**Version:** 1.3.0 (Service Layers Architecture)
