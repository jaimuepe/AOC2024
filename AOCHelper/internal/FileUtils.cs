using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

namespace AOCHelper.@internal;

internal static class FileUtils
{
    private const string SecretsPath = "./secrets.json";
    private const string SettingsPath = "./settings.json";

    private static string GetInputFilePath(int year, int day) => $"./data/input_{year:0000}_{day:00}.txt";

    internal static bool InputFileExists(int year, int day)
    {
        var path = GetInputFilePath(year, day);
        return File.Exists(path);
    }

    internal static string? ReadInputFile(int year, int day)
    {
        if (!InputFileExists(year, day)) return null;

        var path = GetInputFilePath(year, day);
        return File.ReadAllText(path);
    }

    internal static void SaveInputFile(int year, int day, string input)
    {
        var filepath = GetInputFilePath(year, day);

        var file = new FileInfo(filepath);
        file.Directory!.Create();

        File.WriteAllText(filepath, input);
    }

    internal static SettingsModel ReadSettingsFile()
    {
        return TryReadJsonFile<SettingsModel>(SettingsPath, out var model)
            ? model
            : new SettingsModel();
    }

    internal static Dictionary<string, string> ReadSecretsFile()
    {
        return TryReadJsonFile<Dictionary<string, string>>(SecretsPath, out var dict)
            ? dict
            : new Dictionary<string, string>();
    }

    private static bool TryReadJsonFile<T>(
        string filepath,
        [MaybeNullWhen(false)] out T value)
    {
        if (File.Exists(filepath))
        {
            var obj = JsonSerializer.Deserialize<T>(File.ReadAllText(filepath));
            if (obj != null)
            {
                value = obj;
                return true;
            }
        }

        value = default;
        return false;
    }
}