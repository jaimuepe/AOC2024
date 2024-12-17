using System.Text.RegularExpressions;
using AOCHelper;
using AOCHelper.Math;

namespace AOC2024;

public class Day14(int year) : AOC_DayBase(year, 14)
{
    private static readonly Regex _regex = new(@"p=(\d+),(\d+) v=([-\+]?\d+),([-\+]?\d+)");

    private class Robot
    {
        public Vector2I Position { get; set; }

        public Vector2I Velocity { get; set; }
    }

    protected override void SolveA_Internal(string input)
    {
        var width = IsTest ? 11 : 101;
        var height = IsTest ? 7 : 103;

        var robots = Parse(input);

        for (var i = 0; i < 100; i++)
        {
            foreach (var robot in robots)
            {
                var pos = robot.Position + robot.Velocity;

                if (pos.X < 0) pos.X = width + pos.X;
                else if (pos.X >= width) pos.X -= width;

                if (pos.Y < 0) pos.Y = height + pos.Y;
                else if (pos.Y >= height) pos.Y -= height;

                robot.Position = pos;
            }
        }

        var halfWidth = width / 2;
        var halfHeight = height / 2;

        var quadrants = new int[4];

        foreach (var robot in robots)
        {
            if (robot.Position.X == halfWidth) continue;
            if (robot.Position.Y == halfHeight) continue;

            var qx = robot.Position.X > halfWidth ? 1 : 0;
            var qy = robot.Position.Y > halfHeight ? 1 : 0;

            quadrants[qx + 2 * qy]++;
        }

        var safetyFactor = quadrants.Aggregate(1, (x, y) => x * y);
        AOC_Logger.Display("Result: " + safetyFactor);
    }

    protected override void SolveB_Internal(string input)
    {
        var robots = Parse(input);

        var width = IsTest ? 11 : 101;
        var height = IsTest ? 7 : 103;

        var halfWidth = width / 2;

        var hits = AOC_Utils.CreateMatrix<int>(height, width);

        for (var i = 0; i < 50_000; i++)
        {
            var symmetricRows = 0;

            AOC_Utils.Clear(hits, 0);

            foreach (var robot in robots)
            {
                var pos = robot.Position + robot.Velocity;

                if (pos.X < 0) pos.X = width + pos.X;
                else if (pos.X >= width) pos.X -= width;

                if (pos.Y < 0) pos.Y = height + pos.Y;
                else if (pos.Y >= height) pos.Y -= height;

                hits[pos.Y][pos.X]++;
                robot.Position = pos;
            }

            for (var j = 0; j < height; j++)
            {
                var isRowSymmetric = true;

                for (var k = 0; k < halfWidth; k++)
                {
                    var left = hits[j][k];
                    var right = hits[j][width - 1 - k];

                    if (left == right || left > 0 && right > 0) continue;

                    isRowSymmetric = false;
                    break;
                }

                if (isRowSymmetric)
                {
                    symmetricRows++;
                }
            }

            if (symmetricRows > 12)
            {
                AOC_Logger.Display($"i = {i}");
                PrintMap(robots, width, height);
            }
        }
    }

    private static void PrintMap(List<Robot> robots, int width, int height)
    {
        var mat = AOC_Utils.CreateMatrixWithDefaultValue(height, width, '.');

        foreach (var robot in robots)
        {
            mat[robot.Position.Y][robot.Position.X] = '*';
        }

        AOC_Utils.PrintMatrix(mat);
    }

    private static List<Robot> Parse(string input)
    {
        var robots = new List<Robot>();

        foreach (var line in AOC_Utils.SplitLines(input))
        {
            var match = _regex.Match(line);

            var px = int.Parse(match.Groups[1].Value);
            var py = int.Parse(match.Groups[2].Value);

            var vx = int.Parse(match.Groups[3].Value);
            var vy = int.Parse(match.Groups[4].Value);

            var p = new Vector2I(px, py);
            var v = new Vector2I(vx, vy);

            robots.Add(new Robot
            {
                Position = p,
                Velocity = v,
            });
        }

        return robots;
    }

    protected override string TestInput => """
                                           p=0,4 v=3,-3
                                           p=6,3 v=-1,-3
                                           p=10,3 v=-1,2
                                           p=2,0 v=2,-1
                                           p=0,0 v=1,3
                                           p=3,0 v=-2,-2
                                           p=7,6 v=-1,-3
                                           p=3,0 v=-1,-2
                                           p=9,3 v=2,3
                                           p=7,3 v=-1,2
                                           p=2,4 v=2,-3
                                           p=9,5 v=-3,-3
                                           """;
}