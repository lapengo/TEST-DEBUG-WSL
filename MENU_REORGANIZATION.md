# Menu Reorganization - Based on Server Capabilities

## Overview
Menu di Program.cs telah direorganisasi berdasarkan operasi yang didukung oleh PME server, sesuai dengan response dari `GetWebServiceInformation`.

## Supported Operations (dari GetWebServiceInformation)

Berdasarkan response GetWebServiceInformation, server mendukung operasi berikut:
- GetWebServiceInformation
- GetAlarmEvents
- GetUpdatedAlarmEvents
- GetAlarmEventTypes
- AcknowledgeAlarmEvents
- GetContainerItems
- GetHistory
- GetItems
- GetValues

## Menu Organization

### Operasi yang Didukung (✓) - Menu 1-9
Operasi-operasi ini disortir berdasarkan kompleksitas parameter (dari yang paling sederhana):

1. **GetWebServiceInformation** - Root function, hanya butuh parameter `version`
2. **GetAlarmEventTypes** - Operasi simple untuk mendapatkan tipe alarm
3. **GetContainerItems** - Mendapatkan struktur container (simple)
4. **GetAlarmEvents** - Mendapatkan alarm events aktif
5. **GetUpdatedAlarmEvents** - Mendapatkan update alarm events
6. **AcknowledgeAlarmEvents** - Acknowledge alarm events
7. **GetItems** - Mendapatkan detail items (memerlukan Item IDs)
8. **GetValues** - Mendapatkan nilai items (memerlukan Item IDs)
9. **GetHistory** - Mendapatkan data historis (parameter lebih kompleks)

### Operasi yang Tidak Didukung (✗) - Menu 10-22
Operasi-operasi ini TIDAK ada dalam daftar supported operations dari GetWebServiceInformation:

10. GetEnums
11. ForceValues
12. GetAlarmHistory
13. GetHierarchicalInformation
14. GetHistoricalDataAggregation
15. GetNotification
16. GetSystemEvents
17. GetSystemEventTypes
18. Renew
19. SetValues
20. Subscribe
21. UnforceValues
22. Unsubscribe

**Catatan:** Operasi-operasi ini kemungkinan akan menghasilkan error `OPERATION_NOT_SUPPORTED` jika dipanggil.

### Menu 23 - Run All Supported
Opsi ini akan menjalankan **hanya operasi yang didukung** (menu 1-9), bukan semua 22 operasi.

## Benefits

1. **Clarity** - User langsung tahu operasi mana yang didukung server
2. **Efficiency** - Operasi yang didukung diprioritaskan di awal menu
3. **Better UX** - User tidak perlu mencoba-coba operasi yang tidak didukung
4. **Sorted by Simplicity** - Operasi simple (parameter sedikit) ada di awal
5. **Visual Indicators** - Simbol ✓ dan ✗ memberikan feedback visual yang jelas

## Usage Recommendation

Untuk penggunaan optimal:
1. Mulai dengan **GetWebServiceInformation** untuk verifikasi koneksi
2. Gunakan **GetContainerItems** untuk eksplorasi struktur data
3. Dapatkan **GetAlarmEventTypes** untuk melihat tipe alarm yang tersedia
4. Gunakan operasi lain sesuai kebutuhan

Hindari menggunakan operasi dengan tanda ✗ kecuali Anda yakin server Anda mendukungnya (mungkin versi PME yang berbeda).
