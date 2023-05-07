using System.Windows.Input;

namespace ShowStopper.CustomComponents;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class ColoredMarginButton
{
    private static readonly BindableProperty BorderColorProperty
           = BindableProperty.Create(nameof(BorderColor), typeof(Color), typeof(ColoredMarginButton));

    public Color BorderColor
    {
        get => (Color)GetValue(BorderColorProperty);
        set => SetValue(BorderColorProperty, value);
    }


    private static readonly BindableProperty TextProperty
		= BindableProperty.Create(nameof(Text), typeof(string), typeof(ColoredMarginButton));



	public string Text
	{
		get => (string)GetValue(TextProperty);
		set => SetValue(TextProperty, value);
	}

    private static readonly BindableProperty ButtonBackgroundColorProperty
            = BindableProperty.Create(nameof(ButtonBackgroundColor), typeof(GradientBrush),
                typeof(ColoredMarginButton));

    public GradientBrush ButtonBackgroundColor
    {
        get => (GradientBrush)GetValue(ButtonBackgroundColorProperty);
        set => SetValue(ButtonBackgroundColorProperty, value);
    }

    private static readonly BindableProperty GradientColor1Property
           = BindableProperty.Create(nameof(GradientColor1), typeof(Color), typeof(ColoredMarginButton));

    public Color GradientColor1
    {
        get => (Color)GetValue(GradientColor1Property);
        set => SetValue(GradientColor1Property, value);
    }

    private static readonly BindableProperty GradientColor2Property
           = BindableProperty.Create(nameof(GradientColor2), typeof(Color), typeof(ColoredMarginButton));

    public Color GradientColor2
    {
        get => (Color)GetValue(GradientColor2Property);
        set => SetValue(GradientColor2Property, value);
    }

    private static readonly BindableProperty TextColorProperty
            = BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(ColoredMarginButton), Colors.YellowGreen);

    public Color TextColor
    {
        get => (Color)GetValue(TextColorProperty);
        set => SetValue(TextColorProperty, value);
    }

    public static readonly BindableProperty TapCommandProperty = BindableProperty.Create(
            nameof(TapCommand),
            typeof(ICommand),
            typeof(ColoredMarginButton));

    public ICommand TapCommand
    {
        get => (ICommand)GetValue(TapCommandProperty);
        set => SetValue(TapCommandProperty, value);
    }

    public static readonly BindableProperty TapCommandParameterProperty = BindableProperty.Create(
        nameof(TapCommandParameter),
        typeof(object),
        typeof(ColoredMarginButton));

    public object TapCommandParameter
    {
        get => GetValue(TapCommandParameterProperty);
        set => SetValue(TapCommandParameterProperty, value);
    }

    public ColoredMarginButton()
	{
		InitializeComponent();
	}
}