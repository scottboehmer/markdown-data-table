namespace MarkdownData;

public class Row
{
    public Row()
    {
        _values = new Dictionary<string, string>();
    }

    public string GetValue(string column)
    {
        if (_values.ContainsKey(column))
        {
            return _values[column];
        }
        else
        {
            return "";
        }
    }

    internal void SetValue(string column, string value)
    {
        _values[column] = value;
    }

    private Dictionary<string, string> _values;
}