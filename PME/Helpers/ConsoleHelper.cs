namespace PME.Helpers;

/// <summary>
/// Helper class untuk formatting console output
/// </summary>
public static class ConsoleHelper
{
    /// <summary>
    /// Print separator line
    /// </summary>
    /// <param name="length">Panjang separator (default: 80)</param>
    /// <param name="character">Karakter untuk separator (default: '=')</param>
    public static void PrintSeparator(int length = 80, char character = '=')
    {
        Console.WriteLine(character.ToString().PadRight(length, character));
    }

    /// <summary>
    /// Print header dengan separator
    /// </summary>
    /// <param name="title">Judul header</param>
    public static void PrintHeader(string title)
    {
        PrintSeparator();
        Console.WriteLine(title);
        PrintSeparator();
        Console.WriteLine();
    }

    /// <summary>
    /// Print section header
    /// </summary>
    /// <param name="title">Judul section</param>
    public static void PrintSectionHeader(string title)
    {
        Console.WriteLine(title);
    }

    /// <summary>
    /// Print key-value pair dengan indentasi
    /// </summary>
    /// <param name="key">Key/label</param>
    /// <param name="value">Value</param>
    /// <param name="indent">Jumlah spasi indentasi (default: 2)</param>
    public static void PrintKeyValue(string key, string? value, int indent = 2)
    {
        var indentation = new string(' ', indent);
        Console.WriteLine($"{indentation}{key}: {value ?? "N/A"}");
    }

    /// <summary>
    /// Print list item dengan indentasi
    /// </summary>
    /// <param name="item">Item text</param>
    /// <param name="indent">Jumlah spasi indentasi (default: 2)</param>
    public static void PrintListItem(string item, int indent = 2)
    {
        var indentation = new string(' ', indent);
        Console.WriteLine($"{indentation}- {item}");
    }

    /// <summary>
    /// Print success message
    /// </summary>
    /// <param name="message">Success message</param>
    public static void PrintSuccess(string message)
    {
        PrintSeparator();
        Console.WriteLine(message);
        PrintSeparator();
    }

    /// <summary>
    /// Print error message
    /// </summary>
    /// <param name="message">Error message</param>
    public static void PrintError(string message)
    {
        Console.WriteLine();
        PrintSeparator();
        Console.WriteLine("ERROR:");
        PrintSeparator();
        Console.WriteLine($"Message: {message}");
        Console.WriteLine();
    }
}
