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

    /// <summary>
    /// Display GetEnums response
    /// </summary>
    public static void DisplayEnums(GetEnumsResponseDto response)
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

        // Tampilkan error results jika ada
        if (response.ErrorResults != null && response.ErrorResults.Any())
        {
            ConsoleHelper.PrintSectionHeader("ERROR RESULTS:");
            foreach (var error in response.ErrorResults)
            {
                ConsoleHelper.PrintListItem(error);
            }
            Console.WriteLine();
        }

        // Tampilkan enums
        if (response.Enums != null && response.Enums.Any())
        {
            ConsoleHelper.PrintSectionHeader($"ENUMS ({response.Enums.Count} enums):");
            Console.WriteLine();

            foreach (var enumDto in response.Enums)
            {
                Console.WriteLine($"  Enum: {enumDto.Name ?? "N/A"}");
                ConsoleHelper.PrintKeyValue("ID", enumDto.Id, indent: 4);
                
                if (enumDto.Values != null && enumDto.Values.Any())
                {
                    Console.WriteLine($"    Values ({enumDto.Values.Count}):");
                    foreach (var value in enumDto.Values)
                    {
                        Console.WriteLine($"      • {value.Value} = {value.Text}");
                    }
                }
                else
                {
                    Console.WriteLine("    Values: Tidak ada");
                }
                Console.WriteLine();
            }
        }
        else
        {
            ConsoleHelper.PrintSectionHeader("ENUMS: Tidak ada data");
            Console.WriteLine();
        }
    }
}
