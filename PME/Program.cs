using PME.Models;
using PME.Services;
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

    Console.WriteLine("=".PadRight(80, '='));
    Console.WriteLine("PME DataExchange SOAP Client - GetWebServiceInformation Demo");
    Console.WriteLine("=".PadRight(80, '='));
    Console.WriteLine();
    Console.WriteLine($"Konfigurasi dimuat dari appsettings.json");
    Console.WriteLine();

try
{
    Console.WriteLine($"Menghubungkan ke SOAP service: {settings.ServiceUrl}");
    Console.WriteLine($"Username: {settings.Username}");
    Console.WriteLine($"Version: {settings.Version}");
    Console.WriteLine();

    using var service = new DataExchangeService(settings.ServiceUrl, settings.Username, settings.Password);

    // Buat request dengan versi dari appsettings.json
    var request = new WebServiceInfoRequestDto
    {
        Version = settings.Version
    };

    Console.WriteLine("Memanggil GetWebServiceInformation...");
    var response = await service.GetWebServiceInformationAsync(request);

    Console.WriteLine();
    Console.WriteLine("=".PadRight(80, '='));
    Console.WriteLine("HASIL RESPONSE:");
    Console.WriteLine("=".PadRight(80, '='));
    Console.WriteLine();

    // Tampilkan informasi versi
    if (response.Version != null)
    {
        Console.WriteLine("INFORMASI VERSI WEB SERVICE:");
        Console.WriteLine($"  Major Version: {response.Version.MajorVersion ?? "N/A"}");
        Console.WriteLine($"  Minor Version: {response.Version.MinorVersion ?? "N/A"}");
        Console.WriteLine();
    }

    // Tampilkan response version
    if (!string.IsNullOrEmpty(response.ResponseVersion))
    {
        Console.WriteLine($"Response Version: {response.ResponseVersion}");
        Console.WriteLine();
    }

    // Tampilkan operasi yang didukung
    if (response.SupportedOperations.Any())
    {
        Console.WriteLine("OPERASI YANG DIDUKUNG:");
        foreach (var operation in response.SupportedOperations)
        {
            Console.WriteLine($"  - {operation}");
        }
        Console.WriteLine();
    }
    else
    {
        Console.WriteLine("OPERASI YANG DIDUKUNG: Tidak ada data");
        Console.WriteLine();
    }

    // Tampilkan profil yang didukung
    if (response.SupportedProfiles.Any())
    {
        Console.WriteLine("PROFIL YANG DIDUKUNG:");
        foreach (var profile in response.SupportedProfiles)
        {
            Console.WriteLine($"  - {profile}");
        }
        Console.WriteLine();
    }
    else
    {
        Console.WriteLine("PROFIL YANG DIDUKUNG: Tidak ada data");
        Console.WriteLine();
    }

    // Tampilkan informasi sistem
    if (response.System != null)
    {
        Console.WriteLine("INFORMASI SISTEM:");
        Console.WriteLine($"  Nama   : {response.System.Name ?? "N/A"}");
        Console.WriteLine($"  ID     : {response.System.Id ?? "N/A"}");
        Console.WriteLine($"  Versi  : {response.System.Version ?? "N/A"}");
        Console.WriteLine();
    }

    Console.WriteLine("=".PadRight(80, '='));
    Console.WriteLine("Berhasil mendapatkan informasi web service!");
    Console.WriteLine("=".PadRight(80, '='));
}
catch (Exception ex)
{
    Console.WriteLine();
    Console.WriteLine("=".PadRight(80, '='));
    Console.WriteLine("ERROR:");
    Console.WriteLine("=".PadRight(80, '='));
    Console.WriteLine($"Message: {ex.Message}");
    Console.WriteLine();
    
    if (ex.InnerException != null)
    {
        Console.WriteLine("Inner Exception:");
        Console.WriteLine(ex.InnerException.Message);
        Console.WriteLine();
    }
    
    Console.WriteLine("Stack Trace:");
    Console.WriteLine(ex.StackTrace);
    Console.WriteLine("=".PadRight(80, '='));
    
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
