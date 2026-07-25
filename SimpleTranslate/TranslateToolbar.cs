namespace SimpleTranslate;

public sealed partial class TranslateToolbar : Grid
{
    public static string[] LanguageList =
    [
        
    ];

    public TranslateToolbar()
    {
        this.DataContext(TranslateState.Instance, (control, vm) =>
        {
            control
                .Grid(row: 0)
                .ColumnDefinitions(
                    new ColumnDefinition()
                        .Width("*"),
                    new ColumnDefinition()
                        .Width("48"),
                    new ColumnDefinition()
                        .Width("*"),
                    new ColumnDefinition()
                        .Width("auto")
                )
                .VerticalAlignment(VerticalAlignment.Center)
                .Children(
                    new ComboBox()
                        .Name(out ComboBox SourceLanguageBox)
                        .Grid(column: 0)
                        .Width(150)
                        .PlaceholderText("源语言")
                        .Items(
                            "zho_Hans",
                            "eng_Latn"
                        )
                        .SelectedItem(x => x.Binding(() => vm.SourceLanguage).TwoWay()),
                    new Button()
                        .Grid(column: 1)
                        .Content(
                            new FontIcon()
                                .FontFamily(ThemeResource.Get<FontFamily>("SymbolThemeFontFamily"))
                                .Glyph("\uF1CB")
                        ),
                    new ComboBox()
                        .Name(out ComboBox TargetLanguageBox)
                        .Grid(column: 2)
                        .Width(150)
                        .PlaceholderText("目标语言")
                        .Items(
                            "zho_Hans",
                            "eng_Latn"
                        )
                        .SelectedItem(x => x.Binding(() => vm.TargetLanguage).TwoWay()),
                    new Button()
                        .Grid(column: 3)
                        .Content(
                            new FontIcon()
                                .FontFamily(ThemeResource.Get<FontFamily>("SymbolThemeFontFamily"))
                                .Glyph("\uE115")
                        )
                );

            SourceLanguageBox.SelectionChanged += (_, _) =>
            {
                HttpUtil.SeedTranslation();
            };

            TargetLanguageBox.SelectionChanged += (_, _) =>
            {
                HttpUtil.SeedTranslation();
            };
        });
    }
}
