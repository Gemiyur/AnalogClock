using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using AnalogClock.Controls;

namespace AnalogClock.Dialogs;

/// <summary>
/// Окно настроек приложения.
/// </summary>
public partial class SettingsWindow : Window
{
    private readonly ClockControl clock;

    private readonly Grid grid;

    public SettingsWindow(ClockControl clock, Grid grid)
    {
        InitializeComponent();
        this.clock = clock;
        this.grid = grid;
        ClockGroupBox.DataContext = clock;
        AppGroupBox.DataContext = Properties.Settings.Default;
    }

    private void CheckShowInTaskbar() =>
        System.Windows.Application.Current.MainWindow.ShowInTaskbar = ShowInTaskbarCheckBox.IsChecked == true;

    private void Window_Closed(object sender, EventArgs e) => App.SettingsWindow = null;

    private void ShowInTaskbarCheckBox_Click(object sender, RoutedEventArgs e) => CheckShowInTaskbar();

    private void DigitsColorButton_Click(object sender, RoutedEventArgs e)
    {
        var picker = new BrushPicker(clock.DigitBrush);
        if (picker.Execute())
            clock.DigitBrush = picker.Brush;
    }

    private void BackgroundColorButton_Click(object sender, RoutedEventArgs e)
    {
        var picker = new BrushPicker((SolidColorBrush)grid.Background);
        if (picker.Execute())
            grid.Background = picker.Brush;
    }

    private void ResetButton_Click(object sender, RoutedEventArgs e)
    {
        grid.Background = App.ColorToBrush(Properties.Settings.Default.DefaultBackgroundColor);
        Properties.Settings.Default.CloseToTray = Properties.Settings.Default.DefaultCloseToTray;
        Properties.Settings.Default.MinimizeToTray = Properties.Settings.Default.DefaultMinimizeToTray;
        Properties.Settings.Default.ShowInTaskbar = Properties.Settings.Default.DefaultShowInTaskbar;
        clock.DigitBrush = App.ColorToBrush(Properties.Settings.Default.DefaultClockDigitsColor);
        clock.IsDigitsShown = Properties.Settings.Default.DefaultClockShowDigits;
        clock.IsRomanDigits = Properties.Settings.Default.DefaultClockRomanDigits;
        clock.IsRunning = Properties.Settings.Default.DefaultClockRunning;
        clock.IsTransparent = Properties.Settings.Default.DefaultClockTransparent;
        CheckShowInTaskbar();
    }
}
