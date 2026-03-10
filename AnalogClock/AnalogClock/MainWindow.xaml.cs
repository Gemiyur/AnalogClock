using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media;

namespace AnalogClock;

/// <summary>
/// Класс главного окна.
/// </summary>
public partial class MainWindow : Window, INotifyPropertyChanged
{
    public MainWindow()
    {
        InitializeComponent();
        ShowInTaskbar = Properties.Settings.Default.ShowInTaskbar;
        GridContextMenu.DataContext = Clock;
    }

    public int CanvasSize
    {
        get => (int)MainCanvas.Width;
        set
        {
            MainCanvas.Height = value;
            MainCanvas.Width = value;
            ClockViewbox.Height = value;
            ClockViewbox.Width = value;
            OnPropertyChanged("CanvasSize");
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    public void OnPropertyChanged([CallerMemberName] string property = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
    }


    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        Left = Properties.Settings.Default.WindowPoint.X;
        Top = Properties.Settings.Default.WindowPoint.Y;
        CanvasSize = Properties.Settings.Default.CanvasSize;
        Clock.BackgroundBrush = App.ColorToBrush(Properties.Settings.Default.ClockBackgroundColor);
        Clock.IsDigitsShown = Properties.Settings.Default.ClockShowDigits;
    }

    private void Window_Closed(object sender, EventArgs e)
    {
        Properties.Settings.Default.WindowPoint = new System.Drawing.Point((int)Left, (int)Top);
        Properties.Settings.Default.CanvasSize = CanvasSize;
        Properties.Settings.Default.ClockBackgroundColor = App.BrushToColor(Clock.BackgroundBrush);
        Properties.Settings.Default.ClockShowDigits = Clock.IsDigitsShown;
        Properties.Settings.Default.Save();
    }

    private void Window_StateChanged(object sender, EventArgs e)
    {
        if (WindowState == WindowState.Minimized)
        {
            App.CloseSettingsWindow();
        }
    }

    private void Window_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
        if (Visibility == Visibility.Hidden)
        {
            App.CloseSettingsWindow();
        }
    }

    private void Window_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        DragMove();
    }

    private void SettingsMenuItem_Click(object sender, RoutedEventArgs e) => App.ShowSettingsWindow();

    private void AboutMenuItem_Click(object sender, RoutedEventArgs e) => App.ShowAboutDialog();

    private void ExitMenuItem_Click(object sender, RoutedEventArgs e)
    {
        Behaviors.TrayIconBehavior.TrayIcon?.Dispose();
        System.Windows.Application.Current.Shutdown();
    }
}