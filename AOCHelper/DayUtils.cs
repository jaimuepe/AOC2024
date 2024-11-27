using System.Reflection;

namespace AOCHelper;

public static class DayUtils
{
    public static bool TryCreateClassForDay(int year, int day)
    {
        var existingType = FindTypeForDay(day);
        if (existingType != null) return false;
        
        var namespaceName = $"AOC{year:0000}";
        var day00 = day.ToString("00");

        var classContent = string.Format(
            File.ReadAllText("./DayTypeTemplate.txt"),
            namespaceName,
            day00,
            day);

        var workingDirectory = Environment.CurrentDirectory;
        string projectDirectory = Directory.GetParent(workingDirectory)!.Parent!.Parent!.FullName;

        var dayClassPath = Path.Combine(projectDirectory, "Day" + day00 + ".cs");

        File.WriteAllText(dayClassPath, classContent);

        return true;
    }

    public static DayBase? CreateDay(int year, int day)
    {
        var type = FindTypeForDay(day);
        
        if (type == null)
        {
            Console.Error.WriteLine(
                $"Could not find type for day = {day} in entry assembly!");
            return null;
        }

        if (!type.IsSubclassOf(typeof(DayBase)))
        {
            Console.Error.WriteLine(
                $"Type = {type.FullName} is not a subclass of {typeof(DayBase).FullName}!");
            return null;
        }

        // get public constructors
        var constructors = type.GetConstructors();

        if (constructors.Length == 0)
        {
            Console.Error.WriteLine(
                $"Type = {type.FullName} does not have any public constructors!");
            return null;
        }

        // invoke the first public constructor with no parameters.
        var instance = constructors[0].Invoke([year]);
        return (DayBase?)instance;
    }

    private static Type? FindTypeForDay(int day)
    {
        var entryAssembly = Assembly.GetEntryAssembly();
        if (entryAssembly == null)
        {
            Console.Error.WriteLine("Entry assembly is null!");
            return null;
        }

        var types = entryAssembly.GetTypes();

        var typeName = $"Day{day:00}";

        return types.FirstOrDefault(type => type.Name == typeName);
    }
}