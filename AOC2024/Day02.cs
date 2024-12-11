using AOCHelper;

namespace AOC2024;

public class Day02(int year) : AOC_DayBase(year, 2)
{
    protected override void SolveA_Internal(string input)
    {
        var reports = AOC_Utils.ParseAsIntMatrixList(input);

        var safeReports = reports
            .Count(report => IsSafeReport(report));

        AOC_Logger.Display("Safe reports: " + safeReports);
    }

    protected override void SolveB_Internal(string input)
    {
        var reports = AOC_Utils.ParseAsIntMatrixList(input);

        var safeReports = 0;

        foreach (var report in reports)
        {
            var isSafe = IsSafeReport(report);

            if (!isSafe)
            {
                // create sub-reports removing a different item each time
                for (var i = 0; i < report.Count; i++)
                {
                    var subReport = AOC_Utils.RemoveOneItem(report, i);

                    isSafe = IsSafeReport(subReport);
                    if (isSafe) break;
                }
            }

            if (isSafe) safeReports++;
        }

        AOC_Logger.Display("Safe reports: " + safeReports);
    }

    protected override string TestInput => """
                                           7 6 4 2 1
                                           1 2 7 8 9
                                           9 7 6 2 1
                                           1 3 2 4 5
                                           8 6 4 4 1
                                           1 3 6 7 9
                                           """;

    private static bool IsSafeReport(List<int> report)
    {
        const int minDistance = 1;
        const int maxDistance = 3;

        var lastIncrement = 0;

        for (var i = 1; i < report.Count; i++)
        {
            var increment = report[i] - report[i - 1];

            var absIncrement = Math.Abs(increment);

            // too far!
            if (absIncrement < minDistance || absIncrement > maxDistance) return false;

            // change of direction!
            if (lastIncrement != 0 &&
                Math.Sign(increment) != Math.Sign(lastIncrement)) return false;

            lastIncrement = increment;
        }

        return true;
    }
}