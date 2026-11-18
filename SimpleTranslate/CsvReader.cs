using System.Text;

namespace SimpleTranslate;

public static class CsvReader
{
    public static string[] Read(string path)
    {
        List<string> csv = [];
        using(StreamReader st = new(path))
        {
            char c;
            StringBuilder sb = new();
            while (!st.EndOfStream)
            {
                c = (char)st.Read();

                if (c == '\n')
                {
                    continue;
                }
                else if (c == ',')
                {
                    csv.Add(sb.ToString());
                    sb.Clear();
                    continue;
                }
                else
                {
                    sb.Append(c);
                }
            }
        }

        return [.. csv];
    }
}
