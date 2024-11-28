namespace AOCHelper.@internal;

internal static class InputCache
{
    internal static string? GetInput(int year, int day)
    {
        if (!EnsureIsCached(year, day))
        {
            AOC_Logger.Error($"Could not get input for year = {year} and day = {day}!");
            return null;
        }

        return FileUtils.ReadInputFile(year, day);
    }

    private static bool EnsureIsCached(int year, int day)
    {
        AOC_Logger.Debug($"Checking if input for year = {year} and day = {day} is cached...");

        AOC_Logger.Debug("Checking if local file exists...");

        if (FileUtils.InputFileExists(year, day))
        {
            AOC_Logger.Debug("Local file found!");
            return true;
        }

        AOC_Logger.Debug("No local file found, downloading input...");

        var (input, errorCode) = Task.Run(async () => await DownloadUtils.DownloadInput_Async(year, day))
            .Result;

        var successful = errorCode is >= 200 and <= 299;

        if (successful)
        {
            AOC_Logger.Debug("Download successful!");
            FileUtils.SaveInputFile(year, day, input);

            return true;
        }
        else
        {
            AOC_Logger.Error($"Download failed with statusCode = {errorCode}!");
            return false;
        }
    }
}