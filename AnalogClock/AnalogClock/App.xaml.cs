using AnalogClock.Dialogs;
using System.Windows;
using System.Windows.Media;

namespace AnalogClock;

/// <summary>
/// Класс приложения.
/// </summary>
public partial class App : System.Windows.Application
{
    private readonly Mutex mutex = new(false, "AnalogClock");

    /// <summary>
    /// Возвращает или задаёт окно настроек приложения.
    /// </summary>
    public static SettingsWindow? SettingsWindow { get; set; }

    /// <summary>
    /// Возвращает цвет System.Drawing.Color указанной кисти SolidColorBrush.
    /// </summary>
    /// <param name="brush">Кисть SolidColorBrush.</param>
    /// <returns>Цвет System.Drawing.Color.</returns>
    public static System.Drawing.Color BrushToColor(SolidColorBrush brush) =>
        System.Drawing.Color.FromArgb(brush.Color.A, brush.Color.R, brush.Color.G, brush.Color.B);

    /// <summary>
    /// Возвращает кисть SolidColorBrush для указанного цвета System.Drawing.Color.
    /// </summary>
    /// <param name="color">Цвет System.Drawing.Color.</param>
    /// <returns>Кисть SolidColorBrush.</returns>
    public static SolidColorBrush ColorToBrush(System.Drawing.Color color) =>
        new(System.Windows.Media.Color.FromArgb(color.A, color.R, color.G, color.B));

    /// <summary>
    /// Возвращает главное окно приложения.
    /// </summary>
    /// <returns>Главное окно приложения.</returns>
    public static MainWindow GetMainWindow() => (MainWindow)Current.MainWindow;

    /// <summary>
    /// Возвращает окно "О программе" или null, если окна нет.
    /// </summary>
    /// <returns>Окно "О программе" или null, если окна нет.</returns>
    public static AboutDialog? FindAboutWindow()
    {
        foreach (var window in Current.Windows)
            if (window is AboutDialog aboutWindow)
                return aboutWindow;
        return null;
    }

    /// <summary>
    /// Отображает окно "О программе".
    /// </summary>
    /// <param name="owner">Окно-владелец.</param>
    /// <remarks>
    /// Отображает окно как из главного окна, так и из меню значка в области уведомлений, без конфликта.
    /// </remarks>
    public static void ShowAboutDialog(Window owner)
    {
        var window = FindAboutWindow();
        if (window != null)
        {
            window.Activate();
        }
        else
        {
            new AboutDialog() { Owner = owner }.ShowDialog();
        }
    }

    /// <summary>
    /// Отображает окно настроек приложения.
    /// </summary>
    /// <param name="mainWindow">Главное окно.</param>
    public static void ShowSettingsWindow(MainWindow mainWindow)
    {
        if (SettingsWindow == null)
        {
            SettingsWindow = new SettingsWindow(mainWindow);
            SettingsWindow.Show();
        }
        else
            SettingsWindow.Activate();
    }

    private void Application_Startup(object sender, StartupEventArgs e)
    {
        if (!mutex.WaitOne(500, false))
        {
            System.Windows.MessageBox.Show("Приложение уже запущено.", "Часы");
            Environment.Exit(0);
        }
    }
}
