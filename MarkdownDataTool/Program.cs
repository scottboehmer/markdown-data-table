using System.CommandLine;
using System.Security.Cryptography.X509Certificates;
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

        var columnOption = new Option<string?>(
            name: "--column",
            description: "The column name to operate on");

        var orderingOption = new Option<string?>(
            name: "--ordering",
            description: "The order of column names");

        var defaultOption = new Option<string?>(
            name: "--default",
            description: "The default value for new columns");

        var readCommand = new Command("read", "Read and display a markdown table file.")
        {
            fileOption,
            keyOption,
        };
        readCommand.SetHandler((file, key) => { ReadFile(file, key); }, fileOption, keyOption);

        var tidyCommand = new Command("tidy", "Read a markdown file, then save a cleaned up version of it.")
        {
            fileOption,
        };
        tidyCommand.SetHandler((file) => { TidyFile(file); }, fileOption);

        var sortCommand = new Command("sort", "Read a markdown file, then save a sorted version of it.")
        {
            fileOption,
            columnOption,
        };
        sortCommand.SetHandler((file, column) => { SortFile(file, column); }, fileOption, columnOption);

        var arrangeCommand = new Command("arrange", "Rad a markdown file, then re-order the columns")
        {
            fileOption,
            orderingOption,
            defaultOption
        };
        arrangeCommand.SetHandler((file,ordering,value) => { ArrangeColumns(file, ordering, value); }, fileOption, orderingOption, defaultOption);

        var rootCommand = new RootCommand("Markdown Table Tool");
        rootCommand.AddCommand(readCommand);
        rootCommand.AddCommand(tidyCommand);
        rootCommand.AddCommand(sortCommand);
        rootCommand.AddCommand(arrangeCommand);

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

    static int SortFile(FileInfo? file, string? column)
    {
        if (file == null || !file.Exists)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Error.WriteLine("Unable to open file");
            Console.ResetColor();
            return 1;
        }

        if (String.IsNullOrEmpty(column))
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Error.WriteLine("Must specify a column name");
            Console.ResetColor();
            return 1;
        }

        var path = file.ToString();

        var table = new Table(path);

        if (table.Columns.FirstOrDefault((x) => x.Name == column) == null)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Error.WriteLine($"Table does not contain column {column}");
            Console.ResetColor();
            return 1;
        }

        table.Sort(column);

        table.SaveAs(path);

        return 0;
    }

    static int ArrangeColumns(FileInfo? file, string? columns, string? fillValue)
    {
        if (file == null || !file.Exists)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Error.WriteLine("Unable to open file");
            Console.ResetColor();
            return 1;
        }

        if (String.IsNullOrEmpty(columns))
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Error.WriteLine("Must specify a list of column name");
            Console.ResetColor();
            return 1;
        }

        var path = file.ToString();

        var table = new Table(path);

        table.ArrangeColumns(columns.Split(",", StringSplitOptions.TrimEntries), fillValue != null ? fillValue : "" );

        table.SaveAs(path);

        return 0;
    }
}