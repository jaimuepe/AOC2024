using AOCHelper;
using AOCHelper.Math;

namespace AOC2024;

public class Day15(int year) : AOC_DayBase(year, 15)
{
    protected override void SolveA_Internal(string input)
    {
        var lines = AOC_Utils.SplitLines(input).ToList();

        var map = Parse(
            lines,
            false,
            out var commands,
            out var fishPosition);

        // AOC_Utils.PrintMatrix(map);

        foreach (var command in commands)
        {
            var dir = CommandToDirection(command);

            var nextPos = fishPosition + dir;

            var c = map[nextPos.Y][nextPos.X];

            if (IsWall(c))
            {
                // AOC_Utils.PrintMatrix(map);
                continue;
            }

            if (IsBox(c))
            {
                var p = nextPos;
                while (IsBox(map[p.Y][p.X])) p += dir;

                // if after a sequence of boxes we don't end up in an empty
                // space, then we can't move them
                if (!IsEmpty(map[p.Y][p.X]))
                {
                    // AOC_Utils.PrintMatrix(map);
                    continue;
                }

                while (p != nextPos)
                {
                    var prevPos = p - dir;
                    Swap(map, prevPos, p);
                    p -= dir;
                }
            }

            Swap(map, fishPosition, nextPos);
            fishPosition = nextPos;

            // AOC_Utils.PrintMatrix(map);
        }

        var result = 0L;

        for (var i = 0; i < map.Count; i++)
        {
            for (var j = 0; j < map[i].Count; j++)
            {
                if (!IsBox(map[i][j])) continue;

                result += 100 * i + j;
            }
        }

        AOC_Logger.Display("Result: " + result);
    }

    protected override void SolveB_Internal(string input)
    {
        var lines = AOC_Utils.SplitLines(input).ToList();

        var map = Parse(
            lines, true,
            out var commands,
            out var fishPosition);

        AOC_Utils.PrintMatrix(map);

        for (var i = 0; i < commands.Count; i++)
        {
            var command = commands[i];

            // AOC_Logger.Display($"i = {i}");

            var dir = CommandToDirection(command);

            var nextPos = fishPosition + dir;

            var c = map[nextPos.Y][nextPos.X];

            if (IsWall(c))
            {
                // AOC_Utils.PrintMatrix(map);
                continue;
            }

            if (IsBox(c))
            {
                var boxesToPush = new List<Vector2I>();

                var boxesToTest = new List<Vector2I>
                {
                    GetBoxLeftSide(map, nextPos)
                };

                var canBePushed = true;

                while (boxesToTest.Count > 0)
                {
                    for (var j = boxesToTest.Count - 1; j >= 0; j--)
                    {
                        var leftPos = boxesToTest[j];
                        var rightPos = leftPos + Vector2I.Right;

                        var nextLeftPos = leftPos + dir;
                        var nextRightPos = rightPos + dir;

                        Vector2I[] s;
                        if (dir == Vector2I.Right) s = [nextRightPos];
                        else if (dir == Vector2I.Left) s = [nextLeftPos];
                        else s = [nextLeftPos, nextRightPos];

                        foreach (var pos in s)
                        {
                            var cc = map[pos.Y][pos.X];

                            if (IsWall(cc))
                            {
                                canBePushed = false;
                                break;
                            }

                            if (IsBox(cc))
                            {
                                var leftSide = GetBoxLeftSide(map, pos);
                                if (!boxesToTest.Contains(leftSide)) boxesToTest.Add(leftSide);
                            }

                            if (!boxesToPush.Contains(leftPos)) boxesToPush.Add(leftPos);
                        }

                        if (!canBePushed) break;

                        boxesToTest.RemoveAt(j);
                    }

                    if (!canBePushed) break;
                }

                if (!canBePushed)
                {
                    // AOC_Utils.PrintMatrix(map);
                    continue;
                }

                // if this is empty means that we can move the boxes
                if (boxesToTest.Count == 0)
                {
                    for (var j = boxesToPush.Count - 1; j >= 0; j--)
                    {
                        var leftPos = boxesToPush[j];
                        var rightPos = leftPos + Vector2I.Right;

                        if (dir == Vector2I.Right)
                        {
                            Swap(map, rightPos, rightPos + dir);
                            Swap(map, leftPos, leftPos + dir);
                        }
                        else
                        {
                            Swap(map, leftPos, leftPos + dir);
                            Swap(map, rightPos, rightPos + dir);
                        }
                    }
                }
            }

            Swap(map, fishPosition, nextPos);
            fishPosition = nextPos;
        }

        var result = 0L;

        for (var i = 0; i < map.Count; i++)
        {
            for (var j = 0; j < map[i].Count; j++)
            {
                if (!IsBoxLeftSide(map[i][j])) continue;

                result += 100 * i + j;
            }
        }

        AOC_Logger.Display("Result: " + result);
    }

    private static List<List<char>> Parse(
        List<string> lines,
        bool isPartB,
        out List<char> commands,
        out Vector2I fishPosition)
    {
        var map = new List<List<char>>();

        commands = [];

        var isParsingMap = true;

        fishPosition = new Vector2I();

        for (var i = 0; i < lines.Count; i++)
        {
            var line = lines[i];

            if (line == "")
            {
                isParsingMap = false;
                continue;
            }

            if (isParsingMap)
            {
                var row = new List<char>();

                for (var j = 0; j < line.Length; j++)
                {
                    var c = line[j];

                    switch (c)
                    {
                        case '#':
                            row.Add('#');
                            if (isPartB) row.Add('#');
                            break;
                        case 'O':
                            if (isPartB)
                            {
                                row.Add('[');
                                row.Add(']');
                            }
                            else
                            {
                                row.Add('O');
                            }

                            break;
                        case '.':
                            row.Add('.');
                            if (isPartB) row.Add('.');
                            break;
                        case '@':
                            row.Add('@');
                            if (isPartB)
                            {
                                row.Add('.');
                                fishPosition = new Vector2I(j * 2, i);
                            }
                            else
                            {
                                fishPosition = new Vector2I(j, i);
                            }

                            break;
                    }
                }

                map.Add(row);
            }
            else
            {
                commands.AddRange(line);
            }
        }

        return map;
    }

    private static bool IsWall(char c) => c == '#';
    private static bool IsBox(char c) => c is 'O' || IsBoxLeftSide(c) || IsBoxRightSide(c);
    private static bool IsEmpty(char c) => c == '.';
    private static bool IsBoxLeftSide(char c) => c == '[';
    private static bool IsBoxRightSide(char c) => c == ']';

    private static Vector2I GetBoxLeftSide(List<List<char>> map, Vector2I pos)
    {
        var c = map[pos.Y][pos.X];
        if (IsBoxLeftSide(c)) return pos;
        else return pos + Vector2I.Left;
    }

    private static Vector2I GetOppositeBoxSide(List<List<char>> map, Vector2I boxPos)
    {
        var c = map[boxPos.Y][boxPos.X];

        int otherSideX;
        if (IsBoxLeftSide(c)) otherSideX = boxPos.X + 1;
        else if (IsBoxRightSide(c)) otherSideX = boxPos.X - 1;
        else otherSideX = boxPos.X;

        return new Vector2I(otherSideX, boxPos.Y);
    }

    private static void Swap(List<List<char>> map, Vector2I a, Vector2I b)
    {
        Swap(map, a.X, a.Y, b.X, b.Y);
    }

    private static void Swap(List<List<char>> map, int lx, int ly, int rx, int ry)
    {
        (map[ly][lx], map[ry][rx]) = (map[ry][rx], map[ly][lx]);
    }

    private static Vector2I CommandToDirection(char c)
    {
        return c switch
        {
            'v' => Vector2I.Up,
            '>' => Vector2I.Right,
            '^' => Vector2I.Down,
            _ => Vector2I.Left
        };
    }

    protected override string TestInput => """
                                           ##########
                                           #..O..O.O#
                                           #......O.#
                                           #.OO..O.O#
                                           #..O@..O.#
                                           #O#..O...#
                                           #O..O..O.#
                                           #.OO.O.OO#
                                           #....O...#
                                           ##########

                                           <vv>^<v^>v>^vv^v>v<>v^v<v<^vv<<<^><<><>>v<vvv<>^v^>^<<<><<v<<<v^vv^v>^
                                           vvv<<^>^v^^><<>>><>^<<><^vv^^<>vvv<>><^^v>^>vv<>v<<<<v<^v>^<^^>>>^<v<v
                                           ><>vv>v^v^<>><>>>><^^>vv>v<^^^>>v^v^<^^>v^^>v^<^v>v<>>v^v^<v>v^^<^^vv<
                                           <<v<^>>^^^^>>>v^<>vvv^><v<<<>^^^vv^<vvv>^>v<^^^^v<>^>vvvv><>>v^<<^^^^^
                                           ^><^><>>><>^^<<^^v>>><^<v>^<vv>>v>>>^v><>^v><<<<v>>v<v<v>vvv>^<><<>^><
                                           ^>><>^v<><^vvv<^^<><v<<<<<><^v<<<><<<^^<v<^^^><^>>^<v^><<<^>>^v<v^v<v^
                                           >^>>^v>vv>^<<^v<>><<><<v<<v><>v<^vv<<<>^^v^>^^>>><<^v>>v^v><^^>>^<>vv^
                                           <><^^>^^^<><vvvvv^v<v<<>^v<v>v<<^><<><<><<<^^<<<^<<>><<><^^^>^^<>^>v<>
                                           ^^>vv<^v^v<vv>^<><v<^v>^^^>>>^^vvv^>vvv<>>>^<^>>>>>^<<^v>^vvv<>^<><<v>
                                           v^^>>><<^^<>>^v^<v^vv<>v^<<>^<^v^v><^<<<><<^<v><v<>vv>>v><v^<vv<>v^<<^
                                           """;
}