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

    public static char[][] ParseAsCharMatrix(
        string input,
        Action<int, int, char>? visitorFcn = null) =>
        ParseAsMatrix(input, (_, _, c) => c, visitorFcn);

    public static int[][] ParseAsIntMatrix(string input) =>
        ParseAsMatrix(input, (_, _, c) => c - '0');

    public static T[][] ParseAsMatrix<T>(
        string input,
        Func<int, int, char, T> mapperFcn,
        Action<int, int, T>? visitorFcn = null)
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
                var c = lines[i][j];
                var item = mapperFcn(j, i, lines[i][j]);
                matrix[i][j] = item;
                visitorFcn?.Invoke(j, i, item);
            }
        }

        return matrix;
    }

    public static List<long> ParseAsLongList(string input, string separator = " ")
    {
        var tokens = input.Split(separator);

        var list = new List<long>(tokens.Length);

        foreach (var tok in tokens)
        {
            list.Add(long.Parse(tok));
        }

        return list;
    }

    public static List<int> ParseAsIntList(string input, string separator = " ")
    {
        var tokens = input.Split(separator);

        var list = new List<int>(tokens.Length);

        foreach (var tok in tokens)
        {
            list.Add(int.Parse(tok));
        }

        return list;
    }

    public static List<List<int>> ParseAsIntMatrixList(string input, string separator = " ")
    {
        var outerList = new List<List<int>>();

        foreach (var line in SplitLines(input))
        {
            var innerList = ParseAsIntList(line);
            outerList.Add(innerList);
        }

        return outerList;
    }

    public static T?[][] CreateMatrix<T>(int height, int width)
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

    public static void PrintMatrix(List<char[]> matrix)
    {
        var sb = new StringBuilder();

        for (var i = 0; i < matrix.Count; i++)
        {
            for (var j = 0; j < matrix[i].Length; j++)
            {
                sb.Append(matrix[i][j]);
            }

            sb.AppendLine();
        }

        AOC_Logger.Display(sb.ToString());
    }

    public static void PrintMatrix(List<List<char>> matrix)
    {
        var sb = new StringBuilder();

        for (var i = 0; i < matrix.Count; i++)
        {
            for (var j = 0; j < matrix[i].Count; j++)
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

    public static bool IsInsideBounds(int x, int y, int width, int height)
    {
        return x >= 0 && x < width && y >= 0 && y < height;
    }

    public static bool IsInsideBounds(Vector2I vec, int width, int height) =>
        IsInsideBounds(vec.X, vec.Y, width, height);

    public static IEnumerable<T> GetNeighbors<T>(T[][] grid, Vector2I pos)
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

    public static int GetNumberOfDigits(long n)
    {
        return n switch
        {
            < 10L => 1,
            < 100L => 2,
            < 1000L => 3,
            < 10000L => 4,
            < 100000L => 5,
            < 1000000L => 6,
            _ => 1 + (int)System.Math.Floor(System.Math.Log10(n))
        };
    }

    public static int GetNumberOfDigits(int n)
    {
        return n switch
        {
            < 10 => 1,
            < 100 => 2,
            < 1000 => 3,
            < 10000 => 4,
            < 100000 => 5,
            < 1000000 => 6,
            _ => 1 + (int)System.Math.Floor(System.Math.Log10(n))
        };
    }

    public static long Pow10(int n)
    {
        if (n == 0) return 1L;

        var result = 10L;
        for (var i = 1; i < n; ++i) result *= 10L;

        return result;
    }

    public static eDirectionBitFlags VectorToDirectionEnum(Vector2I dir)
    {
        if (dir == Vector2I.Up) return eDirectionBitFlags.Up;
        if (dir == Vector2I.Right) return eDirectionBitFlags.Right;
        if (dir == Vector2I.Down) return eDirectionBitFlags.Down;
        return eDirectionBitFlags.Left;
    }

    public static Vector2I DirectionEnumToVector(eDirectionBitFlags dir)
    {
        if (dir == eDirectionBitFlags.Up) return Vector2I.Up;
        if (dir == eDirectionBitFlags.Right) return Vector2I.Right;
        if (dir == eDirectionBitFlags.Down) return Vector2I.Down;
        return Vector2I.Left;
    }
    
    public static int DirectionEnumToIndex(eDirectionBitFlags dir)
    {
        if (dir == eDirectionBitFlags.Up) return 0;
        if (dir == eDirectionBitFlags.Right) return 1;
        if (dir == eDirectionBitFlags.Down) return 2;
        return 3;
    }
}