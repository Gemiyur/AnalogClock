using System.Configuration;
using System.Data;
using System.Windows;
using AnalogClock.Dialogs;

namespace AnalogClock;

#region Задачи (TODO)

// TODOL: Сделать значки приложения и панели уведомлений.

#endregion

/// <summary>
/// Класс приложения.
/// </summary>
public partial class App : System.Windows.Application
{
    public static AboutDialog? AboutDialog { get; set; }
}
