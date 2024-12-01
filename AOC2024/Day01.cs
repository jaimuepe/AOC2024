using AOCHelper;

namespace AOC2024;

public class Day01(int year) : AOC_DayBase(year, 1)
{
    protected override void SolveA_Internal(string input)
    {
        var (leftColumn, rightColumn) = ParseColumns(input);

        leftColumn.Sort();
        rightColumn.Sort();

        var totalDistance = 0;
        for (var i = 0; i < leftColumn.Count; i++)
        {
            totalDistance += Math.Abs(leftColumn[i] - rightColumn[i]);
        }

        AOC_Logger.Display("Total distance: " + totalDistance);
    }

    protected override void SolveB_Internal(string input)
    {
        var (leftColumn, rightColumn) = ParseColumns(input);

        // classic chad for loop
        var hits = new Dictionary<int, int>();

        foreach (var rightNum in rightColumn)
        {
            hits.TryAdd(rightNum, 0);
            hits[rightNum]++;
        }

        var similarityScore = leftColumn
            .Sum(leftNum => leftNum * hits.GetValueOrDefault(leftNum));

        AOC_Logger.Display("Similarity score: " + similarityScore);
    }

    private static (List<int>, List<int>) ParseColumns(string input)
    {
        var leftColumn = new List<int>();
        var rightColumn = new List<int>();

        foreach (var line in AOC_Utils.SplitLines(input))
        {
            var tokens = line.Split("   ");

            var leftNum = int.Parse(tokens[0]);
            var rightNum = int.Parse(tokens[1]);

            leftColumn.Add(leftNum);
            rightColumn.Add(rightNum);
        }

        return (leftColumn, rightColumn);
    }

    protected override string TestInput => """
                                           3   4
                                           4   3
                                           2   5
                                           1   3
                                           3   9
                                           3   3
                                           """;
}