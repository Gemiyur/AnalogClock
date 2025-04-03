using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using Brushes = System.Windows.Media.Brushes;
using Point = System.Windows.Point;
using Size = System.Windows.Size;

namespace AnalogClock.Controls;

// TODO: Сделать варианты отображения цифр на циферблате: не отображать, арабские цифры, римские цифры.

/// <summary>
/// Элемент управления "Часы".
/// </summary>
public class ClockControl : System.Windows.Controls.Control
{
    /// <summary>
    /// Таймер.
    /// </summary>
    private readonly DispatcherTimer timer = new() { Interval = TimeSpan.FromMilliseconds(100) };

    /// <summary>
    /// Время последнего срабатывания обработчика события таймера.
    /// </summary>
    private DateTime lastTick = DateTime.Now;

    ///// <summary>
    ///// Могут ли отображаться цифры на циферблате.
    ///// </summary>
    //private readonly bool canDigitsShown;

    ///// <summary>
    ///// Нужно ли отображать цифры на циферблате.
    ///// </summary>
    //private bool needDigitsShown = true;

    /// <summary>
    /// Шрифт циферблата.
    /// </summary>
    private readonly Typeface hourFont;

    /// <summary>
    /// Глиф шрифта циферблата.
    /// </summary>
    private readonly GlyphTypeface? hourFontGlyph;

    /// <summary>
    /// Возвращает или задаёт отображаемое на часах время.
    /// </summary>
    public DateTime DateTime
    {
        get => (DateTime)GetValue(DateTimeProperty);
        set => SetValue(DateTimeProperty, value);
    }

    /// <summary>
    /// Возвращает или задаёт радиус для отрисовки цифр на циферблате.
    /// </summary>
    public double HourRadius
    {
        get => (double)GetValue(HourRadiusProperty);
        set => SetValue(HourRadiusProperty, value);
    }

    ///// <summary>
    ///// Отображаются ли цифры на циферблате.
    ///// </summary>
    //public bool IsDigitsShown
    //{
    //    get => canDigitsShown && needDigitsShown;
    //    set => needDigitsShown = value;
    //}

    /// <summary>
    /// Возвращает или задаёт отображение римских чисел вместо арабских.
    /// </summary>
    public bool IsRomanDigits
    {
        get => (bool)GetValue(IsRomanDigitsProperty);
        set => SetValue(IsRomanDigitsProperty, value);
    }

    /// <summary>
    /// Возвращает или задаёт работают ли часы или они остановлены.
    /// </summary>
    /// <remarks>
    /// true - часы работают<br/>
    /// false - часы остановлены 
    /// </remarks>
    public bool IsRunning
    {
        get => (bool)GetValue(IsRunningProperty);
        set => SetValue(IsRunningProperty, value);
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <exception cref="InvalidOperationException"></exception>
    public ClockControl()
    {
        hourFont = new Typeface(FontFamily, FontStyle, FontWeight, FontStretch);
        if (!hourFont.TryGetGlyphTypeface(out hourFontGlyph))
        {
            // Тут, возможно, будут какие-то действия, если глиф для шрифта не найден.
        }

        // Это пока оставлено для отладки.
        //hourFontGlyph = null;

        timer.Tick += Timer_Tick;
    }

    /// <summary>
    /// Инициализирует элемент управления. Запускает таймер.
    /// </summary>
    protected override void OnInitialized(EventArgs e)
    {
        base.OnInitialized(e);
        ToggleTimer(IsRunning);
    }

    /// <summary>
    /// Вызывается после применения шаблона элемента управления.
    /// </summary>
    /// <remarks>
    /// Здесь можно манипулировать шаблоном элемента управления на лету по своему усмотрению.<br/>
    /// В данном случае рисуются числа на циферблате заданным шрифтом и цветом.<br/>
    /// Это можно было бы сделать и в XAML, но в данном проекте предпочтительней это делать в коде.
    /// </remarks>
    public override void OnApplyTemplate()
    {
        // TODO: Возможно прорисовку цифр на циферблате следует вынести в отдельный метод.
        if (hourFontGlyph == null)
            return;
        string[] romanDigits = ["I", "II", "III", "IV", "V", "VI", "VII", "VIII", "IX", "X", "XI", "XII"];
        var labels = ((DrawingGroup)Template.FindName("ClockGlyphsContainer", this)).Children.OfType<GlyphRunDrawing>();
        double innerOffset = (50 - HourRadius) + 1;
        double innerCircleDiameter = HourRadius * 2;
        double fontSize = FontSize;
        int index = 1;
        foreach (var label in labels)
        {
            //var text = index.ToString();
            var text = IsRomanDigits ? romanDigits[index - 1] : index.ToString();
            var point = GetHourPosition(index);
            var size = MeasureString(text, fontSize);
            point.X -= ((point.X - innerOffset) / innerCircleDiameter) * size.Width;
            point.Y += (1.0 - ((point.Y - innerOffset) / innerCircleDiameter)) * size.Height;
            label.GlyphRun = CreateGlyphRun(text, point, fontSize);
            label.ForegroundBrush = Brushes.DarkRed;
            index++;
        }
    }

    /// <summary>
    /// Возвращает объект GlyphRun для указанного текста в указанной точке с указанным размером шрифта.
    /// </summary>
    /// <param name="text">Строка.</param>
    /// <param name="point">Верхняя левая точка.</param>
    /// <param name="fontSize">Размер шрифта.</param>
    /// <returns>Объект GlyphRun.</returns>
    private GlyphRun? CreateGlyphRun(string text, Point point, double fontSize)
    {
        if (hourFontGlyph == null)
            return null;
        var glyphIndexes = new ushort[text.Length];
        var advanceWidths = new double[text.Length];
        for (var i = 0; i < text.Length; i++)
        {
            var glyphIndex = hourFontGlyph.CharacterToGlyphMap[text[i]];
            glyphIndexes[i] = glyphIndex;
            var width = hourFontGlyph.AdvanceWidths[glyphIndex] * fontSize;
            advanceWidths[i] = width;
        }

        // Значение пикселя, не зависящее от плотности, которое эквивалентно масштабному коэффициенту экрана.
        // Поэкспериментировал. Разницы при разных коэффициентах никакой не заметил. Поставил 1.25 (свой).
        // Возможные варианты:
        //var pixelsPerDip = 1f;  // масштаб 100%
        var pixelsPerDip = 1.25f;  // масштаб 125% (мой)
        //var pixelsPerDip = 1.5f;  // масштаб 150%
        //var pixelsPerDip = 1.75f;  // масштаб 175%

        return new GlyphRun(hourFontGlyph, 0, false, fontSize, pixelsPerDip, glyphIndexes,
                            point, advanceWidths, null, null, null, null, null, null);
    }

    /// <summary>
    /// Возвращает точку верхнего левого угла текста указанного часа на циферблате.
    /// </summary>
    /// <param name="hour">Час. Значение от 1 до 12.</param>
    /// <returns>Точка верхнего левого угла текста часа на циферблате.</returns>
    private Point GetHourPosition(int hour)
    {
        var angle = (Math.PI / 180) * ((30.0 * hour) - 90);
        return new Point(HourRadius * Math.Cos(angle) + 50, HourRadius * Math.Sin(angle) + 50);
    }

    /// <summary>
    /// Возвращает размер указанной строки с указанным размером шрифта. 
    /// </summary>
    private Size MeasureString(string text, double fontSize)
    {
        if (hourFontGlyph == null)
            return new();
        var size = new Size();
        for (var i = 0; i < text.Length; i++)
        {
            var glyph = hourFontGlyph.CharacterToGlyphMap[text[i]];
            size.Width += hourFontGlyph.AdvanceWidths[glyph] * fontSize;
            size.Height = Math.Max(hourFontGlyph.AdvanceHeights[glyph] * (fontSize * (72.0 / 96.0)), size.Height);
        }
        return size;
    }

    /// <summary>
    /// Запускает или останавливает таймер.
    /// </summary>
    /// <param name="run">Запустить или остановить таймер: true - запустить, false - остановить.</param>
    /// <remarks>
    ///  run = true - запустить<br/>
    ///  run = false - остановить
    /// </remarks>
    private void ToggleTimer(bool run)
    {
        timer.IsEnabled = run;
    }

    /// <summary>
    /// Обработчик события срабатывания таймера часов.
    /// </summary>
    private void Timer_Tick(object? sender, EventArgs e)
    {
        DateTime now = DateTime.Now;
        if (now.Second != lastTick.Second)
        {
            lastTick = now;
            DateTime = now.AddMilliseconds(-now.Millisecond);
        }
    }

    /// <summary>
    /// Обновляет шаблон (применяет шаблон заново).
    /// </summary>
    private void UpdateTemplate()
    {
        var template = Template;
        Template = null;
        Template = template;
    }

    internal static DependencyProperty DateTimeProperty = DependencyProperty.Register(
            "DateTime",
            typeof(DateTime),
            typeof(ClockControl),
            new PropertyMetadata(DateTime.Now, new PropertyChangedCallback(OnDateTimePropertyChanged)));

    internal static DependencyProperty HourRadiusProperty = DependencyProperty.Register(
            "HourRadius",
            typeof(double),
            typeof(ClockControl),
            new PropertyMetadata(36.0));

    internal static DependencyProperty IsRunningProperty = DependencyProperty.Register(
            "IsRunning",
            typeof(bool),
            typeof(ClockControl),
            new PropertyMetadata(true, new PropertyChangedCallback(OnIsRunningPropertyChanged)));

    private static void OnDateTimePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var clock = (ClockControl)d;
        var oldValue = (DateTime)e.OldValue;
        var newValue = (DateTime)e.NewValue;
        if (oldValue.Second != newValue.Second)
            clock.OnDateTimeChanged(oldValue, newValue.AddMilliseconds(-newValue.Millisecond));
    }

    private static void OnIsRunningPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var clock = (ClockControl)d;
        var oldValue = (bool)e.OldValue;
        var newValue = (bool)e.NewValue;
        if (oldValue != newValue)
            clock.OnIsRunningChanged(oldValue, newValue);
    }

    public static readonly RoutedEvent DateTimeChangedEvent =
        EventManager.RegisterRoutedEvent(
            "DateTimeChanged",
            RoutingStrategy.Bubble,
            typeof(RoutedPropertyChangedEventHandler<DateTime>),
            typeof(ClockControl));

    public static readonly RoutedEvent RunningChangedEvent =
        EventManager.RegisterRoutedEvent(
            "RunningChanged",
            RoutingStrategy.Bubble,
            typeof(RoutedPropertyChangedEventHandler<bool>),
            typeof(ClockControl));

    protected virtual void OnDateTimeChanged(DateTime oldValue, DateTime newValue)
    {
        var args = new RoutedPropertyChangedEventArgs<DateTime>(oldValue, newValue)
        {
            RoutedEvent = DateTimeChangedEvent
        };
        RaiseEvent(args);
    }

    protected virtual void OnIsRunningChanged(bool oldValue, bool newValue)
    {
        ToggleTimer(newValue);
        var args = new RoutedPropertyChangedEventArgs<bool>(oldValue, newValue)
        {
            RoutedEvent = RunningChangedEvent
        };
        RaiseEvent(args);
    }

    // Римские цифры.

    internal static DependencyProperty IsRomanDigitsProperty = DependencyProperty.Register(
            "IsRomanDigits",
            typeof(bool),
            typeof(ClockControl),
            new PropertyMetadata(true, new PropertyChangedCallback(OnIsRomanDigitsPropertyChanged)));

    private static void OnIsRomanDigitsPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var clock = (ClockControl)d;
        var oldValue = (bool)e.OldValue;
        var newValue = (bool)e.NewValue;
        if (oldValue != newValue)
            clock.OnIsRomanDigitsChanged(oldValue, newValue);
    }

    public static readonly RoutedEvent RomanDigitsChangedEvent =
        EventManager.RegisterRoutedEvent(
            "RomanDigitsChanged",
            RoutingStrategy.Bubble,
            typeof(RoutedPropertyChangedEventHandler<bool>),
            typeof(ClockControl));

    protected virtual void OnIsRomanDigitsChanged(bool oldValue, bool newValue)
    {
        UpdateTemplate();
        var args = new RoutedPropertyChangedEventArgs<bool>(oldValue, newValue)
        {
            RoutedEvent = RomanDigitsChangedEvent
        };
        RaiseEvent(args);
    }

}
