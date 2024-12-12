using AOCHelper;
using AOCHelper.Math;

namespace AOC2024;

public class Day12(int year) : AOC_DayBase(year, 12)
{
    private class GardenPlot
    {
        public char Plant { get; init; }

        public int Area { get; set; }

        public int Perimeter { get; set; }

        public int Sides { get; set; }
    }

    protected override void SolveA_Internal(string input)
    {
        var map = AOC_Utils.ParseAsCharMatrix(input);

        var height = map.Length;
        var width = map[0].Length;

        var visitedCells = AOC_Utils.CreateMatrix<bool>(height, width);

        var result = 0L;

        for (var i = 0; i < height; i++)
        {
            for (var j = 0; j < width; j++)
            {
                if (visitedCells[i][j]) continue;
                visitedCells[i][j] = true;

                var plot = new GardenPlot
                {
                    Plant = map[i][j],
                };

                FloodFill_A(plot, new Vector2i(j, i), map, visitedCells);

                result += plot.Area * plot.Perimeter;
            }
        }

        AOC_Logger.Display("Result: " + result);
    }

    protected override void SolveB_Internal(string input)
    {
        var map = AOC_Utils.ParseAsCharMatrix(input);

        var height = map.Length;
        var width = map[0].Length;

        var visitedCells = AOC_Utils.CreateMatrix<bool>(height, width);

        var sides = AOC_Utils.CreateMatrix<eSideBitFlags>(height, width);
        
        var result = 0L;

        for (var i = 0; i < height; i++)
        {
            for (var j = 0; j < width; j++)
            {
                if (visitedCells[i][j]) continue;
                visitedCells[i][j] = true;

                var plot = new GardenPlot
                {
                    Plant = map[i][j],
                };

                FloodFill_B(plot, new Vector2i(j, i), map, visitedCells, sides);

                result += plot.Area * plot.Sides;
            }
        }

        AOC_Logger.Display("Result: " + result);
    }

    private static void FloodFill_A(
        GardenPlot plot,
        Vector2i cellCoords,
        char[][] map,
        bool[][] visitedCells)
    {
        var height = map.Length;
        var width = map[0].Length;

        plot.Area++;

        for (var y = cellCoords.Y - 1; y <= cellCoords.Y + 1; y += 2)
        {
            if (y >= 0 &&
                y < height
                && map[y][cellCoords.X] == plot.Plant)
            {
                if (visitedCells[y][cellCoords.X]) continue;
                visitedCells[y][cellCoords.X] = true;

                FloodFill_A(plot, new Vector2i(cellCoords.X, y), map, visitedCells);
            }
            else
            {
                plot.Perimeter++;
            }
        }

        for (var x = cellCoords.X - 1; x <= cellCoords.X + 1; x += 2)
        {
            if (x >= 0 &&
                x < width &&
                map[cellCoords.Y][x] == plot.Plant)
            {
                if (visitedCells[cellCoords.Y][x]) continue;
                visitedCells[cellCoords.Y][x] = true;

                FloodFill_A(plot, new Vector2i(x, cellCoords.Y), map, visitedCells);
            }
            else
            {
                plot.Perimeter++;
            }
        }
    }

    [Flags]
    private enum eSideBitFlags
    {
        None = 0,
        Top = 1 << 0,
        Right = 1 << 1,
        Down = 1 << 2,
        Left = 1 << 3,
    }

    private static void FloodFill_B(
        GardenPlot plot,
        Vector2i cellCoords,
        char[][] map,
        bool[][] visitedCells,
        eSideBitFlags[][] sides)
    {
        var height = map.Length;
        var width = map[0].Length;

        plot.Area++;

        for (var y = cellCoords.Y - 1; y <= cellCoords.Y + 1; y += 2)
        {
            if (y >= 0 &&
                y < height &&
                map[y][cellCoords.X] == plot.Plant)
            {
                if (visitedCells[y][cellCoords.X]) continue;
                visitedCells[y][cellCoords.X] = true;

                FloodFill_B(plot, new Vector2i(cellCoords.X, y), map, visitedCells, sides);
            }
            else
            {
                if (y == cellCoords.Y - 1) sides[cellCoords.Y][cellCoords.X] |= eSideBitFlags.Top;
                if (y == cellCoords.Y + 1) sides[cellCoords.Y][cellCoords.X] |= eSideBitFlags.Down;
            }
        }

        for (var x = cellCoords.X - 1; x <= cellCoords.X + 1; x += 2)
        {
            if (x >= 0 &&
                x < width &&
                map[cellCoords.Y][x] == plot.Plant)
            {
                if (visitedCells[cellCoords.Y][x]) continue;
                visitedCells[cellCoords.Y][x] = true;

                FloodFill_B(plot, new Vector2i(x, cellCoords.Y), map, visitedCells, sides);
            }
            else
            {
                if (x == cellCoords.X - 1) sides[cellCoords.Y][cellCoords.X] |= eSideBitFlags.Left;
                if (x == cellCoords.X + 1) sides[cellCoords.Y][cellCoords.X] |= eSideBitFlags.Right;
            }
        }
    }

    protected override string TestInput => """
                                           AAAA
                                           BBCD
                                           BBCC
                                           EEEC
                                           """;
}