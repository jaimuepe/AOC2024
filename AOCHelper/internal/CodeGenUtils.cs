using System.Reflection;

namespace AOCHelper.@internal;

internal static class CodeGenUtils
{
    private const string DayClassTemplatePath = "./DayClassTemplate.txt";
    
    internal static bool CreateClassForDay(int year, int day)
    {
        var existingType = FindDayClass(day);
        if (existingType != null) return false;

        var namespaceName = $"AOC{year:0000}";
        var day00 = day.ToString("00");

        var workingDirectory = Environment.CurrentDirectory;
        var projectDirectory = Directory.GetParent(workingDirectory)!.Parent!.Parent!.FullName;

        var dayClassPath = Path.Combine(projectDirectory, "Day" + day00 + ".cs");

        if (File.Exists(dayClassPath)) return false;
        
        var classContent = string.Format(
            File.ReadAllText(DayClassTemplatePath),
            namespaceName,
            day00,
            day);

        File.WriteAllText(dayClassPath, classContent);

        return true;
    }

    internal static Type? FindDayClass(int day)
    {
        var entryAssembly = Assembly.GetEntryAssembly();
        if (entryAssembly == null)
        {
            throw new ApplicationException("Entry assembly is null!");
        }

        var types = entryAssembly.GetTypes();

        var typeName = $"Day{day:00}";

        return types
            .FirstOrDefault(type => type.Name == typeName);
    }
}