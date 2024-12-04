using AOCHelper;
using AOCHelper.Math;

namespace AOC2024;

public class Day04(int year) : AOC_DayBase(year, 4)
{
    protected override void SolveA_Internal(string input)
    {
        const string word = "XMAS";

        Vector2i[] directions =
        [
            new Vector2i(0, 1),
            new Vector2i(1, 1),
            new Vector2i(1, 0),
            new Vector2i(1, -1),
            new Vector2i(0, -1),
            new Vector2i(-1, -1),
            new Vector2i(-1, 0),
            new Vector2i(-1, 1),
        ];

        var matrix = AOC_Utils.ParseAsCharMatrix(input);

        var count = 0;

        var rows = matrix.Length;
        var cols = matrix[0].Length;

        for (var i = 0; i < rows; i++)
        {
            for (var j = 0; j < cols; j++)
            {
                count += FindWord(matrix, word, j, i, directions);
            }
        }

        AOC_Logger.Display("Count: " + count);
    }

    protected override void SolveB_Internal(string input)
    {
        const string word = "MAS";

        // we only have to search in two directions and then search in the opposite axes
        Vector2i[] directions =
        [
            new Vector2i(1, 1),
            new Vector2i(-1, -1),
        ];

        Vector2i[] oppositeDirections =
        [
            new Vector2i(1, -1),
            new Vector2i(-1, 1),
        ];

        var matrix = AOC_Utils.ParseAsCharMatrix(input);

        var count = 0;

        var rows = matrix.Length;
        var cols = matrix[0].Length;

        for (var i = 0; i < rows; i++)
        {
            for (var j = 0; j < cols; j++)
            {
                foreach (var dir in directions)
                {
                    if (!FindWord(matrix, word, j, i, dir)) continue;

                    var found = false;

                    foreach (var oppositeDir in oppositeDirections)
                    {
                        var x = j + dir.X - oppositeDir.X;
                        var y = i + dir.Y - oppositeDir.Y;

                        if (FindWord(matrix, word, x, y, oppositeDir))
                        {
                            found = true;
                            break;
                        }
                    }

                    if (found) count++;
                }
            }
        }

        AOC_Logger.Display("Count: " + count);
    }

    private static int FindWord(
        char[][] matrix,
        string word,
        int x,
        int y,
        Vector2i[] directions)
    {
        var count = 0;

        foreach (var dir in directions)
        {
            if (FindWord(matrix, word, x, y, dir)) count++;
        }

        return count;
    }

    private static bool FindWord(
        char[][] matrix,
        string word,
        int x,
        int y,
        Vector2i direction)
    {
        var rows = matrix.Length;
        var cols = matrix[0].Length;

        for (var i = 0; i < word.Length; i++)
        {
            var xx = x + direction.X * i;
            var yy = y + direction.Y * i;

            if (xx < 0 || xx > cols - 1 ||
                yy < 0 || yy > rows - 1)
            {
                return false;
            }

            var letter = matrix[yy][xx];

            if (letter != word[i])
            {
                return false;
            }
        }

        return true;
    }

    protected override string TestInput => """
                                           MMMSXXMASM
                                           MSAMXMSMSA
                                           AMXSXMAAMM
                                           MSAMASMSMX
                                           XMASAMXAMM
                                           XXAMMXXAMA
                                           SMSMSASXSS
                                           SAXAMASAAA
                                           MAMMMXMMMM
                                           MXMXAXMASX
                                           """;
}