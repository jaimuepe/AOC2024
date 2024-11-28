namespace AOCHelper;

public static class AOC_Logger
{
    public static eLogLevel logLevel = eLogLevel.Info;

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
        Console.WriteLine("[DBG ] " + text);
    }

    public static void Info(string text = "")
    {
        if (logLevel > eLogLevel.Info) return;
        Console.WriteLine(text);
    }

    public static void Error(string text = "")
    {
        if (logLevel > eLogLevel.Error) return;
        Console.Error.WriteLine("[ERR ] " + text);
    }
}