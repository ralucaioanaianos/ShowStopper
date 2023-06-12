using System.Windows.Input;

namespace ShowStopper.CustomComponents;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class GradientFilledLabel 
{
    private static readonly BindableProperty BorderColorProperty
          = BindableProperty.Create(nameof(BorderColor), typeof(Color), typeof(GradientFilledLabel));

    public Color BorderColor
    {
        get => (Color)GetValue(BorderColorProperty);
        set => SetValue(BorderColorProperty, value);
    }

    private static readonly BindableProperty TextProperty
        = BindableProperty.Create(nameof(Text), typeof(string), typeof(GradientFilledLabel));

    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    private static readonly BindableProperty ButtonBackgroundColorProperty
            = BindableProperty.Create(nameof(ButtonBackgroundColor), typeof(GradientBrush),
                typeof(GradientFilledLabel));

    public GradientBrush ButtonBackgroundColor
    {
        get => (GradientBrush)GetValue(ButtonBackgroundColorProperty);
        set => SetValue(ButtonBackgroundColorProperty, value);
    }

    private static readonly BindableProperty GradientColor1Property
           = BindableProperty.Create(nameof(GradientColor1), typeof(Color), typeof(GradientFilledLabel));

    public Color GradientColor1
    {
        get => (Color)GetValue(GradientColor1Property);
        set => SetValue(GradientColor1Property, value);
    }

    private static readonly BindableProperty GradientColor2Property
           = BindableProperty.Create(nameof(GradientColor2), typeof(Color), typeof(GradientFilledLabel));

    public Color GradientColor2
    {
        get => (Color)GetValue(GradientColor2Property);
        set => SetValue(GradientColor2Property, value);
    }

    private static readonly BindableProperty TextColorProperty
            = BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(GradientFilledLabel), Colors.YellowGreen);

    public Color TextColor
    {
        get => (Color)GetValue(TextColorProperty);
        set => SetValue(TextColorProperty, value);
    }

    public GradientFilledLabel()
	{
		InitializeComponent();
	}
}