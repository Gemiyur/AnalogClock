using Microsoft.Xaml.Behaviors;
using System.Windows;

namespace AnalogClock.Behaviors;

/// <summary>
/// Класс поведения значка в области уведомлений.
/// </summary>
public class TrayIconBehavior : Behavior<Window>
{
    private NotifyIcon? trayIcon;

    /// <summary>
    /// Скрывать окно приложения, а не закрывать.
    /// </summary>
    private static bool CloseToTray => Properties.Settings.Default.CloseToTray;

    /// <summary>
    /// Скрывать окно приложения, а не сворачивать.
    /// </summary>
    private static bool MinimizeToTray => Properties.Settings.Default.MinimizeToTray;

    /// <summary>
    /// Показывать приложение в панели задач.
    /// </summary>
    private static bool ShowInTaskbar => Properties.Settings.Default.ShowInTaskbar;

    protected override void OnAttached()
    {
        base.OnAttached();
        AssociatedObject.Loaded += AssociatedObject_Loaded;
        AssociatedObject.Closing += AssociatedObject_Closing;
        AssociatedObject.StateChanged += AssociatedObject_StateChanged;
    }

    protected override void OnDetaching()
    {
        base.OnDetaching();
        AssociatedObject.Loaded -= AssociatedObject_Loaded;
        AssociatedObject.Closing -= AssociatedObject_Closing;
        AssociatedObject.StateChanged -= AssociatedObject_StateChanged;
    }

    private void AssociatedObject_Loaded(object sender, RoutedEventArgs e)
    {
        InitializeTrayIcon();
    }

    private void AssociatedObject_Closing(object? sender, System.ComponentModel.CancelEventArgs e)
    {
        if (CloseToTray)
        {
            e.Cancel = true;
            AssociatedObject.Hide();
        }
        else
        {
            trayIcon?.Dispose();
        }
    }

    private void AssociatedObject_StateChanged(object? sender, EventArgs e)
    {
        if (AssociatedObject.WindowState == WindowState.Minimized && (MinimizeToTray || !ShowInTaskbar))
        {
            AssociatedObject.Hide();
        }
        else
        {
            AssociatedObject.ShowInTaskbar = ShowInTaskbar;
        }
    }

    private void InitializeTrayIcon()
    {
        trayIcon = new NotifyIcon
        {
            Icon = GetIcon(new Uri("Images/TrayIcon.ico", UriKind.Relative)),
            Text = "Часы со стрелками",
            Visible = true
        };

        ContextMenuStrip contextMenu = new();

        ToolStripMenuItem openMenuItem = new()
        {
            Text = "Открыть"
        };
        var font = new Font(openMenuItem.Font, System.Drawing.FontStyle.Bold);
        openMenuItem.Font = font;
        contextMenu.Items.Add(openMenuItem);
        openMenuItem.Click += OpenMenuItem_Click;

        contextMenu.Items.Add(new ToolStripSeparator());

        ToolStripMenuItem aboutMenuItem = new()
        {
            Text = "О программе..."
        };
        contextMenu.Items.Add(aboutMenuItem);
        aboutMenuItem.Click += AboutMenuItem_Click;

        contextMenu.Items.Add(new ToolStripSeparator());

        ToolStripMenuItem exitMenuItem = new()
        {
            Text = "Выход"
        };
        contextMenu.Items.Add(exitMenuItem);
        exitMenuItem.Click += ExitMenuItem_Click;

        trayIcon.ContextMenuStrip = contextMenu;
        trayIcon.MouseClick += TrayIcon_MouseClick;
    }

    private void TrayIcon_MouseClick(object? sender, MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Left)
            ShowAssociatedWindow();
    }

    private void AboutMenuItem_Click(object? sender, EventArgs e)
    {
        App.ShowAboutDialog(AssociatedObject);
    }

    private void OpenMenuItem_Click(object? sender, EventArgs e)
    {
        ShowAssociatedWindow();
    }

    private void ExitMenuItem_Click(object? sender, EventArgs e)
    {
        trayIcon?.Dispose();
        System.Windows.Application.Current.Shutdown();
    }

    private void ShowAssociatedWindow()
    {
        AssociatedObject.Show();
        AssociatedObject.WindowState = WindowState.Normal;
        AssociatedObject.Activate();
    }

    // Отображение системных уведомлений. Не используется в данном проекте.
    // Оставлено на всякий случай. Ну и как пример тоже.
    //private void ShowNotificationInTray(string title, string message)
    //{
    //    // Отображает системное уведомление на 2 секунды (2000 миллисекунд).
    //    trayIcon?.ShowBalloonTip(2000, title, message, ToolTipIcon.Info);
    //}

    private static Icon? GetIcon(Uri uri)
    {
        using var stream = System.Windows.Application.GetResourceStream(uri)?.Stream;
        return stream != null ? new Icon(stream) : null;
    }
}
