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

    public readonly int year;
    public readonly int day;

    protected bool IsTest { get; private set; }
    
    protected abstract string TestInput { get; }

    protected AOC_DayBase(int year, int day)
    {
        this.year = year;
        this.day = day;
    }

    public void Solve(eDayPart part)
    {
        var input = InputCache.GetInput(year, day);
        if (input == null)
        {
            AOC_Logger.Error($"Could not retrieve input for year = {year} and day = {day}!");
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
        IsTest = false;
        
        AOC_Logger.Info($"\n--- AOC {year} DAY {day:00} A ---\n");
        SolveA_Internal(input);
    }

    private void SolveB(string input)
    {
        IsTest = false;
        
        AOC_Logger.Info($"\n--- AOC {year} DAY {day:00} B ---\n");
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
        IsTest = true;
        
        AOC_Logger.Info($"\n--- AOC {year} DAY {day:00} A (TEST) ---\n");
        SolveA_Internal(testInput);
    }

    private void TestB(string testInput)
    {
        IsTest = true;
        
        AOC_Logger.Info($"\n--- AOC {year} DAY {day:00} B (TEST) ---\n");
        SolveB_Internal(testInput);
    }

    protected abstract void SolveA_Internal(string input);

    protected abstract void SolveB_Internal(string input);
}