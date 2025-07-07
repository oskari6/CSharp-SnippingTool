using System.Configuration;
using System.Data;
using System.Windows;

namespace SnippingTool;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
protected override void OnStartup(StartupEventArgs e)
{
    base.OnStartup(e);
    var mainWindow = new MainWindow();

    // Must Show() first so RegisterHotKey runs inside Loaded
    mainWindow.Show();

    // Then hide AFTER it's loaded
    mainWindow.WindowState = WindowState.Minimized;
    mainWindow.ShowInTaskbar = false;
}
}

