namespace AOCHelper;

public static class AOC_Logger
{
    public static eLogLevel logLevel;

    public enum eLogLevel
    {
        Debug,
        Info,
        Error,
        None,
    }
    
    public static void Display(string text = "")
    {
        Console.WriteLine(text);
    }

    public static void Debug(string text = "")
    {
        if (logLevel > eLogLevel.Debug) return;

        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("[DBG ] " + text);
        Console.ResetColor();
    }

    public static void Info(string text = "")
    {
        if (logLevel > eLogLevel.Info) return;

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine(text);
        Console.ResetColor();
    }

    public static void Error(string text = "")
    {
        if (logLevel > eLogLevel.Error) return;

        Console.ForegroundColor = ConsoleColor.Red;
        Console.Error.WriteLine("[ERR ] " + text);
        Console.ResetColor();
    }
}