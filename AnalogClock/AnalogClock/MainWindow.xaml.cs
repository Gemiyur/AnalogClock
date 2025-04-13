using System.Windows;
using System.Windows.Media;

namespace AnalogClock;

/// <summary>
/// Класс главного окна.
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        MainGrid.Background = App.ColorToBrush(Properties.Settings.Default.BackgroundColor);
        ShowInTaskbar = Properties.Settings.Default.ShowInTaskbar;
        GridContextMenu.DataContext = Clock;
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        Clock.DigitBrush = App.ColorToBrush(Properties.Settings.Default.ClockDigitsColor);
        Clock.IsDigitsShown = Properties.Settings.Default.ClockShowDigits;
        Clock.IsRomanDigits = Properties.Settings.Default.ClockRomanDigits;
        Clock.IsRunning = Properties.Settings.Default.ClockRunning;
        Clock.IsTransparent = Properties.Settings.Default.ClockTransparent;
    }

    private void Window_Closed(object sender, EventArgs e)
    {
        Properties.Settings.Default.BackgroundColor = App.BrushToColor((SolidColorBrush)MainGrid.Background);
        Properties.Settings.Default.ClockDigitsColor = App.BrushToColor(Clock.DigitBrush);
        Properties.Settings.Default.ClockShowDigits = Clock.IsDigitsShown;
        Properties.Settings.Default.ClockRomanDigits = Clock.IsRomanDigits;
        Properties.Settings.Default.ClockRunning = Clock.IsRunning;
        Properties.Settings.Default.ClockTransparent = Clock.IsTransparent;
        Properties.Settings.Default.Save();
    }

    private void Window_StateChanged(object sender, EventArgs e)
    {
        if (WindowState == WindowState.Minimized && App.SettingsWindow != null)
        {
            App.SettingsWindow.Close();
        }
    }

    private void Window_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
        if (Visibility == Visibility.Hidden && App.SettingsWindow != null)
        {
            App.SettingsWindow.Close();
        }
    }

    //private void SettingsButton_Click(object sender, RoutedEventArgs e)
    //{
    //    App.ShowSettingsWindow(Clock, MainGrid);
    //}

    //private void AboutButton_Click(object sender, RoutedEventArgs e)
    //{
    //    App.ShowAboutDialog(this);
    //}

    private void SettingsMenuItem_Click(object sender, RoutedEventArgs e)
    {
        App.ShowSettingsWindow(Clock, MainGrid);
    }

    private void AboutMenuItem_Click(object sender, RoutedEventArgs e)
    {
        App.ShowAboutDialog(this);
    }

    private void ExitMenuItem_Click(object sender, RoutedEventArgs e)
    {
        Behaviors.TrayIconBehavior.TrayIcon?.Dispose();
        System.Windows.Application.Current.Shutdown();
    }
}