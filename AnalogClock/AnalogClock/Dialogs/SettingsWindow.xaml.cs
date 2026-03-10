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
    /// Конструктор.
    /// </summary>
    /// <param name="mainWindow">Главное окно.</param>
    public SettingsWindow(MainWindow mainWindow)
    {
        InitializeComponent();
        clock = mainWindow.Clock;
        ClockGroupBox.DataContext = clock;
        SizeGroupBox.DataContext = mainWindow;
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

        var mainWindow = App.GetMainWindow();
        var screen = Screen.FromHandle(new System.Windows.Interop.WindowInteropHelper(mainWindow).Handle);
        var area = screen.WorkingArea;
        var ratio = screen.Bounds.Width / SystemParameters.PrimaryScreenWidth;

        // Позиция и ширина главного окна на экране.
        var mainLeft = (int)(mainWindow.Left * ratio);
        var mainTop = (int)(mainWindow.Top * ratio);
        var mainWidth = (int)(mainWindow.Width * ratio);

        // Размер окна настроек на экране.
        var width = (int)(Width * ratio);
        var height = (int)(Height * ratio);

        // Определяем координату X (левую) верхней левой точки окна настроек на экране.
        var left = mainLeft - area.Left < width ? mainLeft + mainWidth : mainLeft - width;

        // Определяем координату Y (верхнюю) верхней левой точки окна настроек на экране.
        var top = mainTop + height <= area.Bottom ? mainTop : area.Bottom - height;

        // Устанавливаем значения верхней левой точки окна настроек.
        Left = left / ratio;
        Top = top / ratio;
    }

    private void Window_Loaded(object sender, RoutedEventArgs e) => LocateWindow();

    private void Window_Closed(object sender, EventArgs e) => App.GetMainWindow()?.Activate();

    private void BackgroundColorButton_Click(object sender, RoutedEventArgs e)
    {
        var picker = new BrushPicker(clock.BackgroundBrush);
        if (picker.Execute())
            clock.BackgroundBrush = picker.Brush;
    }

    private void ShowInTaskbarCheckBox_Click(object sender, RoutedEventArgs e) => CheckShowInTaskbar();

    private void ResetButton_Click(object sender, RoutedEventArgs e)
    {
        clock.BackgroundBrush = App.ColorToBrush(Properties.Settings.Default.PresetClockBackgroundColor);
        clock.IsDigitsShown = Properties.Settings.Default.PresetClockShowDigits;
        Properties.Settings.Default.CanvasSize = Properties.Settings.Default.PresetCanvasSize;
        App.GetMainWindow().CanvasSize = Properties.Settings.Default.CanvasSize;
        Properties.Settings.Default.MinimizeToTray = Properties.Settings.Default.PresetMinimizeToTray;
        Properties.Settings.Default.ShowInTaskbar = Properties.Settings.Default.PresetShowInTaskbar;
        CheckShowInTaskbar();
    }
}
