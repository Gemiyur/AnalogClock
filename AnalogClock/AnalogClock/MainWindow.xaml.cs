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

    private void StartClockButton_Click(object sender, RoutedEventArgs e)
    {
        Clock.IsRunning = true;
    }

    private void StopClockButton_Click(object sender, RoutedEventArgs e)
    {
        Clock.IsRunning = false;
    }

    private void ArabicButton_Click(object sender, RoutedEventArgs e)
    {
        Clock.IsRomanDigits = false;
    }

    private void RomanButton_Click(object sender, RoutedEventArgs e)
    {
        Clock.IsRomanDigits = true;
    }

    private void DigitsButton_Click(object sender, RoutedEventArgs e)
    {
        Clock.IsDigitsShown = true;
    }

    private void NoDigitsButton_Click(object sender, RoutedEventArgs e)
    {
        Clock.IsDigitsShown = false;
    }

    private void BackgroundButton_Click(object sender, RoutedEventArgs e)
    {
        Clock.IsTransparent = false;
    }

    private void TransparentButton_Click(object sender, RoutedEventArgs e)
    {
        Clock.IsTransparent = true;
    }

    private void RedButton_Click(object sender, RoutedEventArgs e)
    {
        Clock.DigitBrush = System.Windows.Media.Brushes.DarkRed;
    }

    private void BlackButton_Click(object sender, RoutedEventArgs e)
    {
        Clock.DigitBrush = System.Windows.Media.Brushes.Black;
    }

    private void ColorButton_Click(object sender, RoutedEventArgs e)
    {
        //var brush = ClockControl.DefaultDigitBrush;
        var dialog = new ColorDialog();
        if (dialog.ShowDialog() != System.Windows.Forms.DialogResult.OK)
            return;
        var color = System.Windows.Media.Color.FromArgb(dialog.Color.A, dialog.Color.R, dialog.Color.G, dialog.Color.B);
        Clock.DigitBrush = new System.Windows.Media.SolidColorBrush(color);

    }
}