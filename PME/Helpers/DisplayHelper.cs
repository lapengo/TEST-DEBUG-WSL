using PME.Models;

namespace PME.Helpers;

/// <summary>
/// Helper class untuk display data ke console
/// </summary>
public static class DisplayHelper
{
    /// <summary>
    /// Display WebServiceInfo response
    /// </summary>
    public static void DisplayWebServiceInfo(WebServiceInfoResponseDto response)
    {
        ConsoleHelper.PrintSeparator();
        Console.WriteLine("HASIL RESPONSE:");
        ConsoleHelper.PrintSeparator();
        Console.WriteLine();

        // Tampilkan informasi versi
        if (response.Version != null)
        {
            ConsoleHelper.PrintSectionHeader("INFORMASI VERSI WEB SERVICE:");
            ConsoleHelper.PrintKeyValue("Major Version", response.Version.MajorVersion);
            ConsoleHelper.PrintKeyValue("Minor Version", response.Version.MinorVersion);
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
            ConsoleHelper.PrintSectionHeader("OPERASI YANG DIDUKUNG:");
            foreach (var operation in response.SupportedOperations)
            {
                ConsoleHelper.PrintListItem(operation);
            }
            Console.WriteLine();
        }
        else
        {
            ConsoleHelper.PrintSectionHeader("OPERASI YANG DIDUKUNG: Tidak ada data");
            Console.WriteLine();
        }

        // Tampilkan profil yang didukung
        if (response.SupportedProfiles.Any())
        {
            ConsoleHelper.PrintSectionHeader("PROFIL YANG DIDUKUNG:");
            foreach (var profile in response.SupportedProfiles)
            {
                ConsoleHelper.PrintListItem(profile);
            }
            Console.WriteLine();
        }
        else
        {
            ConsoleHelper.PrintSectionHeader("PROFIL YANG DIDUKUNG: Tidak ada data");
            Console.WriteLine();
        }

        // Tampilkan informasi sistem
        if (response.System != null)
        {
            ConsoleHelper.PrintSectionHeader("INFORMASI SISTEM:");
            ConsoleHelper.PrintKeyValue("Nama", response.System.Name);
            ConsoleHelper.PrintKeyValue("ID", response.System.Id);
            ConsoleHelper.PrintKeyValue("Versi", response.System.Version);
            Console.WriteLine();
        }
    }

    /// <summary>
    /// Display AlarmEventTypes response
    /// </summary>
    public static void DisplayAlarmEventTypes(AlarmEventTypesResponseDto response)
    {
        ConsoleHelper.PrintSeparator();
        Console.WriteLine("HASIL RESPONSE:");
        ConsoleHelper.PrintSeparator();
        Console.WriteLine();

        // Tampilkan response version
        if (!string.IsNullOrEmpty(response.ResponseVersion))
        {
            Console.WriteLine($"Response Version: {response.ResponseVersion}");
            Console.WriteLine();
        }

        // Tampilkan alarm event types
        if (response.Types != null && response.Types.Any())
        {
            ConsoleHelper.PrintSectionHeader("ALARM EVENT TYPES:");
            foreach (var type in response.Types)
            {
                ConsoleHelper.PrintListItem(type);
            }
            Console.WriteLine();
        }
        else
        {
            ConsoleHelper.PrintSectionHeader("ALARM EVENT TYPES: Tidak ada data");
            Console.WriteLine();
        }
    }
}
