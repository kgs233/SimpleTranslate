using CommunityToolkit.Mvvm.ComponentModel;

namespace SimpleTranslate;

public class SettingsModel : ObservableObject
{
    public static SettingsModel Instance { get; } = new();

    public static string API
    {
        get => TryGet("TranslateAPI", out string v) ? v : "127.0.0.1:54413";

        set => Set("TranslateAPI", value);
    }

    public static int WaitTime
    {
        get => TryGet("WaitTime", out int v) ? v : 2000;
        set => Set("TranslateAPI", value);
    }

    private static bool TryGet<T>(string key, out T value)
    {
        if (ApplicationData.Current.LocalSettings.Values.TryGetValue(key, out var obj)
        && obj is T t)
        {
            value = t;
            return true;
        }

        value = default!;
        return false;
    }

    private static void Set<T>(string key, T value)
    {
        ApplicationData.Current.LocalSettings.Values[key] = value;
    }
}
