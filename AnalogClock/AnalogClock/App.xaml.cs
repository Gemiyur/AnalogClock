using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using AnalogClock.Controls;
using AnalogClock.Dialogs;

namespace AnalogClock;

/// <summary>
/// Класс приложения.
/// </summary>
public partial class App : System.Windows.Application
{
    /// <summary>
    /// Возвращает или задаёт окно "О программе".
    /// </summary>
    public static AboutDialog? AboutDialog { get; set; }

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
        new SolidColorBrush(System.Windows.Media.Color.FromArgb(color.A, color.R, color.G, color.B));

    /// <summary>
    /// Отображает окно "О программе".
    /// </summary>
    /// <param name="owner">Окно-владелец.</param>
    /// <remarks>
    /// Отображает окно как из главного окна, так и из меню значка в области уведомлений, без конфликта.
    /// </remarks>
    public static void ShowAboutDialog(Window owner)
    {
        if (AboutDialog != null)
        {
            AboutDialog.Activate();
        }
        else
        {
            AboutDialog = new AboutDialog() { Owner = owner };
            AboutDialog.ShowDialog();
        }
    }

    /// <summary>
    /// Отображает окно настроек приложения.
    /// </summary>
    /// <param name="clock">Элемент управления "Часы".</param>
    public static void ShowSettingsWindow(ClockControl clock, Grid grid)
    {
        if (SettingsWindow == null)
        {
            SettingsWindow = new SettingsWindow(clock, grid);
            SettingsWindow.Show();
        }
        else
            SettingsWindow.Activate();
    }
}
