using AOCHelper.@internal;

namespace AOCHelper;

public abstract class AOC_DayBase
{
    [Flags]
    public enum eDayPart
    {
        None = 0,
        PartOne = 1 << 0,
        PartTwo = 1 << 1,
    }

    private readonly int _year;
    private readonly int _day;

    protected abstract string TestInput { get; }

    protected AOC_DayBase(int year, int day)
    {
        _year = year;
        _day = day;
    }

    public void Solve(eDayPart part)
    {
        var input = InputCache.GetInput(_year, _day);
        if (input == null)
        {
            AOC_Logger.Error($"Could not retrieve input for year = {_year} and day = {_day}!");
            return;
        }

        if (part.HasFlag(eDayPart.PartOne))
        {
            SolveA(input);
        }

        if (part.HasFlag(eDayPart.PartTwo))
        {
            SolveB(input);
        }
    }

    private void SolveA(string input)
    {
        AOC_Logger.Info($"\n--- AOC {_year} DAY {_day:00} A ---\n");
        SolveA_Internal(input);
    }

    private void SolveB(string input)
    {
        AOC_Logger.Info($"\n--- AOC {_year} DAY {_day:00} B ---\n");
        SolveB_Internal(input);
    }

    public void Test(eDayPart part, string input = "")
    {
        if (input == "") input = TestInput;

        if (part.HasFlag(eDayPart.PartOne))
        {
            TestA(input);
        }

        if (part.HasFlag(eDayPart.PartTwo))
        {
            TestB(input);
        }
    }

    private void TestA(string testInput)
    {
        AOC_Logger.Info($"\n--- AOC {_year} DAY {_day:00} A (TEST) ---\n");
        SolveA_Internal(testInput);
    }

    private void TestB(string testInput)
    {
        AOC_Logger.Info($"\n--- AOC {_year} DAY {_day:00} B (TEST) ---\n");
        SolveB_Internal(testInput);
    }

    protected abstract void SolveA_Internal(string input);

    protected abstract void SolveB_Internal(string input);
}