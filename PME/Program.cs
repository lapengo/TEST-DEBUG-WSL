using PME.Models;
using PME.Services;
using PME.Helpers;
using Microsoft.Extensions.Configuration;

try
{
    // Build configuration
    var configuration = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .Build();

    // Load settings from appsettings.json
    var settings = new PmeSettings();
    configuration.GetSection("PmeSettings").Bind(settings);

    // Validate required configuration
    if (string.IsNullOrWhiteSpace(settings.ServiceUrl))
        throw new InvalidOperationException("ServiceUrl tidak ditemukan di appsettings.json");
    if (string.IsNullOrWhiteSpace(settings.Username))
        throw new InvalidOperationException("Username tidak ditemukan di appsettings.json");
    if (string.IsNullOrWhiteSpace(settings.Password))
        throw new InvalidOperationException("Password tidak ditemukan di appsettings.json");
    if (string.IsNullOrWhiteSpace(settings.Version))
        throw new InvalidOperationException("Version tidak ditemukan di appsettings.json");

    // Display header
    ConsoleHelper.PrintHeader("PME DataExchange SOAP Client - Demo");
    Console.WriteLine($"Konfigurasi dimuat dari appsettings.json");
    Console.WriteLine();

    try
    {
        Console.WriteLine($"Menghubungkan ke SOAP service: {settings.ServiceUrl}");
        Console.WriteLine($"Username: {settings.Username}");
        Console.WriteLine($"Version: {settings.Version}");
        Console.WriteLine();

        using var dataExchangeService = new DataExchangeService(settings.ServiceUrl, settings.Username, settings.Password);

        // Main menu loop
        bool continueRunning = true;
        while (continueRunning)
        {
            try
            {
                // Menu pilihan
                Console.WriteLine("Pilih operasi yang ingin dijalankan:");
                Console.WriteLine();
                Console.WriteLine("=== OPERASI YANG DIDUKUNG SERVER (✓) ===");
                Console.WriteLine("1. ✓ GetWebServiceInformation (Info dasar server)");
                Console.WriteLine("2. ✓ GetAlarmEventTypes (Tipe alarm)");
                Console.WriteLine("3. ✓ GetContainerItems (Struktur container)");
                Console.WriteLine("4. ✓ GetAlarmEvents (Event alarm aktif)");
                Console.WriteLine("5. ✓ GetUpdatedAlarmEvents (Update alarm)");
                Console.WriteLine("6. ✓ AcknowledgeAlarmEvents (Acknowledge alarm)");
                Console.WriteLine("7. ✓ GetItems (Detail items - butuh IDs)");
                Console.WriteLine("8. ✓ GetValues (Nilai items - butuh IDs)");
                Console.WriteLine("9. ✓ GetHistory (Data historis)");
                Console.WriteLine();
                Console.WriteLine("=== OPERASI TIDAK DIDUKUNG SERVER (✗) ===");
                Console.WriteLine("10. ✗ GetEnums");
                Console.WriteLine("11. ✗ ForceValues");
                Console.WriteLine("12. ✗ GetAlarmHistory");
                Console.WriteLine("13. ✗ GetHierarchicalInformation");
                Console.WriteLine("14. ✗ GetHistoricalDataAggregation");
                Console.WriteLine("15. ✗ GetNotification");
                Console.WriteLine("16. ✗ GetSystemEvents");
                Console.WriteLine("17. ✗ GetSystemEventTypes");
                Console.WriteLine("18. ✗ Renew");
                Console.WriteLine("19. ✗ SetValues");
                Console.WriteLine("20. ✗ Subscribe");
                Console.WriteLine("21. ✗ UnforceValues");
                Console.WriteLine("22. ✗ Unsubscribe");
                Console.WriteLine();
                Console.WriteLine("23. Jalankan SEMUA operasi yang didukung");
                Console.WriteLine("0. Keluar dari program");
                Console.Write("\nPilihan (0-23): ");
                
                var choice = Console.ReadLine();
                Console.WriteLine();

                if (choice == "0")
                {
                    Console.WriteLine("Keluar dari program...");
                    continueRunning = false;
                    break;
                }

                switch (choice)
                {
                    case "1":
                        await RunGetWebServiceInfo(dataExchangeService, settings.Version);
                        break;
                    case "2":
                        await RunGetAlarmEventTypes(dataExchangeService, settings.Version);
                        break;
                    case "3":
                        await RunGetContainerItems(dataExchangeService, settings.Version);
                        break;
                    case "4":
                        await RunGetAlarmEvents(dataExchangeService, settings.Version);
                        break;
                    case "5":
                        await RunGetUpdatedAlarmEvents(dataExchangeService, settings.Version);
                        break;
                    case "6":
                        await RunAcknowledgeAlarmEvents(dataExchangeService, settings.Version);
                        break;
                    case "7":
                        await RunGetItems(dataExchangeService, settings.Version);
                        break;
                    case "8":
                        await RunGetValues(dataExchangeService, settings.Version);
                        break;
                    case "9":
                        await RunGetHistory(dataExchangeService, settings.Version);
                        break;
                    case "10":
                        await RunGetEnums(dataExchangeService, settings.Version);
                        break;
                    case "11":
                        await RunForceValues(dataExchangeService, settings.Version);
                        break;
                    case "12":
                        await RunGetAlarmHistory(dataExchangeService, settings.Version);
                        break;
                    case "13":
                        await RunGetHierarchicalInformation(dataExchangeService, settings.Version);
                        break;
                    case "14":
                        await RunGetHistoricalDataAggregation(dataExchangeService, settings.Version);
                        break;
                    case "15":
                        await RunGetNotification(dataExchangeService, settings.Version);
                        break;
                    case "16":
                        await RunGetSystemEvents(dataExchangeService, settings.Version);
                        break;
                    case "17":
                        await RunGetSystemEventTypes(dataExchangeService, settings.Version);
                        break;
                    case "18":
                        await RunRenew(dataExchangeService, settings.Version);
                        break;
                    case "19":
                        await RunSetValues(dataExchangeService, settings.Version);
                        break;
                    case "20":
                        await RunSubscribe(dataExchangeService, settings.Version);
                        break;
                    case "21":
                        await RunUnforceValues(dataExchangeService, settings.Version);
                        break;
                    case "22":
                        await RunUnsubscribe(dataExchangeService, settings.Version);
                        break;
                    case "23":
                        // Jalankan hanya operasi yang didukung server
                        Console.WriteLine("Menjalankan semua operasi yang DIDUKUNG server...");
                        Console.WriteLine();
                        await RunGetWebServiceInfo(dataExchangeService, settings.Version);
                        Console.WriteLine();
                        await RunGetAlarmEventTypes(dataExchangeService, settings.Version);
                        Console.WriteLine();
                        await RunGetContainerItems(dataExchangeService, settings.Version);
                        Console.WriteLine();
                        await RunGetAlarmEvents(dataExchangeService, settings.Version);
                        Console.WriteLine();
                        await RunGetUpdatedAlarmEvents(dataExchangeService, settings.Version);
                        Console.WriteLine();
                        await RunAcknowledgeAlarmEvents(dataExchangeService, settings.Version);
                        Console.WriteLine();
                        await RunGetItems(dataExchangeService, settings.Version);
                        Console.WriteLine();
                        await RunGetValues(dataExchangeService, settings.Version);
                        Console.WriteLine();
                        await RunGetHistory(dataExchangeService, settings.Version);
                        break;
                    default:
                        Console.WriteLine("Pilihan tidak valid. Silakan pilih 0-23.");
                        break;
                }

                // Setelah operasi selesai, tanya apakah ingin lanjut
                if (choice != "0" && continueRunning)
                {
                    Console.WriteLine();
                    ConsoleHelper.PrintSeparator();
                    Console.WriteLine();
                }
            }
            catch (Exception ex)
            {
                // Handle error dengan opsi untuk lanjut atau keluar
                Console.WriteLine();
                ConsoleHelper.PrintSeparator();
                ConsoleHelper.PrintError("Terjadi error saat menjalankan operasi:");
                Console.WriteLine(ex.Message);
                ConsoleHelper.PrintSeparator();
                Console.WriteLine();
                
                Console.Write("Apakah Anda ingin melanjutkan ke menu? (y/n): ");
                var response = Console.ReadLine()?.ToLower();
                
                if (response != "y" && response != "yes")
                {
                    Console.WriteLine("Keluar dari program...");
                    continueRunning = false;
                }
                else
                {
                    Console.WriteLine();
                    Console.WriteLine("Kembali ke menu utama...");
                    Console.WriteLine();
                }
            }
        }
    }
    catch (Exception ex)
    {
        HandleException(ex, settings);
        Environment.Exit(1);
    }
}
catch (FileNotFoundException)
{
    Console.WriteLine("ERROR: File appsettings.json tidak ditemukan!");
    Console.WriteLine("Pastikan file appsettings.json ada di folder yang sama dengan executable.");
    Environment.Exit(1);
}
catch (InvalidOperationException ex)
{
    Console.WriteLine($"ERROR KONFIGURASI: {ex.Message}");
    Console.WriteLine("Periksa file appsettings.json dan pastikan semua field sudah diisi dengan benar.");
    Environment.Exit(1);
}
catch (Exception ex)
{
    Console.WriteLine($"ERROR: {ex.Message}");
    Environment.Exit(1);
}

Console.WriteLine();
Console.WriteLine("Program selesai. Terima kasih telah menggunakan PME DataExchange Client!");

// Helper methods
static async Task RunGetWebServiceInfo(DataExchangeService dataExchangeService, string version)
{
    var service = new WebServiceInfoService(dataExchangeService);
    await service.ExecuteAsync(version);
}

static async Task RunGetAlarmEventTypes(DataExchangeService dataExchangeService, string version)
{
    var service = new AlarmEventTypesService(dataExchangeService);
    await service.ExecuteAsync(version);
}

static async Task RunGetEnums(DataExchangeService dataExchangeService, string version)
{
    var service = new GetEnumsService(dataExchangeService);
    await service.ExecuteAsync(version);
}

static async Task RunGetItems(DataExchangeService dataExchangeService, string version)
{
    var service = new GetItemsService(dataExchangeService);
    await service.ExecuteAsync(version, null, false);
}

static async Task RunGetValues(DataExchangeService dataExchangeService, string version)
{
    // Example: Get values for specific item IDs
    // You can modify this to accept user input or read from config
    var service = new GetValuesService(dataExchangeService);
    
    // For demo purposes, we'll try to get all items first
    // In real scenario, you would know the specific item IDs
    Console.WriteLine("GetValues memerlukan Item IDs spesifik.");
    Console.WriteLine("Gunakan GetItems terlebih dahulu untuk melihat available items.");
    Console.WriteLine("Untuk demo, operation ini akan di-skip.");
    Console.WriteLine();
    
    // Uncomment below when you have actual item IDs:
    // var itemIds = new List<string> { "item-id-1", "item-id-2" };
    // await service.ExecuteAsync(version, itemIds);
}

static async Task RunGetContainerItems(DataExchangeService dataExchangeService, string version)
{
    var service = new GetContainerItemsService(dataExchangeService);
    await service.ExecuteAsync(version, null, false);
}

static async Task RunAcknowledgeAlarmEvents(DataExchangeService dataExchangeService, string version)
{
    var service = new AcknowledgeAlarmEventsService(dataExchangeService);
    await service.ExecuteAsync(version);
}

static async Task RunForceValues(DataExchangeService dataExchangeService, string version)
{
    var service = new ForceValuesService(dataExchangeService);
    await service.ExecuteAsync(version);
}

static async Task RunGetAlarmEvents(DataExchangeService dataExchangeService, string version)
{
    var service = new GetAlarmEventsService(dataExchangeService);
    await service.ExecuteAsync(version);
}

static async Task RunGetAlarmHistory(DataExchangeService dataExchangeService, string version)
{
    var service = new GetAlarmHistoryService(dataExchangeService);
    await service.ExecuteAsync(version);
}

static async Task RunGetHierarchicalInformation(DataExchangeService dataExchangeService, string version)
{
    var service = new GetHierarchicalInformationService(dataExchangeService);
    await service.ExecuteAsync(version);
}

static async Task RunGetHistoricalDataAggregation(DataExchangeService dataExchangeService, string version)
{
    var service = new GetHistoricalDataAggregationService(dataExchangeService);
    await service.ExecuteAsync(version);
}

static async Task RunGetHistory(DataExchangeService dataExchangeService, string version)
{
    var service = new GetHistoryService(dataExchangeService);
    await service.ExecuteAsync(version);
}

static async Task RunGetNotification(DataExchangeService dataExchangeService, string version)
{
    var service = new GetNotificationService(dataExchangeService);
    await service.ExecuteAsync(version);
}

static async Task RunGetSystemEvents(DataExchangeService dataExchangeService, string version)
{
    var service = new GetSystemEventsService(dataExchangeService);
    await service.ExecuteAsync(version);
}

static async Task RunGetSystemEventTypes(DataExchangeService dataExchangeService, string version)
{
    var service = new GetSystemEventTypesService(dataExchangeService);
    await service.ExecuteAsync(version);
}

static async Task RunGetUpdatedAlarmEvents(DataExchangeService dataExchangeService, string version)
{
    var service = new GetUpdatedAlarmEventsService(dataExchangeService);
    await service.ExecuteAsync(version);
}

static async Task RunRenew(DataExchangeService dataExchangeService, string version)
{
    var service = new RenewService(dataExchangeService);
    await service.ExecuteAsync(version);
}

static async Task RunSetValues(DataExchangeService dataExchangeService, string version)
{
    var service = new SetValuesService(dataExchangeService);
    await service.ExecuteAsync(version);
}

static async Task RunSubscribe(DataExchangeService dataExchangeService, string version)
{
    var service = new SubscribeService(dataExchangeService);
    await service.ExecuteAsync(version);
}

static async Task RunUnforceValues(DataExchangeService dataExchangeService, string version)
{
    var service = new UnforceValuesService(dataExchangeService);
    await service.ExecuteAsync(version);
}

static async Task RunUnsubscribe(DataExchangeService dataExchangeService, string version)
{
    var service = new UnsubscribeService(dataExchangeService);
    await service.ExecuteAsync(version);
}

static void HandleException(Exception ex, PmeSettings settings)
{
    ConsoleHelper.PrintError(ex.Message);
    
    if (ex.InnerException != null)
    {
        Console.WriteLine("Inner Exception:");
        Console.WriteLine(ex.InnerException.Message);
        Console.WriteLine();
    }
    
    // Tambahkan troubleshooting tips untuk connectivity issues
    if (ex.Message.Contains("endpoint") || ex.Message.Contains("listening") || 
        ex.InnerException?.Message.Contains("endpoint") == true || 
        ex.InnerException?.Message.Contains("listening") == true)
    {
        Console.WriteLine("TROUBLESHOOTING TIPS:");
        ConsoleHelper.PrintSeparator();
        Console.WriteLine("1. Pastikan server SOAP service running dan accessible");
        Console.WriteLine($"2. Cek apakah URL di appsettings.json benar: {settings.ServiceUrl}");
        Console.WriteLine("3. Test koneksi dengan: ping atau curl ke server");
        Console.WriteLine("4. Pastikan tidak ada firewall yang memblokir koneksi");
        Console.WriteLine("5. Jika server internal, pastikan Anda terhubung ke VPN/network yang benar");
        Console.WriteLine("6. Coba ubah HTTP ke HTTPS jika service menggunakan SSL");
        Console.WriteLine();
    }
    
    Console.WriteLine("Stack Trace:");
    Console.WriteLine(ex.StackTrace);
    ConsoleHelper.PrintSeparator();
}
