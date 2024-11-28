namespace AOCHelper;

public static class AOC_Utils
{
    public static IEnumerable<string> SplitLines(string input)
    {
        using var reader = new StringReader(input);
        
        string? line;
        while ((line = reader.ReadLine()) != null)
        {
            yield return line;
        }
    }
}