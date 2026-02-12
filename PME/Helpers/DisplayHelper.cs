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

    /// <summary>
    /// Display GetItems response
    /// </summary>
    public static void DisplayItems(GetItemsResponseDto response)
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

        // Tampilkan Value Items
        if (response.ValueItems != null && response.ValueItems.Any())
        {
            ConsoleHelper.PrintSectionHeader($"VALUE ITEMS ({response.ValueItems.Count} items):");
            Console.WriteLine();

            foreach (var item in response.ValueItems)
            {
                Console.WriteLine($"  Item: {item.Name ?? "N/A"}");
                ConsoleHelper.PrintKeyValue("ID", item.Id, indent: 4);
                ConsoleHelper.PrintKeyValue("Description", item.Description, indent: 4);
                ConsoleHelper.PrintKeyValue("Type", item.Type, indent: 4);
                ConsoleHelper.PrintKeyValue("Value", item.Value, indent: 4);
                ConsoleHelper.PrintKeyValue("Unit", item.Unit, indent: 4);
                ConsoleHelper.PrintKeyValue("Writeable", item.Writeable, indent: 4);
                ConsoleHelper.PrintKeyValue("State", item.State, indent: 4);
                Console.WriteLine();
            }
        }

        // Tampilkan History Items
        if (response.HistoryItems != null && response.HistoryItems.Any())
        {
            ConsoleHelper.PrintSectionHeader($"HISTORY ITEMS ({response.HistoryItems.Count} items):");
            Console.WriteLine();

            foreach (var item in response.HistoryItems)
            {
                Console.WriteLine($"  Item: {item.Name ?? "N/A"}");
                ConsoleHelper.PrintKeyValue("ID", item.Id, indent: 4);
                ConsoleHelper.PrintKeyValue("Description", item.Description, indent: 4);
                ConsoleHelper.PrintKeyValue("Type", item.Type, indent: 4);
                ConsoleHelper.PrintKeyValue("Unit", item.Unit, indent: 4);
                Console.WriteLine();
            }
        }

        // Tampilkan Alarm Items
        if (response.AlarmItems != null && response.AlarmItems.Any())
        {
            ConsoleHelper.PrintSectionHeader($"ALARM ITEMS ({response.AlarmItems.Count} items):");
            Console.WriteLine();

            foreach (var item in response.AlarmItems)
            {
                Console.WriteLine($"  Item: {item.Name ?? "N/A"}");
                ConsoleHelper.PrintKeyValue("ID", item.Id, indent: 4);
                ConsoleHelper.PrintKeyValue("Description", item.Description, indent: 4);
                Console.WriteLine();
            }
        }

        if ((response.ValueItems?.Any() != true) && (response.HistoryItems?.Any() != true) && (response.AlarmItems?.Any() != true))
        {
            ConsoleHelper.PrintSectionHeader("ITEMS: Tidak ada data");
            Console.WriteLine();
        }
    }

    /// <summary>
    /// Display GetValues response
    /// </summary>
    public static void DisplayValues(GetValuesResponseDto response)
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

        // Tampilkan Values
        if (response.Values != null && response.Values.Any())
        {
            ConsoleHelper.PrintSectionHeader($"VALUES ({response.Values.Count} values):");
            Console.WriteLine();

            foreach (var value in response.Values)
            {
                Console.WriteLine($"  Value:");
                ConsoleHelper.PrintKeyValue("ID", value.Id, indent: 4);
                ConsoleHelper.PrintKeyValue("Value", value.Value, indent: 4);
                ConsoleHelper.PrintKeyValue("Timestamp", value.Timestamp?.ToString("yyyy-MM-dd HH:mm:ss"), indent: 4);
                ConsoleHelper.PrintKeyValue("Quality", value.Quality, indent: 4);
                Console.WriteLine();
            }
        }
        else
        {
            ConsoleHelper.PrintSectionHeader("VALUES: Tidak ada data");
            Console.WriteLine();
        }
    }

    /// <summary>
    /// Display GetContainerItems response
    /// </summary>
    public static void DisplayContainerItems(GetContainerItemsResponseDto response)
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

        // Tampilkan Container Items
        if (response.Items != null && response.Items.Any())
        {
            ConsoleHelper.PrintSectionHeader($"CONTAINER ITEMS ({response.Items.Count} items):");
            Console.WriteLine();

            foreach (var item in response.Items)
            {
                string itemType = item.IsContainer ? "📁 Container" : "📄 Item";
                Console.WriteLine($"  {itemType}: {item.Name ?? "N/A"}");
                ConsoleHelper.PrintKeyValue("ID", item.Id, indent: 4);
                ConsoleHelper.PrintKeyValue("Description", item.Description, indent: 4);
                ConsoleHelper.PrintKeyValue("Type", item.Type, indent: 4);
                ConsoleHelper.PrintKeyValue("IsContainer", item.IsContainer.ToString(), indent: 4);
                Console.WriteLine();
            }
        }
        else
        {
            ConsoleHelper.PrintSectionHeader("CONTAINER ITEMS: Tidak ada data");
            Console.WriteLine();
        }
    }
}
