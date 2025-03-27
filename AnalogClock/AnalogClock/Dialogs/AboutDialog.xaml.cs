using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

namespace AnalogClock.Dialogs;

/// <summary>
/// Класс окна "О программе".
/// </summary>
public partial class AboutDialog : Window
{
    public AboutDialog()
    {
        InitializeComponent();

        var assembly = Assembly.GetExecutingAssembly();
        var array = assembly.GetName().Version.ToString().Split('.');
        var version = $"Версия: {array[0]}.{array[1]}.{array[2]}";
        var product =
            ((AssemblyProductAttribute)assembly.GetCustomAttributes(typeof(AssemblyProductAttribute), true)
            .First()).Product;
        var description =
            ((AssemblyDescriptionAttribute)assembly.GetCustomAttributes(typeof(AssemblyDescriptionAttribute), true)
            .First()).Description.Replace("\r\n", "\n");
        var copyright =
            ((AssemblyCopyrightAttribute)assembly.GetCustomAttributes(typeof(AssemblyCopyrightAttribute), true)
            .First()).Copyright;

        ProductTextBlock.Text = product;
        DescriptionTextBlock.Text = description;
        CopyrightTextBlock.Text = copyright;
        VersionTextBlock.Text = version;
    }

    private void Window_Closed(object sender, EventArgs e)
    {
        App.AboutDialog = null;
    }

    private void CloseButton_Click(object sender, RoutedEventArgs e) => Close();
}
