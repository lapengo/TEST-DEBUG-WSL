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
        Console.WriteLine("3. Jalankan semua");
        Console.Write("\nPilihan (1/2/3): ");
        
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
                await RunGetWebServiceInfo(dataExchangeService, settings.Version);
                Console.WriteLine();
                await RunGetAlarmEventTypes(dataExchangeService, settings.Version);
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
