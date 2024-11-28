namespace AOCHelper.@internal;

internal static class DayFactory
{
    internal static AOC_DayBase? Create(int year, int day)
    {
        var type = CodeGenUtils.FindDayClass(day);

        if (type == null)
        {
            AOC_Logger.Error($"Could not find type for day = {day}!");
            return null;
        }

        if (!type.IsSubclassOf(typeof(AOC_DayBase)))
        {
            AOC_Logger.Error(
                $"Type = {type.FullName} is not a subclass of {typeof(AOC_DayBase).FullName}!");
            return null;
        }

        // get public constructors
        var constructors = type.GetConstructors();

        if (constructors.Length == 0)
        {
            AOC_Logger.Error(
                $"Type = {type.FullName} does not have any public constructors!");
            return null;
        }

        // invoke the first public constructor with no parameters.
        var instance = constructors[0].Invoke([year]);
        return (AOC_DayBase?)instance;
    }
}