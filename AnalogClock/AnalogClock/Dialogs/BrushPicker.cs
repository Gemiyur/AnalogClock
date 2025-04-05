using System.Windows.Media;
using Brush = System.Windows.Media.Brush;
using Color = System.Windows.Media.Color;

namespace AnalogClock.Dialogs;

/// <summary>
/// Класс выбора кисти через стандартный диалог выбора цвета.
/// </summary>
/// <param name="brush">Текущая кисть.</param>
public class BrushPicker(SolidColorBrush brush)
{
    /// <summary>
    /// Возвращает или задаёт кисть.
    /// </summary>
    public SolidColorBrush Brush { get; set; } = brush;

    /// <summary>
    /// Выполняет выбор кисти через стандартный диалог выбора цвета.
    /// </summary>
    /// <returns>Была ли выбрана кисть.</returns>
    public bool Execute()
    {
        var dialogColor = System.Drawing.Color.FromArgb(Brush.Color.A, Brush.Color.R, Brush.Color.G, Brush.Color.B);
        var dialog = new ColorDialog { Color = dialogColor };
        if (dialog.ShowDialog() != DialogResult.OK)
            return false;
        Brush = new SolidColorBrush(Color.FromArgb(dialog.Color.A, dialog.Color.R, dialog.Color.G, dialog.Color.B));
        return true;
    }
}
