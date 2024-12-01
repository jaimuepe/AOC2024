using System.Text.Json.Serialization;

namespace AOCHelper.@internal;

internal class SettingsModel
{
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    [JsonPropertyName("year")]
    public int Year { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    [JsonPropertyName("log")]
    public AOC_Logger.eLogLevel LogLevel { get; set; } = AOC_Logger.eLogLevel.Info;
}

internal class Settings
{
    private SettingsModel? _model;

    internal int Year
    {
        get
        {
            _model ??= FileUtils.ReadSettingsFile();
            return _model.Year;
        }
    }

    internal AOC_Logger.eLogLevel LogLevel
    {
        get
        {
            _model ??= FileUtils.ReadSettingsFile();
            return _model.LogLevel;
        }
    }
}