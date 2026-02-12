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

        // Menu pilihan
        Console.WriteLine("Pilih operasi yang ingin dijalankan:");
        Console.WriteLine("1. GetWebServiceInformation");
        Console.WriteLine("2. GetAlarmEventTypes");
        Console.WriteLine("3. GetEnums");
        Console.WriteLine("4. GetItems");
        Console.WriteLine("5. GetValues");
        Console.WriteLine("6. GetContainerItems");
        Console.WriteLine("7. AcknowledgeAlarmEvents");
        Console.WriteLine("8. ForceValues");
        Console.WriteLine("9. GetAlarmEvents");
        Console.WriteLine("10. GetAlarmHistory");
        Console.WriteLine("11. GetHierarchicalInformation");
        Console.WriteLine("12. GetHistoricalDataAggregation");
        Console.WriteLine("13. GetHistory");
        Console.WriteLine("14. GetNotification");
        Console.WriteLine("15. GetSystemEvents");
        Console.WriteLine("16. GetSystemEventTypes");
        Console.WriteLine("17. GetUpdatedAlarmEvents");
        Console.WriteLine("18. Renew");
        Console.WriteLine("19. SetValues");
        Console.WriteLine("20. Subscribe");
        Console.WriteLine("21. UnforceValues");
        Console.WriteLine("22. Unsubscribe");
        Console.WriteLine("23. Jalankan semua");
        Console.Write("\nPilihan (1-23): ");
        
        var choice = Console.ReadLine();
        Console.WriteLine();

        switch (choice)
        {
            case "1":
                await RunGetWebServiceInfo(dataExchangeService, settings.Version);
                break;
            case "2":
                await RunGetAlarmEventTypes(dataExchangeService, settings.Version);
                break;
            case "3":
                await RunGetEnums(dataExchangeService, settings.Version);
                break;
            case "4":
                await RunGetItems(dataExchangeService, settings.Version);
                break;
            case "5":
                await RunGetValues(dataExchangeService, settings.Version);
                break;
            case "6":
                await RunGetContainerItems(dataExchangeService, settings.Version);
                break;
            case "7":
                await RunAcknowledgeAlarmEvents(dataExchangeService, settings.Version);
                break;
            case "8":
                await RunForceValues(dataExchangeService, settings.Version);
                break;
            case "9":
                await RunGetAlarmEvents(dataExchangeService, settings.Version);
                break;
            case "10":
                await RunGetAlarmHistory(dataExchangeService, settings.Version);
                break;
            case "11":
                await RunGetHierarchicalInformation(dataExchangeService, settings.Version);
                break;
            case "12":
                await RunGetHistoricalDataAggregation(dataExchangeService, settings.Version);
                break;
            case "13":
                await RunGetHistory(dataExchangeService, settings.Version);
                break;
            case "14":
                await RunGetNotification(dataExchangeService, settings.Version);
                break;
            case "15":
                await RunGetSystemEvents(dataExchangeService, settings.Version);
                break;
            case "16":
                await RunGetSystemEventTypes(dataExchangeService, settings.Version);
                break;
            case "17":
                await RunGetUpdatedAlarmEvents(dataExchangeService, settings.Version);
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
                await RunGetWebServiceInfo(dataExchangeService, settings.Version);
                Console.WriteLine();
                await RunGetAlarmEventTypes(dataExchangeService, settings.Version);
                Console.WriteLine();
                await RunGetEnums(dataExchangeService, settings.Version);
                Console.WriteLine();
                await RunGetItems(dataExchangeService, settings.Version);
                Console.WriteLine();
                await RunGetValues(dataExchangeService, settings.Version);
                Console.WriteLine();
                await RunGetContainerItems(dataExchangeService, settings.Version);
                Console.WriteLine();
                await RunAcknowledgeAlarmEvents(dataExchangeService, settings.Version);
                Console.WriteLine();
                await RunForceValues(dataExchangeService, settings.Version);
                Console.WriteLine();
                await RunGetAlarmEvents(dataExchangeService, settings.Version);
                Console.WriteLine();
                await RunGetAlarmHistory(dataExchangeService, settings.Version);
                Console.WriteLine();
                await RunGetHierarchicalInformation(dataExchangeService, settings.Version);
                Console.WriteLine();
                await RunGetHistoricalDataAggregation(dataExchangeService, settings.Version);
                Console.WriteLine();
                await RunGetHistory(dataExchangeService, settings.Version);
                Console.WriteLine();
                await RunGetNotification(dataExchangeService, settings.Version);
                Console.WriteLine();
                await RunGetSystemEvents(dataExchangeService, settings.Version);
                Console.WriteLine();
                await RunGetSystemEventTypes(dataExchangeService, settings.Version);
                Console.WriteLine();
                await RunGetUpdatedAlarmEvents(dataExchangeService, settings.Version);
                Console.WriteLine();
                await RunRenew(dataExchangeService, settings.Version);
                Console.WriteLine();
                await RunSetValues(dataExchangeService, settings.Version);
                Console.WriteLine();
                await RunSubscribe(dataExchangeService, settings.Version);
                Console.WriteLine();
                await RunUnforceValues(dataExchangeService, settings.Version);
                Console.WriteLine();
                await RunUnsubscribe(dataExchangeService, settings.Version);
                break;
            default:
                Console.WriteLine("Pilihan tidak valid. Menjalankan GetWebServiceInformation...");
                await RunGetWebServiceInfo(dataExchangeService, settings.Version);
                break;
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
Console.WriteLine("Tekan sembarang tombol untuk keluar...");
Console.ReadKey();

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
