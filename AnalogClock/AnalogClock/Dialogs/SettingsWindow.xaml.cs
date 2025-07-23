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

    private void LocateWindow()
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
        var ratio = screen.Bounds.Width / SystemParameters.PrimaryScreenWidth;

        // Позиция и ширина главного окна на экране.
        var mainLeft = (int)(mainWindow.Left * ratio);
        var mainTop = (int)(mainWindow.Top * ratio);
        var mainWidth = (int)(mainWindow.Width * ratio);

        // Ширина свободного места рабочей области слева и справа от главного окна.
        var freeLeft = mainLeft - area.Left;
        var freeRight = area.Right - (mainLeft + mainWidth);

        // Позиция и размер окна настроек на экране.
        int left;
        int top;
        var width = (int)(Width * ratio);
        var height = (int)(Height * ratio);

        // Получаем из настроек с какой стороны главного окна показывать окно настроек.
        var showRight = Properties.Settings.Default.SettingsOnRight;

        // Определяем координату X (левую) верхней левой точки окна настроек на экране.
        if (showRight)
        {
            left = freeRight < width ? area.Right - width : mainLeft + mainWidth;
        }
        else
        {
            left = freeLeft < width ? area.Left : mainLeft - width;
        }

        // Определяем координату Y (верхнюю) верхней левой точки окна настроек на экране.
        top = mainTop + height <= area.Bottom ? mainTop : area.Bottom - height;

        // Устанавливаем значения верхней левой точки окна настроек.
        Left = left / ratio;
        Top = top / ratio;
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        LocateWindow();
    }

    private void Window_Closed(object sender, EventArgs e)
    {
        var mainWindow = App.GetMainWindow();
        if (mainWindow != null)
            mainWindow.Activate();
    }

    private void DigitsColorButton_Click(object sender, RoutedEventArgs e)
    {
        var picker = new BrushPicker(clock.DigitBrush);
        if (picker.Execute())
            clock.DigitBrush = picker.Brush;
    }

    private void ShowInTaskbarCheckBox_Click(object sender, RoutedEventArgs e) => CheckShowInTaskbar();

    private void SettingsOnRightCheckBox_Click(object sender, RoutedEventArgs e) => LocateWindow();

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
        Properties.Settings.Default.SettingsOnRight = Properties.Settings.Default.DefaultSettingsOnRight;
        Properties.Settings.Default.ShowInTaskbar = Properties.Settings.Default.DefaultShowInTaskbar;
        clock.DigitBrush = App.ColorToBrush(Properties.Settings.Default.DefaultClockDigitsColor);
        clock.IsDigitsShown = Properties.Settings.Default.DefaultClockShowDigits;
        clock.IsTransparent = Properties.Settings.Default.DefaultClockTransparent;
        CheckShowInTaskbar();
        LocateWindow();
    }
}
