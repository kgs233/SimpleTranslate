using CommunityToolkit.Mvvm.ComponentModel;

namespace SimpleTranslate;

public partial class SettingsModel : ObservableObject
{
    public static SettingsModel Instance { get; } = new();
    [ObservableProperty]
    public partial string LanguageListPath { get; set; } = TryGet("LanguageList", out string l) ? l : $"{AppContext.BaseDirectory}/LanguageList.csv";

    [ObservableProperty]
    public partial string API { get; set; } = TryGet("TranslateAPI", out string api) ? api :  "127.0.0.1:54413";

    partial void OnAPIChanged(string value)
    {
        Set("TranslateAPI", value);
    }

    [ObservableProperty]
    public partial int WaitTime { get; set; } = TryGet("TranslateWaitTime", out int t) ? t : 2000;

    partial void OnWaitTimeChanged(int value)
    {
        Set("TranslateWaitTime", value);
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
