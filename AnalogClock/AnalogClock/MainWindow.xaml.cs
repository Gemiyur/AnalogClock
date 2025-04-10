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
    }

    private void LoadSettings()
    {
        MainGrid.Background = App.ColorToBrush(Properties.Settings.Default.BackgroundColor);
        Clock.DigitBrush = App.ColorToBrush(Properties.Settings.Default.ClockDigitsColor);
        Clock.IsDigitsShown = Properties.Settings.Default.ClockShowDigits;
        Clock.IsRomanDigits = Properties.Settings.Default.ClockRomanDigits;
        Clock.IsRunning = Properties.Settings.Default.ClockRunning;
        Clock.IsTransparent = Properties.Settings.Default.ClockTransparent;
    }

    private void SaveSettings()
    {
        Properties.Settings.Default.BackgroundColor = App.BrushToColor((SolidColorBrush)MainGrid.Background);
        Properties.Settings.Default.ClockDigitsColor = App.BrushToColor(Clock.DigitBrush);
        Properties.Settings.Default.ClockShowDigits = Clock.IsDigitsShown;
        Properties.Settings.Default.ClockRomanDigits = Clock.IsRomanDigits;
        Properties.Settings.Default.ClockRunning = Clock.IsRunning;
        Properties.Settings.Default.ClockTransparent = Clock.IsTransparent;
        Properties.Settings.Default.Save();
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        LoadSettings();
    }

    private void Window_Closed(object sender, EventArgs e)
    {
        SaveSettings();
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