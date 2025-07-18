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
        Clock.IsTransparent = Properties.Settings.Default.ClockTransparent;
        if (Properties.Settings.Default.SaveLocation)
        {
            Left = Properties.Settings.Default.WindowPoint.X;
            Top = Properties.Settings.Default.WindowPoint.Y;
            Width = Properties.Settings.Default.WindowSize.Width;
            Height = Properties.Settings.Default.WindowSize.Height;
        }
    }

    private void Window_Closed(object sender, EventArgs e)
    {
        Properties.Settings.Default.BackgroundColor = App.BrushToColor((SolidColorBrush)MainGrid.Background);
        if (Properties.Settings.Default.SaveLocation)
        {
            Properties.Settings.Default.WindowPoint = new System.Drawing.Point((int)Left, (int)Top);
            Properties.Settings.Default.WindowSize = new System.Drawing.Size((int)Width, (int)Height);
        }
        Properties.Settings.Default.ClockDigitsColor = App.BrushToColor(Clock.DigitBrush);
        Properties.Settings.Default.ClockShowDigits = Clock.IsDigitsShown;
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

    private void SettingsMenuItem_Click(object sender, RoutedEventArgs e) => App.ShowSettingsWindow(this);

    private void AboutMenuItem_Click(object sender, RoutedEventArgs e) => App.ShowAboutDialog(this);

    private void ExitMenuItem_Click(object sender, RoutedEventArgs e)
    {
        Behaviors.TrayIconBehavior.TrayIcon?.Dispose();
        System.Windows.Application.Current.Shutdown();
    }
}