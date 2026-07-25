namespace SimpleTranslate;

public sealed partial class MainPage : Page
{
    public MainPage()
    {
        this.DataContext(TranslateState.Instance, (page, vm) => page
            .Background(ThemeResource.Get<Brush>("ApplicationPageBackgroundThemeBrush"))
            .Content(
                new Grid()
                    .Margin(10, 0, 10, 10)
                    .RowDefinitions(
                        new RowDefinition()
                            .Height("60"),
                        new RowDefinition()
                            .Height("1*"),
                        new RowDefinition()
                            .Height("1*")
                    )
                    .Children(
                        new TranslateToolbar()
                            .Grid(row: 0),
                        new TranslateBoxPanel()
                            .Grid(row: 1),
                        new ProgressBar()
                            .Grid(row: 3)
                            .Name(out ProgressBar bar)
                            .IsIndeterminate(true)
                            .Visibility(x => 
                                x
                                 .Binding(() => vm.IsTranslating)
                                 .OneWay()
                            )
                            .VerticalAlignment(VerticalAlignment.Top)
                    )
            )
        );
    }
}
