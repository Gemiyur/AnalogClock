using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using AnalogClock.Controls;

namespace AnalogClock.Dialogs;

/// <summary>
/// Окно настроек приложения.
/// </summary>
public partial class SettingsWindow : Window
{
    private readonly ClockControl clock;

    public SettingsWindow(ClockControl clock)
    {
        InitializeComponent();
        this.clock = clock;
        DataContext = clock;
    }

    private void Window_Closed(object sender, EventArgs e)
    {
        App.SettingsWindow = null;
    }
}
