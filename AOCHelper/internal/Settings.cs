using System.Text.Json.Serialization;

namespace AOCHelper.@internal;

internal class SettingsModel
{
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    [JsonPropertyName("year")]
    public int Year { get; set; }
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
}