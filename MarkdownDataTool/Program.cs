using System.CommandLine;
using MarkdownData;

namespace MarkdownDataTool;

class Program
{
    static int Main(string[] args)
    {
        var fileOption = new Option<FileInfo?>(
            name: "--file",
            description: "The markdown data table file");
        fileOption.IsRequired = true;

        var keyOption = new Option<string?>(
            name: "--key",
            description: "The key for the row to display");

        var readCommand = new Command("read", "Read and display a markdown table file.")
        {
            fileOption,
            keyOption,
        };
        readCommand.SetHandler((file, key) => { ReadFile(file, key); }, fileOption, keyOption);

        var tidyCommand = new Command("tidy", "Read a markdown file, then save a cleaned up copy of it.")
        {
            fileOption,
        };
        tidyCommand.SetHandler((file) => { TidyFile(file); }, fileOption);

        var rootCommand = new RootCommand("Markdown Table Tool");
        rootCommand.AddCommand(readCommand);
        rootCommand.AddCommand(tidyCommand);

        return rootCommand.Invoke(args);
    }

    static int ReadFile(FileInfo? file, string? key)
    {
        if (file == null || !file.Exists)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Error.WriteLine("Unable to open file");
            Console.ResetColor();
            return 1;
        }

        var table = new Table(file.ToString());

        foreach (var column in table.Columns)
        {
            Console.Write($"| {column.Name.PadRight(column.Width)} ");
        }
        Console.WriteLine("|");

        foreach (var column in table.Columns)
        {
            Console.Write($"| {column.Alignment.ToString(column.Width)} ");
        }
        Console.WriteLine("|");

        foreach (var row in table)
        {
            var keyValue = row.GetValue(table.Columns.First().Name);
            if (String.IsNullOrEmpty(key) || String.Equals(keyValue, key))
            {
                foreach (var column in table.Columns)
                {
                    Console.Write($"| {row.GetValue(column.Name).PadRight(column.Width)} ");
                }
                Console.WriteLine("|");
            }
        }

        return 0;
    }

    static int TidyFile(FileInfo? file)
    {
        if (file == null || !file.Exists)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Error.WriteLine("Unable to open file");
            Console.ResetColor();
            return 1;
        }

        var path = file.ToString();

        var table = new Table(path);

        table.SaveAs(path);

        return 0;
    }
}