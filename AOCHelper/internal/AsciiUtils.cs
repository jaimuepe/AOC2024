using System.Text;

namespace AOCHelper.@internal;

internal static class AsciiUtils
{
    private static readonly string[] DigitToAscii = new string[10];

    internal static string GetAsciiTitle(int year)
    {
        var buff = new StringBuilder();

        const string title = """
                               ___      _                 _            __   _____           _       
                              / _ \    | |               | |          / _| /  __ \         | |      
                             / /_\ \ __| |_   _____ _ __ | |_    ___ | |_  | /  \/ ___   __| | ___  
                             |  _  |/ _` \ \ / / _ \ '_ \| __|  / _ \|  _| | |    / _ \ / _` |/ _ \ 
                             | | | | (_| |\ V /  __/ | | | |_  | (_) | |   | \__/\ (_) | (_| |  __/ 
                             \_| |_/\__,_| \_/ \___|_| |_|\__|  \___/|_|    \____/\___/ \__,_|\___| 
                                                                                                    
                                                                                                                                                                       
                             """;

        var yearStr = year.ToString("0000");

        var asciiDigits = new[]
        {
            AOC_Utils.SplitLines(AsciiUtils.GetAsciiDigit(yearStr[0])).ToList(),
            AOC_Utils.SplitLines(AsciiUtils.GetAsciiDigit(yearStr[1])).ToList(),
            AOC_Utils.SplitLines(AsciiUtils.GetAsciiDigit(yearStr[2])).ToList(),
            AOC_Utils.SplitLines(AsciiUtils.GetAsciiDigit(yearStr[3])).ToList(),
        };

        var titleLines = AOC_Utils.SplitLines(title).ToList();

        for (var i = 0; i < titleLines.Count; i++)
        {
            buff.Append(titleLines[i]);
            buff.Append(asciiDigits[0][i]);
            buff.Append(asciiDigits[1][i]);
            buff.Append(asciiDigits[2][i]);
            buff.Append(asciiDigits[3][i]);

            if (i < titleLines.Count - 1)
            {
                buff.AppendLine();
            }
        }

        return buff.ToString();
    }

    private static string GetAsciiDigit(char digit)
    {
        if (digit is < '0' or > '9')
        {
            AOC_Logger.Error($"char = '{digit}' is not a digit!");
            return DigitToAscii[0];
        }

        return DigitToAscii[digit - '0'];
    }

    static AsciiUtils()
    {
        DigitToAscii[0] = """
                           _____ 
                          |  _  |
                          | |/' |
                          |  /| |
                          \ |_/ /
                           \___/ 
                                 
                                 
                          """;

        DigitToAscii[1] = """
                           __  
                          /  | 
                          `| | 
                           | | 
                          _| |_
                          \___/
                               
                               
                          """;

        DigitToAscii[2] = """
                           _____ 
                          / __  \
                          `' / /'
                            / /  
                          ./ /___
                          \_____/
                                 
                                 
                          """;

        DigitToAscii[3] = """
                           _____ 
                          |____ |
                              / /
                              \ \
                          .___/ /
                          \____/ 
                                 
                                 
                          """;

        DigitToAscii[4] = """
                             ___ 
                            /   |
                           / /| |
                          / /_| |
                          \___  |
                              |_/
                                 
                                 
                          """;

        DigitToAscii[5] = """
                           _____ 
                          |  ___|
                          |___ \ 
                              \ \
                          /\__/ /
                          \____/ 
                                 
                                 
                          """;

        DigitToAscii[6] = """
                            ____ 
                           / ___|
                          / /___ 
                          | ___ \
                          | \_/ |
                          \_____/
                                 
                                 
                          """;

        DigitToAscii[7] = """
                           ______
                          |___  /
                             / / 
                            / /  
                          ./ /   
                          \_/    
                                 
                                 
                          """;

        DigitToAscii[8] = """
                           _____ 
                          |  _  |
                           \ V / 
                           / _ \ 
                          | |_| |
                          \_____/
                                 
                                 
                          """;

        DigitToAscii[9] = """
                           _____ 
                          |  _  |
                          | |_| |
                          \____ |
                          .___/ /
                          \____/ 
                                 
                                 
                          """;
    }
}