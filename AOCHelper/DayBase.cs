using System.Net;
using System.Text.Json;

namespace AOCHelper;

public abstract class DayBase
{
    private const string BaseUrl = "https://adventofcode.com";
    private const string LocalInputUrlTemplate = "/{0}/day/{1}/input";

    private static readonly Uri BaseAddress = new(BaseUrl);

    private readonly int _year;
    private readonly int _day;

    private string? _input;

    protected abstract string TestInput { get; }

    protected DayBase(int year, int day)
    {
        _year = year;
        _day = day;
    }

    public DayBase SolveA()
    {
        Task.Run(async () => await SolveA_Async()).Wait();
        return this;
    }

    public DayBase SolveB()
    {
        Task.Run(async () => await SolveB_Async()).Wait();
        return this;
    }

    public DayBase TestA()
    {
        TestA(TestInput);
        return this;
    }

    public DayBase TestB()
    {
        TestB(TestInput);
        return this;
    }

    public DayBase TestA(string testInput)
    {
        Console.WriteLine($"\n--- AOC {_year} DAY {_day:00} A (TEST) ---\n");

        SolveA_Internal(testInput);
        return this;
    }

    public DayBase TestB(string testInput)
    {
        Console.WriteLine($"\n--- AOC {_year} DAY {_day:00} B (TEST) ---\n");

        SolveA_Internal(testInput);
        return this;
    }

    public DayBase TestAB(string testInput)
    {
        TestA(testInput);
        TestB(testInput);
        return this;
    }

    protected abstract void SolveA_Internal(string input);

    protected abstract void SolveB_Internal(string input);


    private async Task SolveA_Async()
    {
        Console.WriteLine($"\n--- AOC {_year} DAY {_day:00} A ---\n");

        if (!await GetInput_Async()) return;
        SolveA_Internal(_input!);
    }

    private async Task SolveB_Async()
    {
        Console.WriteLine($"\n--- AOC {_year} DAY {_day:00} B ---\n");

        if (!await GetInput_Async()) return;
        SolveB_Internal(_input!);
    }

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
            var secretsJson = await File.ReadAllTextAsync("./secrets.json");
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

    private string GetCachedInputFilePath() => $"./Inputs/Input_{_year:0000}_{_day:00}.txt";
}