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

        public void AddObstacle(Vector2i position) => _obstacles[position.Y][position.X] = true;

        public void RemoveObstacle(Vector2i position) => _obstacles[position.Y][position.X] = false;

        public bool IsInsideMap(Vector2i position)
        {
            return position.X >= 0 && position.X < Width &&
                   position.Y >= 0 && position.Y < Height;
        }

        public bool IsBlocked(Vector2i position)
        {
            return _obstacles[position.Y][position.X];
        }
    }

    private class Guard
    {
        private static readonly Vector2i[] DirectionsArray =
        [
            Vector2i.Down,
            Vector2i.Right,
            Vector2i.Up,
            Vector2i.Left,
        ];

        public int DirectionIndex { get; private set; }

        public Vector2i Position { get; set; }

        public Vector2i Direction
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
            if (!map.IsInsideMap(nextGuardPosition)) return eMoveResult.OutsideMap;

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

    [Flags]
    enum eDirectionBitFlags
    {
        None = 0,
        Down = 1 << 0,
        Right = 1 << 1,
        Up = 1 << 2,
        Left = 1 << 3,
    }

    protected override void SolveB_Internal(string input)
    {
        var (map, guard) = Parse(input);

        var initialGuardPosition = guard.Position;

        var visitedPositions = GetRoutePositions(guard, map);

        // ignore initial position
        visitedPositions.Remove(initialGuardPosition);

        var visitedPositionsList = new List<Vector2i>(visitedPositions);

        var lastBlockedPosition = new Vector2i();
        var isFirstIteration = true;

        var loopsCount = 0;

        var directionsMap = AOC_Utils.CreateMatrix<eDirectionBitFlags>(map.Height, map.Width);

        while (visitedPositionsList.Count > 0)
        {
            var positionToBlock = visitedPositionsList[0];
            visitedPositionsList.RemoveAt(0);
            
            guard.Position = initialGuardPosition;
            guard.Direction = Vector2i.Down;

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

                if (directionsMap[y][x].HasFlag(directionBit))
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
                    guard.Position = new Vector2i(j, i);
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

    private static HashSet<Vector2i> GetRoutePositions(Guard guard, Map map)
    {
        var visitedPositions = new HashSet<Vector2i> { guard.Position };

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