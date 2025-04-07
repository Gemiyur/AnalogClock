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
        InitializeCheckBoxes();
    }

    private void InitializeCheckBoxes()
    {
        IsRunningCheckBox.IsChecked = clock.IsRunning;
        IsTransparentCheckBox.IsChecked = clock.IsTransparent;
        IsDigitsShownCheckBox.IsChecked = clock.IsDigitsShown;
        IsRomanDigitsCheckBox.IsChecked =clock.IsRomanDigits;
    }

    private void Window_Closed(object sender, EventArgs e)
    {
        App.DebugClockWindow = null;
    }

    private void IsRunningCheckBox_Click(object sender, RoutedEventArgs e)
    {
        clock.IsRunning = IsRunningCheckBox.IsChecked == true;
    }

    private void IsTransparentCheckBox_Click(object sender, RoutedEventArgs e)
    {
        clock.IsTransparent = IsTransparentCheckBox.IsChecked == true;
    }

    private void IsDigitsShownCheckBox_Click(object sender, RoutedEventArgs e)
    {
        clock.IsDigitsShown = IsDigitsShownCheckBox.IsChecked == true;
    }

    private void IsRomanDigitsCheckBox_Click(object sender, RoutedEventArgs e)
    {
        clock.IsRomanDigits = IsRomanDigitsCheckBox.IsChecked == true;
    }

    private void DigitsColorButton_Click(object sender, RoutedEventArgs e)
    {
        var picker = new BrushPicker(clock.DigitBrush);
        if (!picker.Execute())
            return;
        clock.DigitBrush = picker.Brush;
    }
}
