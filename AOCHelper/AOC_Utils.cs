namespace AOCHelper;

public static class AOC_Utils
{
    public static IEnumerable<string> SplitLines(string input)
    {
        using var reader = new StringReader(input);

        string? line;
        while ((line = reader.ReadLine()) != null)
        {
            yield return line;
        }
    }

    public static List<T> RemoveOneItem<T>(List<T> list, int index)
    {
        // https://stackoverflow.com/a/41958908
        
        var listCount = list.Count;

        // Create an array to store the data.
        var result = new T[listCount - 1];

        // Copy element before the index.
        list.CopyTo(0, result, 0, index);

        // Copy element after the index.
        list.CopyTo(index + 1, result, index, listCount - 1 - index);

        return [..result];
    }

    public static List<List<int>> ParseAsIntMatrix(string input, string separator = " ")
    {
        var outerList = new List<List<int>>();

        foreach (var line in SplitLines(input))
        {
            var tokens = line.Split(separator);

            var innerList = new List<int>();

            foreach (var tok in tokens)
            {
                innerList.Add(int.Parse(tok));
            }

            outerList.Add(innerList);
        }

        return outerList;
    }
}