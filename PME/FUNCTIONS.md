# PME DataExchange - Available Functions

Aplikasi ini sekarang mendukung semua 22 fungsi WSDL yang tersedia di PME DataExchange service.

## Daftar Fungsi yang Tersedia

### Fungsi yang Sudah Ada Sebelumnya (6 fungsi):
1. **GetWebServiceInformation** - Mendapatkan informasi tentang web service, versi, operasi yang didukung
2. **GetAlarmEventTypes** - Mendapatkan tipe-tipe alarm event yang tersedia
3. **GetEnums** - Mendapatkan daftar enumerasi yang tersedia
4. **GetItems** - Mendapatkan daftar item (value items, history items, alarm items)
5. **GetValues** - Mendapatkan nilai dari item-item tertentu
6. **GetContainerItems** - Mendapatkan item-item dalam container

### Fungsi Baru yang Ditambahkan (16 fungsi):
7. **AcknowledgeAlarmEvents** - Mengakui/acknowledge alarm events
8. **ForceValues** - Memaksa nilai tertentu ke item
9. **GetAlarmEvents** - Mendapatkan alarm events yang aktif
10. **GetAlarmHistory** - Mendapatkan histori alarm events
11. **GetHierarchicalInformation** - Mendapatkan informasi hierarki sistem
12. **GetHistoricalDataAggregation** - Mendapatkan agregasi data historis
13. **GetHistory** - Mendapatkan data historis
14. **GetNotification** - Mendapatkan notifikasi
15. **GetSystemEvents** - Mendapatkan system events
16. **GetSystemEventTypes** - Mendapatkan tipe-tipe system event
17. **GetUpdatedAlarmEvents** - Mendapatkan alarm events yang ter-update
18. **Renew** - Memperbaharui subscription
19. **SetValues** - Mengatur nilai item
20. **Subscribe** - Subscribe ke notifikasi
21. **UnforceValues** - Menghapus forced values
22. **Unsubscribe** - Unsubscribe dari notifikasi

## Cara Menggunakan

1. Jalankan aplikasi:
   ```bash
   dotnet run
   ```

2. Pilih fungsi yang ingin dijalankan dengan memasukkan nomor 1-22
3. Atau pilih 23 untuk menjalankan semua fungsi sekaligus

## Catatan Penting

- Tidak semua fungsi mungkin didukung oleh PME server yang Anda gunakan
- Jika fungsi tidak didukung, aplikasi akan menampilkan pesan "OPERATION_NOT_SUPPORTED"
- Beberapa fungsi memerlukan parameter tambahan (misalnya item IDs) yang perlu dikustomisasi sesuai kebutuhan
- Semua fungsi mengikuti pola yang sama seperti GetWebServiceInformation

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
