using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using System.Linq;

public class CSVReader
{
    static string SPLIT_RE = ",";
    static string LINE_SPLIT_RE = @"\n|\r";
    static char[] TRIM_CHARS = { '\"' };

    public static List<Dictionary<string, string>> Read(TextAsset data)
    {
        var list = new List<Dictionary<string, string>>();
        //TextAsset data = Resources.Load(file) as TextAsset;

        var lines = Regex.Split(data.text, LINE_SPLIT_RE);

        if (lines.Length <= 1) return list;

        var header = Regex.Split(lines[0], SPLIT_RE);
        for (var i = 1; i < lines.Length; i++)
        {

            var values = Regex.Split(lines[i], SPLIT_RE);

            if (values.Length == 0) continue;
            int kLength = 0;
            for (int k = 0; k < values.Length; k++)
            {
                if (values[k].Length > 0) kLength++;
            }
            if (kLength == 0) continue;
            var entry = new Dictionary<string, string>();
            for (var j = 0; j < header.Length && j < values.Length; j++)
            {
                string value = values[j];
                value = value.TrimStart(TRIM_CHARS).TrimEnd(TRIM_CHARS).Replace("\\", "");
                entry[header[j]] = value;
            }
            list.Add(entry);
        }
        return list;
    }
    public static List<Dictionary<string, object>> Read2Col(TextAsset data)
    {
        var list = new List<Dictionary<string, object>>();
        var text = data.text.Replace("\r\n", "\n").Replace("\"\"", "[_quote_]");
        var matches = Regex.Matches(text, "\"[\\s\\S]+?\"");
        foreach (Match match in matches)
        {
            text = text.Replace(match.Value, match.Value.Replace("\"", null).Replace(",", "[_comma_]").Replace("\n", "[_newline_]"));
            Debug.Log(text);
        }
        Debug.Log(text);
        var lines = Regex.Split(text, LINE_SPLIT_RE);
        if (lines.Length <= 1) return list;
        var header = Regex.Split(lines[0], ",");
        for (var i = 1; i < lines.Length; i++)
        {

            var values = lines[i].Split(',').Select(j => j.Trim()).Select(j => j.Replace("[_quote_]", "\"").Replace("[_comma_]", ",").Replace("[_newline_]", "\n")).ToList();
            if (values.Count == 0) continue;
            int kLength = 0;
            for (int k = 0; k < values.Count; k++)
            {
                Debug.Log(values[k]);
                if (values[k].Length > 0) kLength++;
            }
            if (kLength == 0) continue;
            var entry = new Dictionary<string, object>();
            for (var j = 0; j < header.Length && j < values.Count; j++)
            {
                string value = values[j];
                entry[header[j]] = value;
            }
            list.Add(entry);
        }


        return list;
    }
}
