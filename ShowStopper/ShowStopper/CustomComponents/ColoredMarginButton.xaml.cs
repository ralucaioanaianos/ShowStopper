using System.Windows.Input;

namespace ShowStopper.CustomComponents;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class ColoredMarginButton
{
	private static readonly BindableProperty TextProperty
		= BindableProperty.Create(nameof(Text), typeof(string), typeof(ColoredMarginButton));

	public string Text
	{
		get => (string)GetValue(TextProperty);
		set => SetValue(TextProperty, value);
	}

    private static readonly BindableProperty ButtonBackgroundColorProperty
            = BindableProperty.Create(nameof(ButtonBackgroundColor), typeof(Color),
                typeof(ColoredMarginButton));

    public Color ButtonBackgroundColor
    {
        get => (Color)GetValue(ButtonBackgroundColorProperty);
        set => SetValue(ButtonBackgroundColorProperty, value);
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