namespace MarkdownData;

public enum ColumnAlignment
{
    Unspecified,
    Left,
    Center,
    Right
}

public static class ColumnAlignmentExtensions
{
    public static string ToString(this ColumnAlignment alignment, int width)
    {
        string start = "-";
        string end = "-";
        if (alignment == ColumnAlignment.Left || alignment == ColumnAlignment.Center)
        {
            start = ":";
        }
        if (alignment == ColumnAlignment.Right || alignment == ColumnAlignment.Center)
        {
            end = ":";
        }
        string result = start;
        for (int i = 0; i < width - 2; i++)
        {
            result += "-";
        }
        result += end;
        
        return result;
    }
}

public class Column
{
    public Column()
    {
        Name = "";
    }

    public string Name { get; set; }
    public ColumnAlignment Alignment { get; set; }
    public int Width { get; set; }
}