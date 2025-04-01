using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using Brushes = System.Windows.Media.Brushes;
using Point = System.Windows.Point;
using Size = System.Windows.Size;

namespace AnalogClock.Controls;

// TODO: Разобраться с модификаторами доступа.

/// <summary>
/// Элемент управления "Часы".
/// </summary>
public class ClockControl : System.Windows.Controls.Control
{
    /// <summary>
    /// Таймер часов.
    /// </summary>
    private DispatcherTimer? timer = null;

    /// <summary>
    /// Время последнего срабатывания обработчика события таймера.
    /// </summary>
    private DateTime lastTick = DateTime.Now;

    /// <summary>
    /// Шрифт циферблата часов.
    /// </summary>
    private readonly Typeface hourFont;

    /// <summary>
    /// Глиф шрифта циферблата часов.
    /// </summary>
    private readonly GlyphTypeface hourFontGlyph;

    /// <summary>
    /// Возвращает или задаёт отображаемое на часах время.
    /// </summary>
    public DateTime DateTime
    {
        get => (DateTime)GetValue(DateTimeProperty);
        set => SetValue(DateTimeProperty, value);
    }

    /// <summary>
    /// Возвращает или задаёт радиус для отрисовки чисел часов.
    /// </summary>
    public double HourRadius
    {
        get => (double)GetValue(HourRadiusProperty);
        set => SetValue(HourRadiusProperty, value);
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
        // TODO: Надо ли выбрасывать исключение или поступить как-то иначе?
        if (!hourFont.TryGetGlyphTypeface(out hourFontGlyph))
            throw new InvalidOperationException("Глиф для шрифта не найден.");

    }

    /// <summary>
    /// Инициализирует элемент управления. Создаёт и запускает таймер.
    /// </summary>
    protected override void OnInitialized(EventArgs e)
    {
        base.OnInitialized(e);
        // TODO: Может нужно создать конструктор и часть или всё перенести туда?
        InitTimer(IsRunning);
        //hourFont = new Typeface(FontFamily, FontStyle, FontWeight, FontStretch);
        //// TODO: Надо ли выбрасывать исключение или поступить как-то иначе?
        //if (!hourFont.TryGetGlyphTypeface(out hourFontGlyph))
        //    throw new InvalidOperationException("Глиф для шрифта не найден.");
    }

    /// <summary>
    /// Вызывается после применения шаблона элемента управления.
    /// </summary>
    /// <remarks>
    /// Здесь можно манипулировать шаблоном элемента управления на лету по своему усмотрению.<br/>
    /// В данном случае рисуются числа часов на циферблате часов заданным шрифтом и цветом.<br/>
    /// Это можно было бы сделать и в XAML, но в данном проекте предпочтительней это делать в коде.
    /// </remarks>
    public override void OnApplyTemplate()
    {
        // TODO: Возможно реализацию прорисовки часов следует вынести в отдельный метод.
        var labels = ((DrawingGroup)Template.FindName("ClockGlyphsContainer", this)).Children.OfType<GlyphRunDrawing>();
        double innerOffset = (50 - HourRadius) + 1;
        double innerCircleDiameter = HourRadius * 2;
        double fontSize = FontSize;
        int index = 1;
        foreach (var label in labels)
        {
            var text = index.ToString();
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
    private GlyphRun CreateGlyphRun(string text, Point point, double fontSize)
    {
        var glyphIndexes = new ushort[text.Length];
        var advanceWidths = new double[text.Length];

        for (var i = 0; i < text.Length; i++)
        {
            var glyphIndex = hourFontGlyph.CharacterToGlyphMap[text[i]];
            glyphIndexes[i] = glyphIndex;
            var width = hourFontGlyph.AdvanceWidths[glyphIndex] * fontSize;
            advanceWidths[i] = width;
        }

        // Устаревшее. Так в оригинале.
        //GlyphRun glyphRun = new GlyphRun(hourFontGlyph, 0, false, fontSize,
        //    glyphIndexes, point, advanceWidths, null, null, null, null, null, null);

        // TODO: Тут надо поэкспериментировать с масштабом экрана.
        // Возможные варианты: 100%, 125%, 150% и 175%.
        var pixelsPerDip = 1.25f;  // мой масштаб 125%
        //var pixelsPerDip = 1f;  // масштаб 100%
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
    public Point GetHourPosition(int hour)
    {
        // Так в оригинале.
        //double angle = (30.0 * hour) - 90;
        //double rads = (Math.PI / 180) * angle;

        var angle = (Math.PI / 180) * ((30.0 * hour) - 90);
        return new Point((50 + HourRadius * Math.Cos(angle)), (50 + HourRadius * Math.Sin(angle)));
    }

    /// <summary>
    /// Возвращает размер указанной строки с указанным размером шрифта. 
    /// </summary>
    private Size MeasureString(string text, double fontSize)
    {
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
    /// Инициализирует таймер часов.
    /// </summary>
    protected void InitTimer(bool create)
    {
        // TODO: Надо ли убивать таймер при остановке или просто останавливать?
        if (create && timer == null)
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(100);
            //timer.Interval = TimeSpan.FromMilliseconds(10);
            timer.Tick += Timer_Tick;
            timer.Start();
        }
        else if (!create && timer != null)
        {
            timer.Stop();
            timer.Tick -= Timer_Tick;
            timer.Interval = TimeSpan.Zero;
            timer = null;
        }
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
    /// Register the "DateTime" property as a formal dependency property.
    /// </summary>
    public static DependencyProperty DateTimeProperty = DependencyProperty.Register(
            "DateTime",
            typeof(DateTime),
            typeof(ClockControl),
            new PropertyMetadata(DateTime.Now, new PropertyChangedCallback(OnDateTimePropertyChanged)));

    /// <summary>
    /// Register the "HourRadius" property as a formal dependency property.
    /// </summary>
    public static DependencyProperty HourRadiusProperty = DependencyProperty.Register(
            "HourRadius",
            typeof(double),
            typeof(ClockControl),
            new PropertyMetadata(36.0));

    /// <summary>
    /// Register the "IsRunning" property as a formal dependency property.
    /// </summary>
    public static DependencyProperty IsRunningProperty = DependencyProperty.Register(
            "IsRunning",
            typeof(bool),
            typeof(ClockControl),
            new PropertyMetadata(true, new PropertyChangedCallback(OnIsRunningPropertyChanged)));

    /// <summary>
    /// Will be called every time the ClockControl.DateTime property changes.
    /// </summary>
    private static void OnDateTimePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var clock = (ClockControl)d;
        var oldValue = (DateTime)e.OldValue;
        var newValue = (DateTime)e.NewValue;
        if (oldValue.Second != newValue.Second)
            clock.OnDateTimeChanged(oldValue, newValue.AddMilliseconds(-newValue.Millisecond));
    }

    /// <summary>
    /// Will be called every time the ClockControl.IsRunning property changes.
    /// </summary>
    private static void OnIsRunningPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var clock = (ClockControl)d;
        var oldValue = (bool)e.OldValue;
        var newValue = (bool)e.NewValue;
        if (oldValue != newValue)
            clock.OnIsRunningChanged(oldValue, newValue);
    }

    /// <summary>
    /// Set up a DateTimeChanged event.
    /// </summary>
    public static readonly RoutedEvent DateTimeChangedEvent =
        EventManager.RegisterRoutedEvent(
            "DateTimeChanged",
            RoutingStrategy.Bubble,
            typeof(RoutedPropertyChangedEventHandler<DateTime>),
            typeof(ClockControl));

    /// <summary>
    /// Set up an RunningChanged event.
    /// </summary>
    public static readonly RoutedEvent RunningChangedEvent =
        EventManager.RegisterRoutedEvent(
            "RunningChanged",
            RoutingStrategy.Bubble,
            typeof(RoutedPropertyChangedEventHandler<bool>),
            typeof(ClockControl));

    /// <summary>
    /// Fire the DateTimeChanged event when the time changes.
    /// </summary>
    protected virtual void OnDateTimeChanged(DateTime oldValue, DateTime newValue)
    {
        var args = new RoutedPropertyChangedEventArgs<DateTime>(oldValue, newValue)
        {
            RoutedEvent = DateTimeChangedEvent
        };
        RaiseEvent(args);
    }

    /// <summary>
    /// Fire the OnIsRunningChanged event when the motion type changes.
    /// </summary>
    /// <param name="oldValue"></param>
    /// <param name="newValue"></param>
    protected virtual void OnIsRunningChanged(bool oldValue, bool newValue)
    {
        InitTimer(newValue);
        var args = new RoutedPropertyChangedEventArgs<bool>(oldValue, newValue)
        {
            RoutedEvent = RunningChangedEvent
        };
        RaiseEvent(args);
    }

}
