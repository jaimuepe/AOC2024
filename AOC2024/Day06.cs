using AOCHelper;
using AOCHelper.Math;

namespace AOC2024;

public class Day06(int year) : AOC_DayBase(year, 6)
{
    private class Map
    {
        public int Width { get; }

        public int Height { get; }

        private readonly bool[][] _obstacles;

        public Map(bool[][] obstacles)
        {
            _obstacles = obstacles;

            Height = _obstacles.Length;
            Width = _obstacles[0].Length;
        }

        public void AddObstacle(Vector2I position) => _obstacles[position.Y][position.X] = true;

        public void RemoveObstacle(Vector2I position) => _obstacles[position.Y][position.X] = false;

        public bool IsBlocked(Vector2I position)
        {
            return _obstacles[position.Y][position.X];
        }
    }

    private class Guard
    {
        private static readonly Vector2I[] DirectionsArray =
        [
            Vector2I.Down,
            Vector2I.Right,
            Vector2I.Up,
            Vector2I.Left,
        ];

        public int DirectionIndex { get; private set; }

        public Vector2I Position { get; set; }

        public Vector2I Direction
        {
            get => DirectionsArray[DirectionIndex];
            set => DirectionIndex = Array.IndexOf(DirectionsArray, value);
        }

        public void RotateRight()
        {
            DirectionIndex = (DirectionIndex + 1) % DirectionsArray.Length;
        }

        public enum eMoveResult
        {
            Success,
            Blocked,
            OutsideMap,
        }

        public eMoveResult Move(Map map)
        {
            var nextGuardPosition = Position + Direction;
            if (!AOC_Utils.IsInsideBounds(nextGuardPosition, map.Width, map.Height)) return eMoveResult.OutsideMap;

            if (map.IsBlocked(nextGuardPosition))
            {
                RotateRight();
                return eMoveResult.Blocked;
            }
            else
            {
                Position = nextGuardPosition;
                return eMoveResult.Success;
            }
        }
    }

    protected override void SolveA_Internal(string input)
    {
        var (map, guard) = Parse(input);

        var visitedPositions = GetRoutePositions(guard, map);

        AOC_Logger.Display("Visited positions: " + visitedPositions.Count);
    }

    protected override void SolveB_Internal(string input)
    {
        var (map, guard) = Parse(input);

        var initialGuardPosition = guard.Position;

        var visitedPositions = GetRoutePositions(guard, map);

        // ignore initial position
        visitedPositions.Remove(initialGuardPosition);

        var visitedPositionsList = new List<Vector2I>(visitedPositions);

        var lastBlockedPosition = new Vector2I();
        var isFirstIteration = true;

        var loopsCount = 0;

        var directionsMap = AOC_Utils.CreateMatrix<eDirectionBitFlags>(map.Height, map.Width);

        while (visitedPositionsList.Count > 0)
        {
            var positionToBlock = visitedPositionsList[0];
            visitedPositionsList.RemoveAt(0);
            
            guard.Position = initialGuardPosition;
            guard.Direction = Vector2I.Down;

            if (isFirstIteration)
            {
                isFirstIteration = false;
            }
            else
            {
                // reset to defaults
                map.RemoveObstacle(lastBlockedPosition);
                AOC_Utils.Clear(directionsMap, eDirectionBitFlags.None);
            }

            map.AddObstacle(positionToBlock);
            lastBlockedPosition = positionToBlock;

            // trace again with the new obstacle and look out for a loop.
            // A loop will happen if the same position is visited twice with the same direction

            // initially the guard is looking down (in our coordinates' system! since Y is inverted)
            directionsMap[guard.Position.Y][guard.Position.X] = eDirectionBitFlags.Down;

            var foundLoop = false;

            while (true)
            {
                var result = guard.Move(map);
                if (result == Guard.eMoveResult.OutsideMap) break;

                var (x, y) = guard.Position;

                var directionBit = DirectionIndexToBitFlag(guard.DirectionIndex);

                if ((directionsMap[y][x] & directionBit) > 0)
                {
                    // loop detected!
                    foundLoop = true;
                    break;
                }

                directionsMap[y][x] |= directionBit;
            }

            if (foundLoop) loopsCount++;
        }

        AOC_Logger.Display("Loops detected: " + loopsCount);
    }

    private static eDirectionBitFlags DirectionIndexToBitFlag(int idx) => (eDirectionBitFlags)(1 << idx);

    private static (Map, Guard) Parse(string input)
    {
        var lines = AOC_Utils
            .SplitLines(input)
            .ToList();

        var rows = lines.Count;
        var cols = lines[0].Length;

        var guard = new Guard();

        var obstacles = new bool[lines.Count][];

        for (var i = 0; i < rows; i++)
        {
            obstacles[i] = new bool[cols];

            for (var j = 0; j < lines[0].Length; j++)
            {
                var c = lines[i][j];
                if (c == '^')
                {
                    guard.Position = new Vector2I(j, i);
                }
                else if (c == '#')
                {
                    obstacles[i][j] = true;
                }
            }
        }

        var map = new Map(obstacles);
        return (map, guard);
    }

    private static HashSet<Vector2I> GetRoutePositions(Guard guard, Map map)
    {
        var visitedPositions = new HashSet<Vector2I> { guard.Position };

        while (true)
        {
            var result = guard.Move(map);

            if (result == Guard.eMoveResult.OutsideMap) break;

            if (result == Guard.eMoveResult.Success)
            {
                visitedPositions.Add(guard.Position);
            }
        }

        return visitedPositions;
    }

    protected override string TestInput => """
                                           ....#.....
                                           .........#
                                           ..........
                                           ..#.......
                                           .......#..
                                           ..........
                                           .#..^.....
                                           ........#.
                                           #.........
                                           ......#...
                                           """;
}