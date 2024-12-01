﻿using System.Text.RegularExpressions;
using AOCHelper.@internal;

namespace AOCHelper;

public class AOC_Console
{
    private enum eCommand
    {
        quit,
        help,
        create,
        run,
        test,
    }

    private readonly Settings _settings = new();

    public void Run()
    {
        AOC_Logger.logLevel = _settings.LogLevel;
        
        PrintTitleAndInfo(_settings.Year);

        var shouldExit = false;

        while (!shouldExit)
        {
            Console.Write("> ");
            string input;

            try
            {
                var nullableInput = Console.ReadLine();
                if (nullableInput == null) break;

                input = nullableInput;
            }
            catch (Exception e)
            {
                AOC_Logger.Error(e.ToString());
                break;
            }

            var tokens = Regex.Split(input, @"\s+");
            if (tokens.Length == 0) continue;

            if (!Enum.TryParse<eCommand>(tokens[0].ToLowerInvariant(), out var command))
            {
                AOC_Logger.Error("Invalid command");
                continue;
            }

            var args = tokens.Length > 1 ? tokens[1..] : [];

            switch (command)
            {
                case eCommand.quit:
                    shouldExit = true;
                    break;
                case eCommand.help:
                    ShowCommands();
                    break;
                case eCommand.create:
                    HandleCreateDayCommand(args);
                    break;
                case eCommand.run:
                    HandleRunCommand(args);
                    break;
                case eCommand.test:
                    HandleTestCommand(args);
                    break;
                default:
                    AOC_Logger.Error($"Unhandled command {command}!");
                    break;
            }
        }
    }

    private static void ShowCommands()
    {
        foreach (var enumVal in Enum.GetValues<eCommand>())
        {
            AOC_Logger.Display("-> " + enumVal);
            PrintUsage(enumVal);
        }
    }

    private static void PrintTitleAndInfo(int year)
    {
        // avoid trying to log this if log is not even active
        if (AOC_Logger.logLevel > AOC_Logger.eLogLevel.Info) return;

        AOC_Logger.Info();
        AOC_Logger.Info(AsciiUtils.GetAsciiTitle(year));
    }

    private void HandleCreateDayCommand(string[] args)
    {
        if (args.Length == 0)
        {
            PrintUsage(eCommand.create);
            return;
        }

        var daysToCreate = new List<int>();

        if (args[0].Equals("all", StringComparison.InvariantCultureIgnoreCase))
        {
            for (var day = 1; day <= 25; day++)
            {
                daysToCreate.Add(day);
            }
        }
        else
        {
            foreach (var arg in args)
            {
                if (int.TryParse(arg, out var day))
                {
                    daysToCreate.Add(day);
                }
            }
        }

        if (daysToCreate.Count == 0)
        {
            PrintUsage(eCommand.create);
            return;
        }

        foreach (var day in daysToCreate)
        {
            if (CodeGenUtils.CreateClassForDay(_settings.Year, day))
            {
                AOC_Logger.Info($"Class for day = {day} created!");
            }
            else
            {
                AOC_Logger.Info($"Class for day = {day} already exists!");
            }
        }
    }

    private static readonly Regex DayAndPartRegex = new(@"(\d+)(?<parts>[a-zA-Z]{0,2})");

    private void HandleRunCommand(string[] args)
    {
        if (args.Length == 0)
        {
            PrintUsage(eCommand.run);
            return;
        }

        var tuple = ParseDayAndParts(args[0]);
        if (tuple == null)
        {
            PrintUsage(eCommand.run);
            return;
        }

        var day = DayFactory.Create(
            _settings.Year,
            tuple.Value.Item1);

        if (day == null)
        {
            PrintUsage(eCommand.run);
            return;
        }

        day.Solve(tuple.Value.Item2);
    }

    private void HandleTestCommand(string[] args)
    {
        if (args.Length == 0)
        {
            PrintUsage(eCommand.test);
            return;
        }

        var tuple = ParseDayAndParts(args[0]);
        if (tuple == null)
        {
            PrintUsage(eCommand.test);
            return;
        }

        var day = DayFactory.Create(
            _settings.Year,
            tuple.Value.Item1);

        if (day == null)
        {
            PrintUsage(eCommand.test);
            return;
        }

        var input = args.Length > 1 ? args[1] : "";

        day.Test(tuple.Value.Item2, input);
    }

    private static void PrintUsage(eCommand command)
    {
        switch (command)
        {
            case eCommand.create:
                AOC_Logger.Display("Usage: create {day}|all");
                break;
            case eCommand.run:
                AOC_Logger.Display("Usage: run {day}{a|b|ab}");
                break;
            case eCommand.test:
                AOC_Logger.Display("Usage: test {day}{a|b|ab} {test_input}?");
                break;
            default:
                AOC_Logger.Display($"Usage: {command}");
                break;
        }
    }

    private static (int, AOC_DayBase.eDayPart)? ParseDayAndParts(string arg)
    {
        var match = DayAndPartRegex.Match(arg);
        if (!match.Success) return null;

        var day = int.Parse(match.Groups[1].Value);

        var parts = match.Groups["parts"].Value;

        if (parts.Length == 0)
        {
            return (day, AOC_DayBase.eDayPart.PartOne | AOC_DayBase.eDayPart.PartTwo);
        }

        var flags = AOC_DayBase.eDayPart.None;

        foreach (var letter in match.Groups[2].Value)
        {
            if (char.ToLowerInvariant(letter) == 'a')
            {
                flags |= AOC_DayBase.eDayPart.PartOne;
            }
            else if (char.ToLowerInvariant(letter) == 'b')
            {
                flags |= AOC_DayBase.eDayPart.PartTwo;
            }
        }

        if (flags == AOC_DayBase.eDayPart.None) return null;
        return (day, flags);
    }
}