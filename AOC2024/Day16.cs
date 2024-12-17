using AOCHelper;
using AOCHelper.Math;

namespace AOC2024;

public class Day16(int year) : AOC_DayBase(year, 16)
{
    private class VisitedNodeInfo
    {
        public Vector2I Position { get; }

        public int[] Scores { get; }

        public VisitedNodeInfo(Vector2I position)
        {
            Position = position;

            Scores = new int[4];
            AOC_Utils.Clear(Scores, int.MaxValue);
        }
    }

    private int _width;
    private int _height;

    private Vector2I _exitPos;

    protected override void SolveA_Internal(string input)
    {
        var pos = new Vector2I();

        _exitPos = new Vector2I();

        var map = AOC_Utils.ParseAsCharMatrix(
            input,
            (x, y, c) =>
            {
                if (c == 'S')
                {
                    pos = new Vector2I(x, y);
                }
                else if (c == 'E')
                {
                    _exitPos = new Vector2I(x, y);
                }
            });

        _height = map.Length;
        _width = map[0].Length;

        var visitedNodes = AOC_Utils.CreateMatrix<VisitedNodeInfo>(_height, _width);

        var startNode = new VisitedNodeInfo(pos)
        {
            Scores =
            {
                [AOC_Utils.DirectionEnumToIndex(eDirectionBitFlags.Up)] = 1000,
                [AOC_Utils.DirectionEnumToIndex(eDirectionBitFlags.Right)] = 0,
                [AOC_Utils.DirectionEnumToIndex(eDirectionBitFlags.Down)] = 1000,
                [AOC_Utils.DirectionEnumToIndex(eDirectionBitFlags.Left)] = 2000,
            }
        };

        visitedNodes[pos.Y][pos.X] = startNode;

        TraverseMaze(pos, eDirectionBitFlags.Right, map, visitedNodes);

        var result = visitedNodes[_exitPos.Y][_exitPos.X]!.Scores
            .Min();

        AOC_Logger.Display("Result: " + result);
    }

    protected override void SolveB_Internal(string input)
    {
        var pos = new Vector2I();

        _exitPos = new Vector2I();

        var map = AOC_Utils.ParseAsCharMatrix(
            input,
            (x, y, c) =>
            {
                if (c == 'S')
                {
                    pos = new Vector2I(x, y);
                }
                else if (c == 'E')
                {
                    _exitPos = new Vector2I(x, y);
                }
            });

        _height = map.Length;
        _width = map[0].Length;

        var visitedNodes = AOC_Utils.CreateMatrix<VisitedNodeInfo>(_height, _width);

        var startNode = new VisitedNodeInfo(pos)
        {
            Scores =
            {
                [AOC_Utils.DirectionEnumToIndex(eDirectionBitFlags.Up)] = 1000,
                [AOC_Utils.DirectionEnumToIndex(eDirectionBitFlags.Right)] = 0,
                [AOC_Utils.DirectionEnumToIndex(eDirectionBitFlags.Down)] = 1000,
                [AOC_Utils.DirectionEnumToIndex(eDirectionBitFlags.Left)] = 2000,
            }
        };

        visitedNodes[pos.Y][pos.X] = startNode;

        var paths = TraverseMaze(pos, eDirectionBitFlags.Right, map, visitedNodes)!;

        // take the shortest paths
        var lowestScore = paths.Min(p => p[0].Item2);

        var pathsWithLowestScore = paths
            .Where(p => p[0].Item2 == lowestScore);

        var score = 1 + pathsWithLowestScore
            .SelectMany(p => p)
            .Select(p => p.Item1)
            .Distinct()
            .Count();

        AOC_Logger.Display("Result: " + score);
    }

    private List<List<(Vector2I, int)>>? TraverseMaze(
        Vector2I pos,
        eDirectionBitFlags facingDir,
        char[][] map,
        VisitedNodeInfo?[][] visitedNodes)
    {
        var facingDirIndex = AOC_Utils.DirectionEnumToIndex(facingDir);
        var facingDirVec = AOC_Utils.DirectionEnumToVector(facingDir);

        var neighbors = GetAllValidNeighbors(pos, facingDir, map);

        var nodeScore = visitedNodes[pos.Y][pos.X]!.Scores[facingDirIndex];

        List<List<(Vector2I, int)>>? pathsScores = null;

        foreach (var neighbor in neighbors)
        {
            // how many turns do we need to reach this neighbor?
            var neighborDirVec = neighbor - pos;

            var rotations = GetRotations(facingDirVec, neighborDirVec);

            var neighborScore = nodeScore + 1 + rotations * 1000;

            var neighborDir = AOC_Utils.VectorToDirectionEnum(neighborDirVec);
            var neighborDirIndex = AOC_Utils.DirectionEnumToIndex(neighborDir);

            var (nx, ny) = neighbor;

            var visitedNodeInfo = visitedNodes[ny][nx];

            if (visitedNodeInfo == null)
            {
                visitedNodeInfo = new VisitedNodeInfo(neighbor);
                visitedNodes[ny][nx] = visitedNodeInfo;
            }

            var storedScore = visitedNodeInfo.Scores[neighborDirIndex];

            if (storedScore >= neighborScore)
            {
                visitedNodeInfo.Scores[neighborDirIndex] = neighborScore;
                visitedNodeInfo.Scores[(neighborDirIndex + 1) % 4] = neighborScore + 1000;
                visitedNodeInfo.Scores[(neighborDirIndex + 2) % 4] = neighborScore + 2000;
                visitedNodeInfo.Scores[(neighborDirIndex + 3) % 4] = neighborScore + 1000;

                if (neighbor == _exitPos)
                {
                    var pathScore = new List<(Vector2I, int)> { (neighbor, neighborScore) };
                    pathsScores ??= [];
                    pathsScores.Add(pathScore);
                }
                else
                {
                    var subPaths = TraverseMaze(neighbor, neighborDir, map, visitedNodes);
                    if (subPaths != null)
                    {
                        foreach (var subPath in subPaths)
                        {
                            subPath.Add((neighbor, neighborScore));
                            pathsScores ??= [];
                            pathsScores.Add(subPath);
                        }
                    }
                }
            }
        }

        return pathsScores;
    }

    private static int GetRotations(Vector2I a, Vector2I b)
    {
        if (a == b) return 0;
        if (a == Vector2I.RotateRight(b) || a == Vector2I.RotateLeft(b)) return 1;
        return 2;
    }

    private List<Vector2I> GetAllValidNeighbors(
        Vector2I pos,
        eDirectionBitFlags dir,
        char[][] map)
    {
        var dirVector = AOC_Utils.DirectionEnumToVector(dir);

        var validNeighbors = new List<Vector2I>();

        for (var i = 0; i < 4; i++)
        {
            var neighbor = pos + dirVector;

            if (AOC_Utils.IsInsideBounds(neighbor, _width, _height) &&
                map[neighbor.Y][neighbor.X] != '#')
            {
                validNeighbors.Add(neighbor);
            }

            dirVector = Vector2I.RotateRight(dirVector);
        }

        return validNeighbors;
    }

    protected override string TestInput => """
                                           #################
                                           #...#...#...#..E#
                                           #.#.#.#.#.#.#.#.#
                                           #.#.#.#...#...#.#
                                           #.#.#.#.###.#.#.#
                                           #...#.#.#.....#.#
                                           #.#.#.#.#.#####.#
                                           #.#...#.#.#.....#
                                           #.#.#####.#.###.#
                                           #.#.#.......#...#
                                           #.#.###.#####.###
                                           #.#.#...#.....#.#
                                           #.#.#.#####.###.#
                                           #.#.#.........#.#
                                           #.#.#.#########.#
                                           #S#.............#
                                           #################
                                           """;
}