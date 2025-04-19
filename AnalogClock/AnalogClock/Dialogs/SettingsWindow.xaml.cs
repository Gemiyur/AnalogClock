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
    /// Главное окно.
    /// </summary>
    private readonly MainWindow mainWindow;

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
        this.mainWindow = mainWindow;
        clock = mainWindow.Clock;
        grid = mainWindow.MainGrid;
        ClockGroupBox.DataContext = clock;
        AppGroupBox.DataContext = Properties.Settings.Default;
    }

    private void CheckShowInTaskbar() =>
        System.Windows.Application.Current.MainWindow.ShowInTaskbar = ShowInTaskbarCheckBox.IsChecked == true;

    //public static int GetWindowsScalePercent()
    //{
    //    return (int)(100 * Screen.PrimaryScreen.Bounds.Width / SystemParameters.PrimaryScreenWidth);
    //}

    //public static int GetWindowsScalePercent(Screen screen)
    //{
    //    return (int)(100 * screen.Bounds.Width / SystemParameters.PrimaryScreenWidth);
    //}

    //public static double GetWindowsScaleRatio()
    //{
    //    return Screen.PrimaryScreen.Bounds.Width / SystemParameters.PrimaryScreenWidth;
    //}

    //public static double GetWindowsScaleRatio(Screen screen)
    //{
    //    return screen.Bounds.Width / SystemParameters.PrimaryScreenWidth;
    //}

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        // Важно!
        // Позиция верхней левой точки окна задаётся относительно всего экрана, а не рабочей области.
        // Позиция, ширина и высота окна на экране зависят масштаба экрана.
        // При получении значений надо умножать на коэффициент масштаба экрана.
        // При установке значений надо делить на коэффициент масштаба экрана.
        // Если масштаб экрана 125%, то, соответственно, коэффициент масштаба экрана 1.25.
        // Позиция верхней левой точки рабочей области так же задаётся относительно всего экрана.
        // От масштаба экрана не зависят. Это физические размеры экрана и рабочей области в пикселях.

        var screen = Screen.FromHandle(new System.Windows.Interop.WindowInteropHelper(mainWindow).Handle);
        var area = screen.WorkingArea;
        //var bounds = screen.Bounds;
        var ratio = screen.Bounds.Width / SystemParameters.PrimaryScreenWidth;

        // Позиция и ширина главного окна на экране.
        var mainLeft = (int)(mainWindow.Left * ratio);
        var mainTop = (int)(mainWindow.Top * ratio);
        var mainWidth = (int)(mainWindow.Width * ratio);
        //var mainHeight = (int)(mainWindow.Height * ratio);

        // Ширина свободного места рабочей области слева и справа от главного окна.
        var freeLeft = mainLeft - area.Left;
        var freeRight = area.Right - (mainLeft + mainWidth);

        // Позиция и размер окна настроек на экране.
        int left; // = (int)(Left * ratio);
        int top; // = (int)(Top * ratio);
        var width = (int)(Width * ratio);
        var height = (int)(Height * ratio);

        // Определяем координату X (левую) верхней левой точки окна настроек на экране.
        if (freeLeft >= freeRight)
        {
            if (freeLeft < width)
                left = area.Left;
            else
                left = mainLeft - width;
        }
        else
        {
            if (freeRight < width)
                left = area.Right - width;
            else
                left = mainLeft + mainWidth;
        }

        // Определяем координату Y (верхнюю) верхней левой точки окна настроек на экране.
        if (mainTop + height <= area.Bottom)
        {
            top = mainTop;
        }
        else
        {
            top = area.Bottom - height;
        }

        // Устанавливаем значения верхней левой точки окна настроек.
        Left = left / ratio;
        Top = top / ratio;

        //var rectangle = new Rectangle(left, top, width, height);
    }

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
