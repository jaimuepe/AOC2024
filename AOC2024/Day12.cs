using System.Numerics;
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

        foreach (var direction in _directions)
        {
            var next = cellCoords + direction;
            
            if (AOC_Utils.IsInsideBounds(next, width, height) &&
                map[next.Y][next.X] == plot.Plant)
            {
                if (visitedCells[next.Y][next.X]) continue;
                visitedCells[next.Y][next.X] = true;
                
                FloodFill_A(plot, next, map, visitedCells);
            }
            else
            {
                plot.Perimeter++;
            }
        }
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

        foreach (var direction in _directions)
        {
            var next = cellCoords + direction;
            
            if (AOC_Utils.IsInsideBounds(next, width, height) &&
                map[next.Y][next.X] == plot.Plant)
            {
                if (visitedCells[next.Y][next.X]) continue;
                visitedCells[next.Y][next.X] = true;
                
                FloodFill_B(plot, next, map, visitedCells, sides);
            }
            else
            {
                var dirBitFlags = DirectionToSideEnum(direction);
                
                var oppositeAxes = new[]
                {
                    Vector2i.RotateLeft(direction),
                    Vector2i.RotateRight(direction),
                };

                if (sides[cellCoords.Y][cellCoords.X].HasFlag(dirBitFlags)) continue;
                
                var alreadyCounted = false;

                sides[cellCoords.Y][cellCoords.X] |= dirBitFlags;
                
                foreach (var axis in oppositeAxes)
                {
                    var p = cellCoords + axis;

                    while (true)
                    {
                        if (!AOC_Utils.IsInsideBounds(p, width, height) || 
                            map[p.Y][p.X] != plot.Plant)
                        {
                            break;
                        }
                        
                        if (sides[p.Y][p.X].HasFlag(dirBitFlags))
                        {
                            alreadyCounted = true;
                        }
                        else
                        {
                            var pp = p + direction;
                            
                            if (!AOC_Utils.IsInsideBounds(pp, width, height) ||
                                map[pp.Y][pp.X] != plot.Plant)
                            {
                                sides[p.Y][p.X] |= dirBitFlags;
                            }
                            else
                            {
                                break;
                            }
                        }
                        
                        p += axis;
                    }
                }

                if (!alreadyCounted) plot.Sides++;
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

    private static readonly Vector2i[] _directions =
    [
        Vector2i.Up,
        Vector2i.Right,
        Vector2i.Down,
        Vector2i.Left,
    ];

    private static eSideBitFlags DirectionToSideEnum(Vector2i dir)
    {
        if (dir == Vector2i.Up) return eSideBitFlags.Top;
        if (dir == Vector2i.Right) return eSideBitFlags.Right;
        if (dir == Vector2i.Down) return eSideBitFlags.Down;
        return eSideBitFlags.Left;
    }

    protected override string TestInput => """
                                           OOOOO
                                           OXOXO
                                           OOOOO
                                           OXOXO
                                           OOOOO
                                           """;
}