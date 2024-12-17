using System.Text.RegularExpressions;
using AOCHelper;
using AOCHelper.Math;

namespace AOC2024;

public class Day13(int year) : AOC_DayBase(year, 13)
{
    private static readonly Regex _aRegex = new(@"Button A: X([\+-]?\d+), Y([-\+]?\d+)");
    private static readonly Regex _bRegex = new(@"Button B: X([\+-]?\d+), Y([-\+]?\d+)");
    private static readonly Regex _prizeRegex = new(@"Prize: X=(\d+), Y=(\d+)");

    private class ClawMachine
    {
        public Vector2I A { get; init; }

        public Vector2I B { get; init; }

        public int CostA { get; } = 3;

        public int CostB { get; } = 1;

        public Vector2L Prize { get; init; }
    }

    protected override void SolveA_Internal(string input)
    {
        var clawMachines = Parse(input);

        var result = 0L;

        foreach (var machine in clawMachines)
        {
            var lowestTokenCost = -1L;

            for (var i = 0; i <= 100; i++)
            {
                var aPresses = i;

                for (var j = 0; j <= 100; j++)
                {
                    var bPresses = j;

                    Vector2L pos = aPresses * machine.A + bPresses * machine.B;

                    if (pos == machine.Prize)
                    {
                        var tokensCost = aPresses * machine.CostA + bPresses * machine.CostB;
                        if (lowestTokenCost == -1 || tokensCost < lowestTokenCost) lowestTokenCost = tokensCost;

                        break;
                    }

                    if (pos.X >= machine.Prize.X || pos.Y >= machine.Prize.Y)
                    {
                        break;
                    }
                }
            }

            if (lowestTokenCost != -1)
            {
                result += lowestTokenCost;
            }
        }

        AOC_Logger.Display("Tokens: " + result);
    }

    protected override void SolveB_Internal(string input)
    {
        var clawMachines = Parse(input);

        var result = 0L;

        foreach (var machine in clawMachines)
        {
            var (px, py) = machine.Prize + 10000000000000L;
            var (ax, ay) = machine.A;
            var (bx, by) = machine.B;
            
            var num = px - ((double) ax / ay) * py;
            var den = -((double) ax / ay) * by + bx;
            
            var j = (long) Math.Round(num / den);
            var i = (py - by * j) / ay;

            if (i * ax + j * bx != px ||
                i * ay + j * by != py)
            {
                continue;
            }

            result += i * machine.CostA + j * machine.CostB;
        }
        
        AOC_Logger.Display("Tokens: " + result);
    }

    private static List<ClawMachine> Parse(string input)
    {
        var lines = AOC_Utils.SplitLines(input).ToList();

        List<ClawMachine> clawMachines = [];

        for (var i = 0; i < lines.Count; i += 4)
        {
            var aMatch = _aRegex.Match(lines[i]);
            var a = new Vector2I(
                int.Parse(aMatch.Groups[1].Value),
                int.Parse(aMatch.Groups[2].Value));

            var bMatch = _bRegex.Match(lines[i + 1]);
            var b = new Vector2I(
                int.Parse(bMatch.Groups[1].Value),
                int.Parse(bMatch.Groups[2].Value));

            var prizeMatch = _prizeRegex.Match(lines[i + 2]);
            var prize = new Vector2L(
                int.Parse(prizeMatch.Groups[1].Value),
                int.Parse(prizeMatch.Groups[2].Value));
                
            clawMachines.Add(new ClawMachine
            {
                A = a,
                B = b,
                Prize = prize,
            });
        }

        return clawMachines;
    }
    
    protected override string TestInput => """
                                           Button A: X+94, Y+34
                                           Button B: X+22, Y+67
                                           Prize: X=8400, Y=5400

                                           Button A: X+26, Y+66
                                           Button B: X+67, Y+21
                                           Prize: X=12748, Y=12176

                                           Button A: X+17, Y+86
                                           Button B: X+84, Y+37
                                           Prize: X=7870, Y=6450

                                           Button A: X+69, Y+23
                                           Button B: X+27, Y+71
                                           Prize: X=18641, Y=10279
                                           """;
}