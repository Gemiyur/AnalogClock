using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using AnalogClock.Controls;
using AnalogClock.Dialogs;

namespace AnalogClock;

/// <summary>
/// Класс главного окна.
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        GridContextMenu.DataContext = Clock;
        MainGrid.Background = App.ColorToBrush(Properties.Settings.Default.BackgroundColor);
    }
    private void SettingsButton_Click(object sender, RoutedEventArgs e)
    {
        App.ShowSettingsWindow(Clock, MainGrid);
    }

    private void AboutButton_Click(object sender, RoutedEventArgs e)
    {
        App.ShowAboutDialog(this);
    }

    private void SettingsMenuItem_Click(object sender, RoutedEventArgs e)
    {
        App.ShowSettingsWindow(Clock, MainGrid);
    }

    private void AboutMenuItem_Click(object sender, RoutedEventArgs e)
    {
        App.ShowAboutDialog(this);
    }
}