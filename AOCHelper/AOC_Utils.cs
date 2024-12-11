using System.Text;
using AOCHelper.Math;

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

    public static char[][] ParseAsCharMatrix(string input) =>
        ParseAsMatrix(input, (_, _, c) => c);

    public static int[][] ParseAsIntMatrix(string input) =>
        ParseAsMatrix(input, (_, _, c) => c - '0');

    public static T[][] ParseAsMatrix<T>(
        string input,
        Func<int, int, char, T> mapperFcn)
    {
        var lines = SplitLines(input)
            .ToList();

        var height = lines.Count;
        var width = lines[0].Length;

        var matrix = new T[height][];

        for (var i = 0; i < height; i++)
        {
            matrix[i] = new T[width];
            for (var j = 0; j < width; j++)
            {
                matrix[i][j] = mapperFcn(j, i, lines[i][j]);
            }
        }

        return matrix;
    }

    public static List<List<int>> ParseAsIntMatrixList(string input, string separator = " ")
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

    public static void PrintMatrix(char[][] matrix)
    {
        var sb = new StringBuilder();

        for (var i = 0; i < matrix.Length; i++)
        {
            for (var j = 0; j < matrix[i].Length; j++)
            {
                sb.Append(matrix[i][j]);
            }

            sb.AppendLine();
        }

        AOC_Logger.Display(sb.ToString());
    }

    public static void PrintMatrix<T>(T[][] matrix, Func<T, string> toStringFcn)
    {
        var sb = new StringBuilder();

        for (var i = 0; i < matrix.Length; i++)
        {
            for (var j = 0; j < matrix[i].Length; j++)
            {
                sb.Append(toStringFcn(matrix[i][j]));
            }

            sb.AppendLine();
        }

        AOC_Logger.Display(sb.ToString());
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
        foreach (var op in values)
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

    public static bool IsInsideBounds(Vector2i vec, int width, int height)
    {
        return vec.X >= 0 && vec.X < width && vec.Y >= 0 && vec.Y < height;
    }

    public static IEnumerable<T> GetNeighbors<T>(T[][] grid, Vector2i pos)
    {
        var x = pos.X;
        var y = pos.Y;
        
        var height = grid.Length;
        var width = grid[0].Length;

        if (x > 0) yield return grid[y][x - 1];
        if (x < width - 1) yield return grid[y][x + 1];
        
        if (y > 0) yield return grid[y - 1][x];
        if (y < height - 1) yield return grid[y + 1][x];
    }
}