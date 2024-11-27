﻿using System.Net;
using System.Text.Json;

namespace AOC2024;

public abstract class DayBase
{
    private const string BaseUrl = "https://adventofcode.com";
    private const string LocalInputUrlTemplate = "/{0}/day/{1}/input";

    private static readonly Uri BaseAddress = new(BaseUrl);

    private readonly int _year;
    private readonly int _day;

    private string? _input;

    protected DayBase(int year, int day)
    {
        _year = year;
        _day = day;
    }

    public async Task SolveA_Async()
    {
        Console.WriteLine($"\n--- DAY {_day:00} A ---\n");

        if (!await GetInput_Async()) return;
        SolveA_Internal(_input!);
    }

    public async Task SolveB_Async()
    {
        Console.WriteLine($"\n--- DAY {_day:00} B ---\n");

        if (!await GetInput_Async()) return;
        SolveB_Internal(_input!);
    }

    protected abstract void SolveA_Internal(string input);

    protected abstract void SolveB_Internal(string input);

    private async Task<bool> GetInput_Async()
    {
        if (await TryReadInputFromMemory_Async()) return true;

        if (await TryReadInputFromDisk_Async()) return true;

        if (await TryDownloadInput_Async()) return true;

        return false;
    }

    private async Task<bool> TryReadInputFromMemory_Async()
    {
        var result = _input != null;
        return await Task.FromResult(result);
    }

    private async Task<bool> TryReadInputFromDisk_Async()
    {
        var filepath = GetCachedInputFilePath();

        if (File.Exists(filepath))
        {
            _input = await File.ReadAllTextAsync(filepath);
            return true;
        }

        return false;
    }

    private async Task<bool> TryDownloadInput_Async()
    {
        var inputUrl = string.Format(LocalInputUrlTemplate, _year, _day);

        try
        {
            var secretsJson = await File.ReadAllTextAsync("./Resources/secrets.json");
            var secrets = JsonSerializer.Deserialize<Dictionary<string, string>>(secretsJson);

            var session = secrets!.GetValueOrDefault("aoc_session_cookie");
            var sessionCookie = new Cookie("session", session);

            var cookieContainer = new CookieContainer();
            cookieContainer.Add(BaseAddress, sessionCookie);

            using var handler = new HttpClientHandler();
            handler.UseCookies = true;
            handler.CookieContainer = cookieContainer;

            using var httpClient = new HttpClient(handler);
            httpClient.BaseAddress = BaseAddress;

            var response = await httpClient.GetAsync(inputUrl);
            response.EnsureSuccessStatusCode();

            _input = await response.Content.ReadAsStringAsync();
            
            var filepath = GetCachedInputFilePath();
            
            var file = new FileInfo(filepath);
            file.Directory!.Create();
            
            await File.WriteAllTextAsync(filepath, _input);

            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine("\nException Caught!");
            Console.WriteLine("Message :{0} ", e.Message);

            return false;
        }
    }

    private string GetCachedInputFilePath() => $"./Inputs/Input_{_day:00}.txt";
}