using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using AnalogClock.Controls;

namespace AnalogClock.Dialogs;

/// <summary>
/// Класс окна настроек приложения.
/// </summary>
public partial class SettingsWindow : Window
{
    /// <summary>
    /// Элемент управления "Часы".
    /// </summary>
    private readonly ClockControl clock;

    /// <summary>
    /// Сетка главного окна.
    /// </summary>
    private readonly Grid grid;

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="mainWindow">Главное окно.</param>
    public SettingsWindow(MainWindow mainWindow)
    {
        InitializeComponent();
        clock = mainWindow.Clock;
        grid = mainWindow.MainGrid;
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
        Properties.Settings.Default.SaveLocation = Properties.Settings.Default.DefaultSaveLocation;
        Properties.Settings.Default.ShowInTaskbar = Properties.Settings.Default.DefaultShowInTaskbar;
        clock.DigitBrush = App.ColorToBrush(Properties.Settings.Default.DefaultClockDigitsColor);
        clock.IsDigitsShown = Properties.Settings.Default.DefaultClockShowDigits;
        clock.IsRomanDigits = Properties.Settings.Default.DefaultClockRomanDigits;
        clock.IsRunning = Properties.Settings.Default.DefaultClockRunning;
        clock.IsTransparent = Properties.Settings.Default.DefaultClockTransparent;
        CheckShowInTaskbar();
    }
}
