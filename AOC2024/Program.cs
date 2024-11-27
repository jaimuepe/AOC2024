using AOCHelper;

const int year = 2024;

return;

DayBase CreateDay(int day)
{
    InitDay(day);
    
    var instance = DayUtils.CreateDay(year, day);
    if (instance == null)
    {
        throw new ApplicationException($"Could not create instance for day = {day}!");
    }

    return instance;
}

void InitDay(int day)
{
    if (DayUtils.TryCreateClassForDay(year, day))
    {
        Console.WriteLine($"Day{day:00}.cs created!");
        Environment.Exit(0);
    }
}