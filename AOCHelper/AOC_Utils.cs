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

    public static char[][] ParseAsCharMatrix(string input)
    {
        var lines = AOC_Utils
            .SplitLines(input)
            .ToList();

        var rowCount = lines.Count;

        var matrix = new char[rowCount][];

        for (var i = 0; i < lines.Count; i++)
        {
            matrix[i] = lines[i].ToCharArray();
        }

        return matrix;
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
    
    public static T[][] CreateMatrix<T>(int height, int width)
    {
        var matrix = new T[height][];
        
        for (var i = 0; i < height; i++)
        {
            matrix[i] = new T[width];
        }

        return matrix;
    }

    public static T[][] CreateMatrixWithDefaultValue<T>(int height, int width, T defaultValue)
    {
        var matrix = new T[height][];
        
        for (var i = 0; i < height; i++)
        {
            matrix[i] = new T[width];
            Array.Fill(matrix[i], defaultValue);
        }

        return matrix;
    }

    public static void Clear<T>(T[] array, T clearValue)
    {
        Array.Fill(array, clearValue);
    }

    public static void Clear<T>(T[][] array, T clearValue)
    {
        for (var i = 0; i < array.Length; i++)
        {
            Array.Fill(array[i], clearValue);
        }
    }

    public static IEnumerable<List<T>> GetPermutations<T>(
        int size,
        T[] values) => GetPermutations([], size, values);

    private static IEnumerable<List<T>> GetPermutations<T>(
        List<T> list, 
        int size,
        T[] values)
    {
        foreach( var op in values)
        {
            var copy = new List<T>(list) { op };

            if (copy.Count == size)
            {
                yield return copy;
            }
            else
            {
                foreach (var permutation in GetPermutations(copy, size, values))
                {
                    yield return permutation;
                }
            }
        }
    }
}