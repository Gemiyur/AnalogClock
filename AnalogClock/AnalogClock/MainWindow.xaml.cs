using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using AnalogClock.Controls;
using AnalogClock.Dialogs;

namespace AnalogClock;

/// <summary>
/// Класс главного окна.
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void PropertyButton_Click(object sender, RoutedEventArgs e)
    {
        if (App.DebugClockWindow == null)
        {
            App.DebugClockWindow = new DebugClockWindow(Clock);
            App.DebugClockWindow.Show();
        }
        else
            App.DebugClockWindow.Activate();
    }
}