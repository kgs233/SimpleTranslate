using CommunityToolkit.Mvvm.ComponentModel;

namespace SimpleTranslate;

public partial class TranslateState : ObservableObject
{
    public static TranslateState Instance { get; } = new();

    [ObservableProperty]
    public partial string SourceText { get; set; } = "";

    [ObservableProperty]
    public partial string ResultText { get; set; } = "";

    [ObservableProperty]
    public partial string SourceLanguage { get; set; } = "";

    [ObservableProperty]
    public partial string TargetLanguage { get; set; } = "";

    [ObservableProperty]
    public partial bool IsTranslating { get; set; } = false;
}
