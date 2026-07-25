using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace SimpleTranslate;

public sealed partial class TranslateBoxPanel : Grid
{
    private readonly Subject<string> sourceChanged = new();

    public TranslateBoxPanel()
    {
        this.DataContext(TranslateState.Instance, (page, vm) =>
        {
            page
                .ColumnSpacing(3)
                .ColumnDefinitions(
                    new ColumnDefinition(),
                    new ColumnDefinition()
                )
                .Children(
                    new Border()
                        .Grid(column: 0)
                        .Child(
                            new TextBox()
                                .Name(out TextBox SourceBox)
                                .AcceptsReturn(true)
                                .TextWrapping(TextWrapping.Wrap)
                                .Background(new SolidColorBrush(Colors.Transparent))
                                .FontSize(28)
                                .PlaceholderText("输入文本")
                                .Text(x => 
                                    x
                                     .Binding(() => vm.SourceText)
                                     .UpdateSourceTrigger(UpdateSourceTrigger.PropertyChanged)
                                     .TwoWay()
                                )
                        ),
                    new Border()
                        .Grid(column: 1)
                        .Child(
                            new TextBox()
                                .Name(out TextBox ResultBox)
                                .IsReadOnly(true)
                                .AcceptsReturn(true)
                                .TextWrapping(TextWrapping.Wrap)
                                .Background(new SolidColorBrush(Colors.Transparent))
                                .FontSize(28)
                                .PlaceholderText("翻译的文本")
                                .Text(x =>
                                    x
                                     .Binding(() => vm.ResultText)
                                     .UpdateSourceTrigger(UpdateSourceTrigger.PropertyChanged)
                                     .TwoWay()
                                )
                        )

                );

            SourceBox.TextChanging += (_, _) =>
            {
                sourceChanged.OnNext(SourceBox.Text);
            };

            sourceChanged
                .Throttle(TimeSpan.FromMilliseconds(SettingsModel.WaitTime))
                .DistinctUntilChanged()
                .Subscribe(async text =>
                {
                    HttpUtil.SeedTranslation();
                });
        });
    }
}
