using AOCHelper;

namespace AOC2024;

public class Day11(int year) : AOC_DayBase(year, 11)
{
    protected override void SolveA_Internal(string input)
    {
        var stones = AOC_Utils.ParseAsLongList(input);

        var extraStonesCache = new Dictionary<long, long[]>();

        long stonesCount = stones.Count;
        foreach (var stone in stones)
        {
            stonesCount += GetExtraStonesCount(stone, 25, extraStonesCache);
        }

        AOC_Logger.Display("Result: " + stonesCount);
    }

    protected override void SolveB_Internal(string input)
    {
        var stones = AOC_Utils.ParseAsLongList(input);

        var extraStonesCache = new Dictionary<long, long[]>();

        long stonesCount = stones.Count;
        foreach (var stone in stones)
        {
            stonesCount += GetExtraStonesCount(stone, 75, extraStonesCache);
        }

        AOC_Logger.Display("Result: " + stonesCount);
    }

    private static long GetExtraStonesCount(
        long stone,
        int iters,
        Dictionary<long, long[]> extraStonesCache)
    {
        if (iters == 0) return 0;

        if (extraStonesCache
                .TryGetValue(stone, out var arr) &&
            arr[iters] != -1)
        {
            return arr[iters];
        }

        var extraStones = 0L;

        if (stone == 0)
        {
            extraStones += GetExtraStonesCount(1, iters - 1, extraStonesCache);
        }
        else
        {
            var numberOfDigits = AOC_Utils.GetNumberOfDigits(stone);
            if (numberOfDigits % 2 == 0)
            {
                var n = AOC_Utils.Pow10(numberOfDigits / 2);

                var left = stone / n;
                var right = stone % n;

                extraStones += 1;
                extraStones += GetExtraStonesCount(left, iters - 1, extraStonesCache);
                extraStones += GetExtraStonesCount(right, iters - 1, extraStonesCache);
            }
            else
            {
                extraStones += GetExtraStonesCount(stone * 2024, iters - 1, extraStonesCache);
            }
        }

        if (!extraStonesCache.TryGetValue(
                stone,
                out var value))
        {
            var newArr = new long[76];
            AOC_Utils.Clear(newArr, -1);

            value = newArr;
            extraStonesCache.Add(stone, value);
        }

        value[iters] = extraStones;
        return extraStones;
    }

    protected override string TestInput => """
                                           125 17
                                           """;
}