# PME DataExchange - Available Functions

Aplikasi ini sekarang mendukung semua 22 fungsi WSDL yang tersedia di PME DataExchange service.

⚠️ **PENTING:** Menu telah direorganisasi berdasarkan operasi yang didukung oleh server Anda!

## Organisasi Menu Baru

Menu sekarang dibagi menjadi 2 kategori berdasarkan response dari `GetWebServiceInformation`:

### ✓ Operasi yang DIDUKUNG Server (Menu 1-9)
Operasi-operasi ini telah dikonfirmasi didukung oleh PME server berdasarkan GetWebServiceInformation response:

1. **GetWebServiceInformation** ✓ - Mendapatkan informasi tentang web service, versi, operasi yang didukung (ROOT function)
2. **GetAlarmEventTypes** ✓ - Mendapatkan tipe-tipe alarm event yang tersedia
3. **GetContainerItems** ✓ - Mendapatkan item-item dalam container (recommended untuk eksplorasi)
4. **GetAlarmEvents** ✓ - Mendapatkan alarm events yang aktif
5. **GetUpdatedAlarmEvents** ✓ - Mendapatkan alarm events yang ter-update
6. **AcknowledgeAlarmEvents** ✓ - Mengakui/acknowledge alarm events
7. **GetItems** ✓ - Mendapatkan daftar item (butuh Item IDs - lihat KNOWN_LIMITATIONS.md)
8. **GetValues** ✓ - Mendapatkan nilai dari item-item tertentu (butuh Item IDs)
9. **GetHistory** ✓ - Mendapatkan data historis

### ✗ Operasi yang TIDAK DIDUKUNG Server (Menu 10-22)
Operasi-operasi ini TIDAK ada dalam daftar supported operations dari server Anda dan kemungkinan akan menghasilkan error `OPERATION_NOT_SUPPORTED`:

10. **GetEnums** ✗
11. **ForceValues** ✗
12. **GetAlarmHistory** ✗
13. **GetHierarchicalInformation** ✗
14. **GetHistoricalDataAggregation** ✗
15. **GetNotification** ✗
16. **GetSystemEvents** ✗
17. **GetSystemEventTypes** ✗
18. **Renew** ✗
19. **SetValues** ✗
20. **Subscribe** ✗
21. **UnforceValues** ✗
22. **Unsubscribe** ✗

**Menu 23** - Jalankan SEMUA operasi yang didukung (hanya menu 1-9)

## Cara Menggunakan

1. Jalankan aplikasi:
   ```bash
   dotnet run
   ```

2. Pilih fungsi yang ingin dijalankan dengan memasukkan nomor 1-23
   - **Fokus pada menu 1-9** untuk operasi yang dijamin berfungsi
   - Menu 10-22 tersedia untuk testing, tapi kemungkinan tidak didukung
3. Atau pilih 23 untuk menjalankan semua fungsi sekaligus

## Catatan Penting

1. **Operasi yang Memerlukan Parameter Spesifik:**
   - **GetItems** (option 4): Memerlukan daftar Item IDs. Tanpa IDs, akan menampilkan pesan error informatif. Gunakan GetContainerItems terlebih dahulu untuk melihat item yang tersedia.
   - **GetValues** (option 5): Memerlukan Item IDs yang spesifik.
   - Lihat `KNOWN_LIMITATIONS.md` untuk detail dan cara kerja alternatif.

2. **Dukungan Server:** Tidak semua fungsi mungkin didukung oleh PME server yang Anda gunakan
3. **Error Handling:** Jika fungsi tidak didukung, aplikasi akan menampilkan pesan "OPERATION_NOT_SUPPORTED"
4. **Parameter Tambahan:** Beberapa fungsi memerlukan parameter tambahan yang perlu dikustomisasi sesuai kebutuhan
5. **Consistency:** Semua fungsi mengikuti pola yang sama seperti GetWebServiceInformation

## Struktur Kode

Setiap fungsi memiliki:
- **Service Class** di folder `Services/` (contoh: `GetWebServiceInformationService.cs`)
- **Helper Method** di `Program.cs` (contoh: `RunGetWebServiceInfo()`)
- **Error Handling** untuk menangani operasi yang tidak didukung
- **Display Output** untuk menampilkan hasil ke console

## Pengembangan Lebih Lanjut

Jika Anda ingin menambahkan fungsionalitas lebih detail untuk fungsi tertentu:
1. Edit service class yang sesuai di folder `Services/`
2. Tambahkan parameter yang diperlukan
3. Tambahkan display logic di `DisplayHelper.cs` jika perlu
4. Update model DTO jika diperlukan

## Testing

Untuk menguji semua fungsi sekaligus, pilih opsi 23 di menu. Ini akan menjalankan semua 22 fungsi secara berurutan dan menampilkan hasilnya.
