using AOCHelper;

namespace AOC2024;

public class Day07(int year) : AOC_DayBase(year, 7)
{
    protected override void SolveA_Internal(string input)
    {
        var operators = new[] { '+', '*' };
        
        var result = Solve(input, operators);

        AOC_Logger.Display("Result: " + result);
    }

    protected override void SolveB_Internal(string input)
    {
        var operators = new[] { '+', '*' , '|'};
        
        var result = Solve(input, operators);

        AOC_Logger.Display("Result: " + result);
    }
    
    private static long Solve(
        string input, 
        char[] operators)
    {
        var result = 0L;

        var permutationsCache = new List<List<char>>?[20];

        foreach (var line in AOC_Utils.SplitLines(input))
        {
            var colonIdx = line.IndexOf(':');

            var target = long.Parse(line[..colonIdx]);

            var operands = line[(colonIdx + 2)..]
                .Split(" ")
                .Select(long.Parse)
                .ToList();

            var operatorsCount = operands.Count - 1;

            permutationsCache[operatorsCount] ??= AOC_Utils.GetPermutations(
                    operatorsCount,
                    operators)
                .ToList();

            var permutations = permutationsCache[operatorsCount]!;
            
            foreach (var permutation in permutations)
            { 
                var calculatedValue = operands[0];
                
                for (var i = 0; i < permutation.Count; i++)
                {
                    calculatedValue = Apply(calculatedValue, operands[i + 1], permutation[i]);
                }
                
                if (calculatedValue == target)
                {
                    result += target;
                    break;
                }
            }
        }

        return result;
    }

    private static long Apply(long left, long right, char op)
    {
        switch (op)
        {
            case '+':
                return left + right;
            case '*':
                return left * right;
            case '|':
                var n = 1L + (long) Math.Log10(right);
                return (long)(left * Math.Pow(10, n) + right);
            default:
                throw new ApplicationException($"Unhandled operation character = {op}!");
        }
    }

    protected override string TestInput => """
                                           190: 10 19
                                           3267: 81 40 27
                                           83: 17 5
                                           156: 15 6
                                           7290: 6 8 6 15
                                           161011: 16 10 13
                                           192: 17 8 14
                                           21037: 9 7 18 13
                                           292: 11 6 16 20
                                           """;
}
