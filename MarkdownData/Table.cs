using System.Collections;

namespace MarkdownData;

public class Table: IEnumerable<Row>
{
    public Table(string path)
    {
        _rows = new List<Row>();
        _columns = new List<Column>();

        using (var reader = new StreamReader(path))
        {
            var headerRow = reader.ReadLine();
            if (String.IsNullOrWhiteSpace(headerRow))
            {
                return;
            }
            var headers = headerRow.Split('|', StringSplitOptions.TrimEntries);
            foreach (var header in headers)
            {
                _columns.Add(new Column() { Name = header, Width = Math.Max(header.Length, 5) });
            }

            var alignmentRow = reader.ReadLine();
            if (String.IsNullOrWhiteSpace(alignmentRow))
            {
                return;
            }
            var alignments = alignmentRow.Split('|', StringSplitOptions.TrimEntries);
            if (alignments.Length != _columns.Count)
            {
                return;
            }
            for (int i = 0; i < alignments.Length; i++)
            {
                var startsWith = alignments[i].StartsWith(':');
                var endsWith = alignments[i].EndsWith(':');
                if (startsWith && endsWith)
                {
                    _columns[i].Alignment = ColumnAlignment.Center;
                }
                else if (startsWith)
                {
                    _columns[i].Alignment = ColumnAlignment.Left;
                }
                else if (endsWith)
                {
                    _columns[i].Alignment = ColumnAlignment.Right;
                }
            }

            var dataRow = reader.ReadLine();
            while (!String.IsNullOrWhiteSpace(dataRow))
            {
                var dataEntries = dataRow.Split('|', StringSplitOptions.TrimEntries);

                var row = new Row();

                for (int i = 0; i < dataEntries.Length && i < _columns.Count; i++)
                {
                    row.SetValue(_columns[i].Name, dataEntries[i]);
                    if (_columns[i].Width < dataEntries[i].Length)
                    {
                        _columns[i].Width = dataEntries[i].Length;
                    }
                }

                _rows.Add(row);

                dataRow = reader.ReadLine();
            }
        }
    }

    public void SaveAs(string path)
    {
        using (var writer = new StreamWriter(path))
        {
            // Header Row
            foreach (var column in Columns)
            {
                writer.Write($"| {column.Name.PadRight(column.Width)} ");
            }
            writer.WriteLine("|");

            // Alignment Row
            foreach (var column in Columns)
            {
                writer.Write($"| {column.Alignment.ToString(column.Width)} ");
            }
            writer.WriteLine("|");

            // Data Rows
            foreach (var row in _rows)
            {
                foreach (var column in Columns)
                {
                    writer.Write($"| {row.GetValue(column.Name).PadRight(column.Width)} ");
                }
                writer.WriteLine("|");
            }
        }
    }

    public IEnumerable<Column> Columns { get { return _columns.Where((x) => !String.IsNullOrEmpty(x.Name)); } }

    public IEnumerator<Row> GetEnumerator()
    {
        return _rows.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return _rows.GetEnumerator();
    }

    private List<Row> _rows;
    private List<Column> _columns;
}
