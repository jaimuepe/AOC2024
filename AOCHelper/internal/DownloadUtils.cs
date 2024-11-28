using System.Net;

namespace AOCHelper.@internal;

internal static class DownloadUtils
{
    private const string LocalInputUrlTemplate = "/{0}/day/{1}/input";

    private static readonly Uri BaseAddress = new("https://adventofcode.com");

    internal static async Task<(string, int)> DownloadInput_Async(int year, int day)
    {
        var inputUrl = string.Format(LocalInputUrlTemplate, year, day);

        try
        {
            var secrets = FileUtils.ReadSecretsFile();

            var session = secrets.GetValueOrDefault("aoc_session_cookie");
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

            var input = await response.Content.ReadAsStringAsync();

            return (input, (int)response.StatusCode);
        }
        catch (Exception e)
        {
            AOC_Logger.Error("Exception Caught!");
            AOC_Logger.Error("Message: " + e.Message);

            int errorCode;

            if (e is HttpRequestException hre &&
                hre.StatusCode != null)
            {
                errorCode = (int)hre.StatusCode!;
            }
            else
            {
                errorCode = -1;
            }

            return (string.Empty, errorCode);
        }
    }
}