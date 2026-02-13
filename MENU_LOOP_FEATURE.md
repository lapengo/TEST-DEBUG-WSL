# Menu Loop Feature

## Overview
Program sekarang mendukung menu loop, memungkinkan user untuk menjalankan beberapa operasi tanpa perlu restart aplikasi.

## Fitur Baru

### 1. Menu Loop
- Setelah menjalankan operasi, program otomatis kembali ke menu
- User bisa memilih operasi lain tanpa keluar
- Tidak perlu restart aplikasi untuk operasi berbeda

### 2. Opsi Exit
- Menu baru: **"0. Keluar dari program"**
- User bisa keluar kapan saja dengan memilih opsi 0
- Program akan tertutup dengan rapi

### 3. Error Handling dengan Pilihan
Jika terjadi error saat menjalankan operasi:
- Error ditampilkan dengan jelas
- User ditanya: "Apakah Anda ingin melanjutkan ke menu? (y/n)"
- **y/yes**: Kembali ke menu untuk mencoba operasi lain
- **n/no**: Keluar dari program

## Cara Kerja

### Flow Normal (Tanpa Error)
```
┌─────────────────────────┐
│   Display Menu          │
└──────────┬──────────────┘
           │
           ▼
┌─────────────────────────┐
│   User memilih operasi  │
└──────────┬──────────────┘
           │
           ▼
┌─────────────────────────┐
│   Jalankan operasi      │
└──────────┬──────────────┘
           │
           ▼
┌─────────────────────────┐
│   Operasi selesai       │
└──────────┬──────────────┘
           │
           ▼
┌─────────────────────────┐
│   Kembali ke menu       │◄──┐
└─────────────────────────┘   │
           │                   │
           └───────────────────┘
           (Loop terus sampai user pilih "0")
```

### Flow dengan Error
```
┌─────────────────────────┐
│   Jalankan operasi      │
└──────────┬──────────────┘
           │
           ▼
┌─────────────────────────┐
│   ERROR terjadi         │
└──────────┬──────────────┘
           │
           ▼
┌─────────────────────────┐
│   Tampilkan error       │
└──────────┬──────────────┘
           │
           ▼
┌─────────────────────────┐
│ Lanjut ke menu? (y/n)   │
└──────────┬──────────────┘
           │
      ┌────┴────┐
      │         │
      ▼         ▼
    [y]       [n]
      │         │
      ▼         ▼
  ┌──────┐  ┌──────┐
  │Menu  │  │Exit  │
  └──────┘  └──────┘
```

## Contoh Penggunaan

### Skenario 1: User menjalankan beberapa operasi
```
1. User memilih "1" (GetWebServiceInformation)
   → Operasi berjalan sukses
   → Kembali ke menu

2. User memilih "3" (GetContainerItems)
   → Operasi berjalan sukses
   → Kembali ke menu

3. User memilih "0" (Keluar)
   → Program selesai
```

### Skenario 2: Terjadi error
```
1. User memilih "7" (GetItems - butuh IDs)
   → Error: MISSING_ID_LIST
   → Prompt: "Apakah Anda ingin melanjutkan ke menu? (y/n)"
   
2. User ketik "y"
   → Kembali ke menu

3. User memilih "3" (GetContainerItems)
   → Operasi berjalan sukses
   → Kembali ke menu

4. User memilih "0"
   → Program selesai
```

### Skenario 3: Error dan keluar
```
1. User memilih operasi yang error
   → Error ditampilkan
   → Prompt: "Apakah Anda ingin melanjutkan ke menu? (y/n)"

2. User ketik "n"
   → Program keluar
```

## Keuntungan

1. **Efisiensi**: Tidak perlu restart aplikasi untuk setiap operasi
2. **Eksplorasi**: User bisa dengan mudah mencoba berbagai operasi
3. **Error Handling**: Error tidak langsung menutup aplikasi
4. **User Control**: User yang kontrol kapan keluar dari program
5. **Testing**: Lebih mudah untuk testing berbagai operasi

## Technical Implementation

### Main Loop
```csharp
bool continueRunning = true;
while (continueRunning)
{
    try
    {
        // Display menu
        // Get user choice
        // Execute operation
        // Loop back
    }
    catch (Exception ex)
    {
        // Show error
        // Ask user if want to continue
        // Continue or exit based on response
    }
}
```

### Exit Mechanism
- Opsi "0" mengubah `continueRunning = false`
- Loop berhenti dan program selesai dengan rapi
- No force termination dengan `Environment.Exit()` pada flow normal

## Breaking Changes
**TIDAK ADA** - Ini adalah enhancement yang backward compatible:
- Semua operasi existing tetap berfungsi sama
- Hanya menambah fitur loop dan exit option
- User experience improved tanpa breaking functionality
