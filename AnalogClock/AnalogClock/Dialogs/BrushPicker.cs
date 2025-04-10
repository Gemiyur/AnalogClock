using System.Windows.Media;

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
        var dialog = new ColorDialog { Color = App.BrushToColor(Brush) };
        if (dialog.ShowDialog() != DialogResult.OK)
            return false;
        Brush = App.ColorToBrush(dialog.Color);
        return true;
    }
}
