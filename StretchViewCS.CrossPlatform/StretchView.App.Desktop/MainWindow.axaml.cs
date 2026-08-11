using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace StretchView.App.Desktop;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        AvaloniaXamlLoader.Load(this);
    }
}
