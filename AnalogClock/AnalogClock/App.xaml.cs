using System.Configuration;
using System.Data;
using System.Windows;
using AnalogClock.Dialogs;

namespace AnalogClock;

#region Задачи (TODO)

#endregion

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
    /// Возвращает или задаёт окно свойств часов.
    /// </summary>
    public static DebugClockWindow? DebugClockWindow { get; set; }

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
}
