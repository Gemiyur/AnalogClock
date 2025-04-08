using System.Windows;
using AnalogClock.Controls;

namespace AnalogClock.Dialogs;

/// <summary>
/// Окно отладки свойств часов.
/// </summary>
public partial class DebugClockWindow : Window
{
    private readonly ClockControl clock;

    public DebugClockWindow(ClockControl clock)
    {
        InitializeComponent();
        this.clock = clock;
        DataContext = clock;
        InitializeCheckBoxes();
    }

    private void InitializeCheckBoxes()
    {
    }

    private void Window_Closed(object sender, EventArgs e)
    {
        App.DebugClockWindow = null;
    }

    private void DigitsColorButton_Click(object sender, RoutedEventArgs e)
    {
        var picker = new BrushPicker(clock.DigitBrush);
        if (!picker.Execute())
            return;
        clock.DigitBrush = picker.Brush;
    }
}
