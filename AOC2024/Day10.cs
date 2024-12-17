using AOCHelper;
using AOCHelper.Math;

namespace AOC2024;

public class Day10(int year) : AOC_DayBase(year, 10)
{
    private struct Node
    {
        public int Number { get; init; }

        public Vector2I Position { get; init; }
    }

    protected override void SolveA_Internal(string input)
    {
        var map = Parse(input);

        var score = 0;

        var visitedNodes = new HashSet<Node>();
        var trails = new List<Node>();

        foreach (var zeroNode in FindZeroNodes(map))
        {
            trails.Clear();
            trails.Add(zeroNode);

            visitedNodes.Clear();

            while (trails.Count > 0)
            {
                for (var k = trails.Count - 1; k >= 0; k--)
                {
                    var node = trails[k];

                    var neighbors = AOC_Utils
                        .GetNeighbors(map, node.Position)
                        .Where(neighbor => neighbor.Number == node.Number + 1);

                    trails.RemoveAt(k);

                    foreach (var neighbor in neighbors)
                    {
                        // already visited
                        if (!visitedNodes.Add(neighbor)) continue;

                        if (neighbor.Number == 9)
                        {
                            score++;
                        }
                        else
                        {
                            trails.Add(neighbor);
                        }
                    }
                }
            }
        }

        AOC_Logger.Display("Score: " + score);
    }

    protected override void SolveB_Internal(string input)
    {
        // same as part A but removing the "already visited" condition
        
        var map = Parse(input);

        var score = 0;

        var trails = new List<Node>();

        foreach (var zeroNode in FindZeroNodes(map))
        {
            trails.Clear();
            trails.Add(zeroNode);

            while (trails.Count > 0)
            {
                for (var k = trails.Count - 1; k >= 0; k--)
                {
                    var node = trails[k];

                    var neighbors = AOC_Utils
                        .GetNeighbors(map, node.Position)
                        .Where(neighbor => neighbor.Number == node.Number + 1);

                    trails.RemoveAt(k);

                    foreach (var neighbor in neighbors)
                    {
                        if (neighbor.Number == 9)
                        {
                            score++;
                        }
                        else
                        {
                            trails.Add(neighbor);
                        }
                    }
                }
            }
        }

        AOC_Logger.Display("Score: " + score);
    }

    private static IEnumerable<Node> FindZeroNodes(Node[][] map)
    {
        for (var i = 0; i < map.Length; i++)
        {
            for (var j = 0; j < map[0].Length; j++)
            {
                var n = map[i][j];

                if (n.Number == 0) yield return n;
            }
        }
    }

    private static Node[][] Parse(string input)
    {
        var map = AOC_Utils.ParseAsMatrix(
            input,
            (x, y, c) => new Node
            {
                Number = c - '0',
                Position = new Vector2I(x, y)
            });
        return map;
    }

    protected override string TestInput => """
                                           89010123
                                           78121874
                                           87430965
                                           96549874
                                           45678903
                                           32019012
                                           01329801
                                           10456732
                                           """;
}